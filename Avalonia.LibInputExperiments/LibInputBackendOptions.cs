#nullable enable
using System.Collections.Generic;

namespace Avalonia.LibInputExperiments;

/// <summary>
/// LibInputBackend Options.
/// </summary>
public sealed record class LibInputBackendOptions
{
    /// <summary>
    /// List Events of events handler to monitoring eg: /dev/eventX.
    /// </summary>
    public IReadOnlyList<string>? Events { get; init; } = null;
    public LibInputKeyboardConfiguration Keyboard { get; init; } = new ();
}

public sealed record class LibInputKeyboardConfiguration
{
    public string Rules { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string Layout { get; init; } = string.Empty;
    public string Variant { get; init; } = string.Empty;
    public string Options { get; init; } = string.Empty;
}
