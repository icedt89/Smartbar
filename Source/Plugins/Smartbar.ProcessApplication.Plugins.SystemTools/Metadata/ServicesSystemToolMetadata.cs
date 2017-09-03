namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class ServicesSystemToolMetadata : SystemToolMetadata
    {
        public ServicesSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "services.msc"), nameof(DisplayTexts.Services))
        {
        }
    }
}
