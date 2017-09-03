namespace JanHafner.Smartbar.Infrastructure.Commanding.Plugins
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    internal sealed class PersistPluginConfigurationCommandHandler : CommandHandler<PersistPluginConfigurationCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public PersistPluginConfigurationCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(PersistPluginConfigurationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var pluginConfigurationAssemblyQualifiedTypeName = command.PluginConfiguration.GetType().AssemblyQualifiedName;

            var existingPluginConfiguration = this.smartbarDbContext.PluginConfigurations.SingleOrDefault(pc => pc.GetType().AssemblyQualifiedName == pluginConfigurationAssemblyQualifiedTypeName);
            if (existingPluginConfiguration != null)
            {
                this.smartbarDbContext.PluginConfigurations.Remove(existingPluginConfiguration);
            }

            this.smartbarDbContext.PluginConfigurations.Add(command.PluginConfiguration);

            await this.smartbarDbContext.SaveChangesAsync();

            this.PublishCommandHandlerDone(command);
        }
    }
}
