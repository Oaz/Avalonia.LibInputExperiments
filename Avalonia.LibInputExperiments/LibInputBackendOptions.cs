#nullable enable
using System.Collections.Generic;

namespace Avalonia.LibInputExperiments;

/// <summary>
/// LibInputBackend Options.
/// </summary>
public sealed record class LibInputBackendOptions
{
    /// <summary>
    /// Sets to listen for event changes, if true it will listen for events changes and register them to make the device work without restarting the application.
    /// </summary>
    public bool ListenEventsChanges { get; init; }

    /// <summary>
    /// Sets the listen for event changes interval in milliseconds.<br/>
    /// Requires <see cref="ListenEventsChanges"/> to be true.
    /// </summary>
    public int ListenEventsChangesInterval { get; init; } = 5000;

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
