namespace JanHafner.Smartbar.Services
{
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    public interface IPluginConfigurationService
    {
        [NotNull]
        TPluginConfiguration GetConfigurationOrDefault<TPluginConfiguration>()
            where TPluginConfiguration : PluginConfiguration, new();
    }
}