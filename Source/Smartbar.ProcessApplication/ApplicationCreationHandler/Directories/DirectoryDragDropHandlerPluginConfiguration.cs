namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories
{
    using System;
    using JanHafner.Smartbar.Model;

    public sealed class DirectoryDragDropHandlerPluginConfiguration : PluginConfiguration
    {
        public DirectoryDragDropHandlerPluginConfiguration()
        {
            this.ProcessDesktopIni = true;
        }

        public Boolean ProcessDesktopIni { get; internal set; }
    }
}
