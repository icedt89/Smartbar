namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class MsConfigSystemToolMetadata : SystemToolMetadata
    {
        public MsConfigSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "msconfig.exe"), nameof(DisplayTexts.Msconfig))
        {
        }

        public override Boolean ShouldRunPostConfigureForSystemTool
        {
            get { return false; }
        }
    }
}
