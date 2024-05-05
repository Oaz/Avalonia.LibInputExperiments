using Avalonia;
using Avalonia.Input;
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
  ev => { }
  );

lib.SetInputRoot(new InputRoot());


Console.WriteLine("Waiting for events...");
while (true)
{
  Thread.Sleep(1000);
}
