namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class EventViewerSystemToolMetadata : SystemToolMetadata
    {
        public EventViewerSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "eventvwr.exe"), nameof(DisplayTexts.Eventvwr))
        {
        }

        public override Boolean ShouldRunPostConfigureForSystemTool
        {
            get { return false; }
        }
    }
}
