#nullable enable
using System;
using System.IO;
using System.Threading;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.LibInputExperiments.Helpers;
using Avalonia.LinuxFramebuffer.Input;
using static Avalonia.LibInputExperiments.LibInputNativeUnsafeMethods;

namespace Avalonia.LibInputExperiments
{
  public partial class LibInputBackend : IInputBackend
  {
    private IScreenInfoProvider? _screen;
    private IInputRoot? _inputRoot;
    private const string LibInput = nameof(LinuxFramebuffer) + "/" + nameof(Input) + "/" + nameof(LibInput);
    private Action<RawInputEventArgs>? _onInput;
    private readonly LibInputBackendOptions? _options;

    public LibInputBackend(LibInputBackendOptions? options = default)
    {
      Console.WriteLine("Using experimental LibInput backend");
      _options = options;
    }
    
    private unsafe void InputThread(IntPtr ctx, LibInputBackendOptions options)
    {
      Console.WriteLine("Starting InputThread");
      SetupKeyboard(options.Keyboard);
      var fd = libinput_get_fd(ctx);

      foreach (var f in options.Events!)
        libinput_path_add_device(ctx, f);
      while (true)
      {
        IntPtr ev;
        libinput_dispatch(ctx);
        while ((ev = libinput_get_event(ctx)) != IntPtr.Zero)
        {
          var type = libinput_event_get_type(ev);

          if (type >= LibInputEventType.LIBINPUT_EVENT_KEYBOARD_KEY
              && type <= LibInputEventType.LIBINPUT_EVENT_KEYBOARD_KEY)
            HandleKeyboard(ev, type);

          if (type >= LibInputEventType.LIBINPUT_EVENT_TOUCH_DOWN &&
              type <= LibInputEventType.LIBINPUT_EVENT_TOUCH_CANCEL)
            HandleTouch(ev, type);

          if (type >= LibInputEventType.LIBINPUT_EVENT_POINTER_MOTION
              && type <= LibInputEventType.LIBINPUT_EVENT_POINTER_AXIS)
            HandlePointer(ev, type);

          // if (type >= LibInputEventType.LIBINPUT_EVENT_TABLET_TOOL_AXIS
          //     && type <= LibInputEventType.LIBINPUT_EVENT_TABLET_TOOL_BUTTON)
          //     HandleTabletTool(ev, type);
          //
          // if (type >= LibInputEventType.LIBINPUT_EVENT_TABLET_PAD_BUTTON
          //     && type <= LibInputEventType.LIBINPUT_EVENT_TABLET_PAD_STRIP)
          //     HandleTabletPad(ev, type);
          //
          // if (type >= LibInputEventType.LIBINPUT_EVENT_GESTURE_SWIPE_BEGIN
          //     && type <= LibInputEventType.LIBINPUT_EVENT_GESTURE_PINCH_END)
          //     HandleGesture(ev, type);

          libinput_event_destroy(ev);
          libinput_dispatch(ctx);
        }

        pollfd pfd = new pollfd { fd = fd, events = 1 };
        NativeUnsafeMethods.poll(&pfd, new IntPtr(1), 10);
      }
    }

    private void ScheduleInput(RawInputEventArgs ev) => _onInput?.Invoke(ev);

    public void Initialize(IScreenInfoProvider screen, Action<RawInputEventArgs> onInput)
    {
      _screen = screen;
      _onInput = onInput;
      var ctx = libinput_path_create_context();
      var options = new LibInputBackendOptions()
      {
        Events = _options?.Events is null
          ? Directory.GetFiles("/dev/input", "event*")
          : _options.Events,
        Keyboard = _options?.Keyboard ?? new LibInputKeyboardConfiguration()
      };
      new Thread(() => InputThread(ctx, options))
      {
        Name = "Input Manager Worker",
        IsBackground = true
      }.Start();
    }

    public void SetInputRoot(IInputRoot root)
    {
      _inputRoot = root;
    }
  }
}