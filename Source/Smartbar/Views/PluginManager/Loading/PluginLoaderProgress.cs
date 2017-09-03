namespace JanHafner.Smartbar.Views.PluginManager.Loading
{
    using System;
    using JetBrains.Annotations;

    internal sealed class PluginLoaderProgress
    {
        public PluginLoaderProgress(
            [NotNull] PluginPackageViewModel pluginPackageViewModel, Boolean isFinished)
        {
            if (pluginPackageViewModel == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageViewModel));
            }

            this.PluginPackageViewModel = pluginPackageViewModel;
            this.IsFinished = isFinished;
        }

        public PluginPackageViewModel PluginPackageViewModel { get; private set; }

        public Boolean IsFinished { get; private set; }
    }
}