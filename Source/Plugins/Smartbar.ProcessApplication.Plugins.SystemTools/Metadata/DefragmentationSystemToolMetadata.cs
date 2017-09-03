namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class DefragmentationSystemToolMetadata : SystemToolMetadata
    {
        public DefragmentationSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "dfrgui.exe"), nameof(DisplayTexts.Defrag))
        {
        }

        public override Boolean ShouldRunPostConfigureForSystemTool
        {
            get { return false; }
        }
    }
}
