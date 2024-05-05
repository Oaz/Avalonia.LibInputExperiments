using ReactiveUI;

namespace Avalonia.LibInputExperiments.Demo;

public class Context : ReactiveObject
{
  public Context()
  {
    CtrlW();
  }

  public void CtrlW()
  {
    Text = "Welcome to Avalonia.LibInputExperiments";
  }
  
  public void CtrlH()
  {
    Text = "Hello, World!";
  }
  
  public string Text
  {
    get => _text;
    set => this.RaiseAndSetIfChanged(ref _text, value);
  }

  private string _text = "";
}