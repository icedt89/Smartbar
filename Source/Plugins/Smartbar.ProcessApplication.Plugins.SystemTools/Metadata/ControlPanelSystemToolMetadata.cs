namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class ControlPanelSystemToolMetadata : SystemToolMetadata
    {
        public ControlPanelSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "control.exe"), nameof(DisplayTexts.Control))
        {
        }

        public override Boolean ShouldRunPostConfigureForSystemTool
        {
            get { return false; }
        }
    }
}
