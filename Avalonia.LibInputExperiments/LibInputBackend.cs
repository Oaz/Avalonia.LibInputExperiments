#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            _options = options;
        }

        private string[] GetAvailableEvents()
        {
            return Directory.GetFiles("/dev/input", "event*");
        }

        private unsafe void InputThread(IntPtr ctx, LibInputBackendOptions options)
        {
            SetupKeyboard(options.Keyboard);
            var fd = libinput_get_fd(ctx);

            var activeEvents = new Dictionary<string, IntPtr>();

            foreach (var f in options.Events!)
            {
                var instance = libinput_path_add_device(ctx, f);
                activeEvents.TryAdd(f, instance);
            }

            var lastPoolUpdate = DateTime.Now;
            
            while (true)
            {
                if (options.ListenEventsChanges && (DateTime.Now - lastPoolUpdate).TotalMilliseconds >= options.ListenEventsChangesInterval)
                {
                    var events = GetAvailableEvents();

                    // Checks if the get events and active events both contain same events
                    foreach (var f in activeEvents.Keys)
                    {
                        if (events.Contains(f)) continue;
                        if (activeEvents.Remove(f, out var inputPtr))
                        {
                            // This create segmentation fault, not sure how to handle this
                            // For now we do not remove old devices ptr
                            /*if (inputPtr != IntPtr.Zero)
                            {
                                libinput_path_remove_device(inputPtr);
                            }*/
                        }
                    }
                    
                    // Adds events if not present in active events
                    foreach (var f in events)
                    {
                        if (activeEvents.ContainsKey(f)) continue;
                        var inputPtr = libinput_path_add_device(ctx, f);
                        activeEvents.TryAdd(f, inputPtr);
                    }
                    lastPoolUpdate = DateTime.Now;
                }

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
                ListenEventsChanges = _options?.ListenEventsChanges ?? false,
                ListenEventsChangesInterval = _options?.ListenEventsChangesInterval ?? 5000,
                Events = _options?.Events ?? GetAvailableEvents(),
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