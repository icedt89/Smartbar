namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class WindowsFirewallSystemToolMetadata : SystemToolMetadata
    {
        public WindowsFirewallSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "wf.msc"), nameof(DisplayTexts.Wf))
        {
        }
    }
}
