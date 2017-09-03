namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories
{
    using System;
    using System.IO;
    using JanHafner.Toolkit.Common.Ini;
    using JetBrains.Annotations;

    public static class DesktopIniHelper
    {
        [NotNull]
        public const String DesktopIniName = "desktop.ini";

        public static Boolean IsDeskptopIniPresent([NotNull] String directory)
        {
            return File.Exists(Path.Combine(directory, DesktopIniName));
        }

        public static Boolean TryGetShellClassInfo([NotNull] String directory, [CanBeNull] out ShellClassInfo shellClassInfo)
        {
            if (!IsDeskptopIniPresent(directory))
            {
                shellClassInfo = null;
                return false;
            }

            shellClassInfo = new ShellClassInfo();
            IniHelper.ReadIniSection(shellClassInfo, Path.Combine(directory, DesktopIniName));

            return true;
        }
    }
}
