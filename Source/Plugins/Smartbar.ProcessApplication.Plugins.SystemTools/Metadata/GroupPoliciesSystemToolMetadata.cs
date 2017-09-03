namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class GroupPoliciesSystemToolMetadata : SystemToolMetadata
    {
        public GroupPoliciesSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "gpedit.msc"), nameof(DisplayTexts.Gpedit))
        {
        }
    }
}
