namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class ComputerManagementSystemToolMetadata : SystemToolMetadata
    {
        public ComputerManagementSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "compmgmt.msc"), nameof(DisplayTexts.Compmgmt))
        {
        }
    }
}
