namespace JanHafner.Smartbar.Views.PluginManager.Loading
{
    using System;

    internal sealed class PluginLoaderResult
    {
        public PluginLoaderResult(Boolean pluginsLoaded, Boolean onlinePackageSourceUnavailable)
        {
            this.PluginsLoaded = pluginsLoaded;
            this.OnlinePackageSourceUnavailable = onlinePackageSourceUnavailable;
        }

        public Boolean PluginsLoaded { get; private set; }

        public Boolean OnlinePackageSourceUnavailable { get; private set; }
    }
}
