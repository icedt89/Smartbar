namespace JanHafner.Smartbar.ProcessApplication.Commanding
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
    public sealed class RenameProcessApplicationCommandHandler : CommandHandler<RenameProcessApplicationCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public RenameProcessApplicationCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(RenameProcessApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var updatedApplication = this.smartbarDbContext.Groups.SelectMany(g => g.Applications).OfType<ProcessApplication>().Single(application => application.Id == command.ApplicationId);

            updatedApplication.Rename(command.Name);

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<ProcessApplicationRenamed>().Publish(new ProcessApplicationRenamed.Data(command.ApplicationId, command.Name));
            this.PublishCommandHandlerDone(command);
        }
    }
}
