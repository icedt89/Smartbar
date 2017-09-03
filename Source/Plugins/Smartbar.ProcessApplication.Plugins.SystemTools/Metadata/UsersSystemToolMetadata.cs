namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class UsersSystemToolMetadata : SystemToolMetadata
    {
        public UsersSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "lusrmgr.msc"), nameof(DisplayTexts.Users))
        {
        }
    }
}
