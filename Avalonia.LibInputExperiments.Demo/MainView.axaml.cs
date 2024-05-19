using System;
using Avalonia.Controls;

namespace Avalonia.LibInputExperiments.Demo;

public partial class MainView : UserControl
{
  public MainView()
  {
    InitializeComponent();
    KeyDown += (sender, args) => 
    {
      Console.WriteLine($"KeyDown {args.PhysicalKey} {args.KeyModifiers} {args.Key} {args.KeySymbol}");
    };
    KeyUp += (sender, args) => 
    {
      Console.WriteLine($"KeyUp {args.PhysicalKey} {args.KeyModifiers} {args.Key} {args.KeySymbol}");
    };
    TextInput += (sender, args) => 
    {
      Console.WriteLine($"TextInput {args.Text}");
    };
  }

}