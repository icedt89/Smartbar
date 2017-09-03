namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    public sealed class PersistPluginConfigurationCommand : ICommand
    {
        public PersistPluginConfigurationCommand([NotNull] PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration == null)
            {
                throw new ArgumentNullException(nameof(pluginConfiguration));
            }

            this.PluginConfiguration = pluginConfiguration;
        }

        [NotNull]
        public PluginConfiguration PluginConfiguration { get; private set; }
    }
}