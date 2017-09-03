namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls
{
    using System;
    using JanHafner.Toolkit.Common.Ini;
    using JetBrains.Annotations;

    [IniSection("InternetShortcut")]
    public sealed class InternetShortcut
    {
        [CanBeNull]
        [IniKey("Url")]
        public String Url { get; set; }

        [CanBeNull]
        [IniKey("WorkingDirectory")]
        public String WorkingDirectory { get; set; }

        [IniKey("ShowCommand")]
        public Int32? ShowCommand { get; set; }

        [IniKey("IconIndex")]
        public Int32? IconIndex { get; set; }

        [CanBeNull]
        [IniKey("IconFile")]
        public String IconFile { get; set; }

        [IniKey("HotKey")]
        public Int32? HotKey { get; set; }
    }
}