using Avalonia.LinuxFramebuffer.Input;

namespace Avalonia.LibInputExperiments.Spike;

public class ScreenInfo : IScreenInfoProvider
{
  public Size ScaledSize { get; } = new Size(1920, 1080);
}