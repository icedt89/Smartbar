namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class SecurityPoliciesSystemToolMetadata : SystemToolMetadata
    {
        public SecurityPoliciesSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "secpol.msc"), nameof(DisplayTexts.Secpol))
        {
        }
    }
}
