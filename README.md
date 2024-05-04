# Avalonia.LibInputExperiments
Experimental lib to support more input events in AvaloniaUI on embedded Linux.

This library provides an extended implementation of the AvaloniaUI LibInputBackend.

The base LibInputBackend handle pointer and touch events.

This library adds support for
- keyboard events.

# Keyboard Events

The libinput events are translated into KeyEventArgs using the [xkbcommon](https://xkbcommon.org/) library.

The keyboard events translation is configured using standard XKB RMLVO (Rules, Model, Layout, Variant, Options) definitions.

See [xkbcommon source code and documentation](https://github.com/xkbcommon/libxkbcommon) for additional information.
