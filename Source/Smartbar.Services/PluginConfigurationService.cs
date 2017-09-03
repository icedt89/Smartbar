namespace JanHafner.Smartbar.Services
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    [Export(typeof(IPluginConfigurationService))]
    internal sealed class PluginConfigurationService : IPluginConfigurationService
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public PluginConfigurationService([NotNull] ISmartbarDbContext smartbarDbContext)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public TPluginConfiguration GetConfigurationOrDefault<TPluginConfiguration>()
            where TPluginConfiguration : PluginConfiguration, new()
        {
            return this.smartbarDbContext.PluginConfigurations.OfType<TPluginConfiguration>().SingleOrDefault() ?? new TPluginConfiguration();
        }
    }
}
