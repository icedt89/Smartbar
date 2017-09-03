namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories
{
    using System;
    using JanHafner.Toolkit.Common.Ini;
    using JetBrains.Annotations;

    [IniSection(".ShellClassInfo")]
    public sealed class ShellClassInfo
    {
        [CanBeNull]
        [IniKey("IconFile")]
        public String IconFile { get; set; }

        [IniKey("IconIndex")]
        public Int32? IconIndex { get; set; }

        [CanBeNull]
        [IniKey("InfoTip")]
        public String InfoTip { get; set; }

        [CanBeNull]
        [IniKey("IconResource")]
        public String IconResource { get; set; }

        [CanBeNull]
        [IniKey("LocalizedResourceName")]
        public String LocalizedResourceName { get; set; }
    }
}