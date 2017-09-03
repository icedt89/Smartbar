namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    public sealed class ExecuteProcessApplicationCommandHandler : CommandHandler<ExecuteProcessApplicationCommand>
    {
        [NotNull]
        private readonly IPluginService pluginService;

        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public ExecuteProcessApplicationCommandHandler([NotNull] IPluginService pluginService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (pluginService == null)
            {
                throw new ArgumentNullException(nameof(pluginService));
            }

            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.pluginService = pluginService;
            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(ExecuteProcessApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var application = this.smartbarDbContext.Groups.SelectMany(g => g.Applications).Single(a => a.Id == command.ApplicationId);
            var applicationExecutionHandler = this.pluginService.TryFindHandler(application);

            applicationExecutionHandler.Execute(application);

            this.EventAggregator.GetEvent<ApplicationExecuted>().Publish(application);
            this.PublishCommandHandlerDone(command);

            await Task.Yield();
        }
    }
}
