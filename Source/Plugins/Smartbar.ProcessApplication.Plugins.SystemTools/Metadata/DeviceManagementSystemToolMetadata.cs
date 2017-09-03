namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class DeviceManagementSystemToolMetadata : SystemToolMetadata
    {
        public DeviceManagementSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "devmgmt.msc"), nameof(DisplayTexts.Devmgmt))
        {
        }
    }
}
