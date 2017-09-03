namespace JanHafner.Smartbar.ProcessApplication.Plugins.ShellLinkFiles
{
    using System;
    using System.IO;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Toolkit.Windows.HotKey;
    using Microsoft.WindowsAPICodePack.Shell;
    using Shell32;

    internal static class ShellLinkHotKeyExtractor
    {
        public static Boolean TryGetHotKey(ShellFile shellFile, out HotKeyModifier hotKeyModifier, out Key hotKey)
        {
            hotKeyModifier = HotKeyModifier.None;
            hotKey = Key.None;

            var shellLinkFolderPath = Path.GetDirectoryName(shellFile.Path);
            var shellLinkFileName = Path.GetFileName(shellFile.ParsingName);

            var shellLinkDual = (IShellLinkDual) new Shell().NameSpace(shellLinkFolderPath).ParseName(shellLinkFileName).GetLink;

            if (shellLinkDual == null)
            {
                return false;
            }

            var systemHotKey = shellLinkDual.Hotkey;

            return SystemHotKeyLookupTable.TryGetHotKey(systemHotKey, out hotKeyModifier, out hotKey);
        }
    }
}