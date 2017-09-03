namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class DiskManagementSystemToolMetadata : SystemToolMetadata
    {
        public DiskManagementSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "diskmgmt.msc"), nameof(DisplayTexts.Diskmgmt))
        {
        }
    }
}
