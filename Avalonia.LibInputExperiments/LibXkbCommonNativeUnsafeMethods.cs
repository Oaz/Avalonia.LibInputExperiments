using System;
using System.Runtime.InteropServices;

namespace Avalonia.LibInputExperiments;

class LibXkbCommonNativeUnsafeMethods
{
  private const string LibXkbCommon = "libxkbcommon.so.0";

  [DllImport(LibXkbCommon)]
  public static extern IntPtr xkb_context_new(XkbContextFlags flags);

  [DllImport(LibXkbCommon)]
  public static extern void xkb_context_unref(IntPtr context);

  [DllImport(LibXkbCommon)]
  public static extern IntPtr xkb_keymap_new_from_names(
    IntPtr context,
    ref XkbRuleNames names,
    XkbKeymapCompileFlags flags
  );

  [DllImport(LibXkbCommon)]
  public static extern void xkb_keymap_unref(IntPtr keymap);

  [DllImport(LibXkbCommon)]
  public static extern IntPtr xkb_state_new(IntPtr keymap);

  [DllImport(LibXkbCommon)]
  public static extern void xkb_state_unref(IntPtr state);

  [DllImport(LibXkbCommon)]
  public static extern XkbStateComponent xkb_state_update_key(
    IntPtr state,
    uint key,
    XkbKeyDirection direction
  );

  [DllImport(LibXkbCommon)]
  public static extern int xkb_state_key_get_utf8(
    IntPtr state,
    uint key,
    [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder buffer,
    int size
  );

  [DllImport(LibXkbCommon)]
  public static extern uint xkb_state_key_get_one_sym(IntPtr state, uint key);

  [DllImport(LibXkbCommon)]
  public static extern int xkb_keysym_get_name(
    uint keysym,
    [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder buffer,
    int size
  );

  [DllImport(LibXkbCommon)]
  public static extern uint xkb_keymap_num_mods(IntPtr keymap);

  [DllImport(LibXkbCommon)]
  public static extern string xkb_keymap_mod_get_name(IntPtr keymap, uint idx);

  [DllImport(LibXkbCommon, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
  public static extern uint xkb_keymap_mod_get_index(
    IntPtr keymap,
    [MarshalAs(UnmanagedType.LPStr)] string name
  );

  [DllImport(LibXkbCommon)]
  public static extern int xkb_state_mod_index_is_active(
    IntPtr state,
    uint idx,
    XkbStateComponent type
  );

  [StructLayout(LayoutKind.Sequential)]
  public struct XkbRuleNames
  {
    public string Rules;
    public string Model;
    public string Layout;
    public string Variant;
    public string Options;
  };

  [Flags]
  public enum XkbContextFlags
  {
    XKB_CONTEXT_NO_FLAGS = 0,
    XKB_CONTEXT_NO_DEFAULT_INCLUDES = (1 << 0),
    XKB_CONTEXT_NO_ENVIRONMENT_NAMES = (1 << 1)
  };

  [Flags]
  public enum XkbKeymapCompileFlags
  {
    XKB_KEYMAP_COMPILE_NO_FLAGS = 0
  };

  [Flags]
  public enum XkbStateComponent
  {
    XKB_STATE_MODS_DEPRESSED = (1 << 0),
    XKB_STATE_MODS_LATCHED = (1 << 1),
    XKB_STATE_MODS_LOCKED = (1 << 2),
    XKB_STATE_MODS_EFFECTIVE = (1 << 3),
    XKB_STATE_LAYOUT_DEPRESSED = (1 << 4),
    XKB_STATE_LAYOUT_LATCHED = (1 << 5),
    XKB_STATE_LAYOUT_LOCKED = (1 << 6),
    XKB_STATE_LAYOUT_EFFECTIVE = (1 << 7),
    XKB_STATE_LEDS = (1 << 8)
  };

  public enum XkbKeyDirection
  {
    XKB_KEY_UP,
    XKB_KEY_DOWN
  };
}