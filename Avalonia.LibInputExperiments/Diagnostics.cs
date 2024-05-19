using System;
using System.Diagnostics;
using System.Threading;

namespace Avalonia.LibInputExperiments;

public static class Diagnostics
{
  public static AppBuilder UseInputBackendDiagnostics(this AppBuilder builder)
  {
#if DEBUG
    AttachDebugger();
#endif
    return builder.AfterSetup(a =>
    {
#if DEBUG
      Console.WriteLine("Setup complete (DEBUG)");
#else
      Console.WriteLine("Setup complete (RELEASE)");
#endif
    });
  }
  
  public static void AttachDebugger()
  {
    Console.WriteLine("Waiting for debugger to attach");
    while (!Debugger.IsAttached)
    {
      Thread.Sleep(100);
    }
    Console.WriteLine("Debugger attached");
  }
}