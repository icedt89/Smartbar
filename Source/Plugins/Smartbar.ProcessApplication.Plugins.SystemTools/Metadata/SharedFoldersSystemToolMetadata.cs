namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class SharedFoldersSystemToolMetadata : SystemToolMetadata
    {
        public SharedFoldersSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "fsmgmt.msc"), nameof(DisplayTexts.Fsmgmt))
        {
        }
    }
}
