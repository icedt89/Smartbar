namespace JanHafner.Smartbar.ProcessApplication.Plugins.ShellLinkFiles
{
    using System;
    using JanHafner.Smartbar.Model;

    public sealed class ShellLinkDragDropHandlerPluginConfiguration : PluginConfiguration
    {
        public Boolean TryDeleteSourceShellLink { get; internal set; }
    }
}
