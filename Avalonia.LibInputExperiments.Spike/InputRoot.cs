using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;

namespace Avalonia.LibInputExperiments.Spike;

public class InputRoot : IInputRoot
{
  public bool Focus(NavigationMethod method = NavigationMethod.Unspecified, KeyModifiers keyModifiers = KeyModifiers.None)
  {
    throw new NotImplementedException();
  }

  public void AddHandler(RoutedEvent routedEvent, Delegate handler, RoutingStrategies routes = RoutingStrategies.Direct | RoutingStrategies.Bubble,
    bool handledEventsToo = false)
  {
    throw new NotImplementedException();
  }

  public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
  {
    throw new NotImplementedException();
  }

  public void RaiseEvent(RoutedEventArgs e)
  {
    throw new NotImplementedException();
  }

  public bool Focusable { get; }
  public bool IsEnabled { get; }
  public Cursor? Cursor { get; }
  public bool IsEffectivelyEnabled { get; }
  public bool IsEffectivelyVisible { get; }
  public bool IsKeyboardFocusWithin { get; }
  public bool IsFocused { get; }
  public bool IsHitTestVisible { get; }
  public bool IsPointerOver { get; }
  public List<KeyBinding> KeyBindings { get; }
  public event EventHandler<GotFocusEventArgs>? GotFocus;
  public event EventHandler<RoutedEventArgs>? LostFocus;
  public event EventHandler<KeyEventArgs>? KeyDown;
  public event EventHandler<KeyEventArgs>? KeyUp;
  public event EventHandler<TextInputEventArgs>? TextInput;
  public event EventHandler<PointerEventArgs>? PointerEntered;
  public event EventHandler<PointerEventArgs>? PointerExited;
  public event EventHandler<PointerPressedEventArgs>? PointerPressed;
  public event EventHandler<PointerEventArgs>? PointerMoved;
  public event EventHandler<PointerReleasedEventArgs>? PointerReleased;
  public event EventHandler<PointerWheelEventArgs>? PointerWheelChanged;
  public IKeyboardNavigationHandler KeyboardNavigationHandler { get; }
  public IFocusManager? FocusManager { get; }
  public IPlatformSettings? PlatformSettings { get; }
  public IInputElement? PointerOverElement { get; set; }
  public bool ShowAccessKeys { get; set; }
}