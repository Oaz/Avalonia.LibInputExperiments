using Avalonia;
using System;
using Avalonia.Logging;
using Avalonia.Media;
using Avalonia.ReactiveUI;

namespace Avalonia.LibInputExperiments.Demo;

class Program
{
  [STAThread]
  public static void Main(string[] args) => BuildAvaloniaApp()
#if DEBUG
    .StartWithClassicDesktopLifetime(args);
#else
    .StartLinuxFbDev(args, inputBackend: CreateInputBackend());
#endif  
  private static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
#if DEBUG
      .UsePlatformDetect()
#endif  
      .WithInterFont()
      .With(new FontManagerOptions
      {
        DefaultFamilyName = "avares://Avalonia.Fonts.Inter/Assets#Inter"
      })
      .LogToTrace(level:LogEventLevel.Verbose)
      .UseReactiveUI()
      .AfterSetup(a =>
      {
        Console.WriteLine("Setup complete");
      });
  
  private static LibInputBackend CreateInputBackend()
  {
    return new LibInputBackend(
      new LibInputBackendOptions
      {
        Keyboard = new LibInputKeyboardConfiguration
        {
          Model = "pc105",
          Layout = "fr",
        }
      }
    );
  }

}