using Avalonia;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.LibInputExperiments;
using Avalonia.LibInputExperiments.Spike;

AvaloniaLocator.CurrentMutable.Bind<IKeyboardDevice>().ToConstant(new KeyboardDevice());

var lib = new LibInputBackend(
  new LibInputBackendOptions
  {
    Keyboard = new LibInputKeyboardConfiguration
    {
      Model = "pc105",
      Layout = "fr",
    }
  }
);

lib.Initialize(
  new ScreenInfo(),
  ev =>
  {
    switch (ev)
    {
      case RawPointerEventArgs p:
        Console.WriteLine($"{p.Type} {p.Point}");
        break;
      case RawKeyEventArgs k:
        Console.WriteLine($"{k.Type} {k.Modifiers} {k.PhysicalKey} {k.Key} {k.KeySymbol}");
        break;
    }
  });

lib.SetInputRoot(new InputRoot());


Console.WriteLine("Waiting for events...");
while (true)
{
  Thread.Sleep(1000);
}
