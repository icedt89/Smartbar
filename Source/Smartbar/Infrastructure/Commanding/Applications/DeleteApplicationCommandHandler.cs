namespace JanHafner.Smartbar.Infrastructure.Commanding.Applications
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    internal sealed class DeleteApplicationCommandHandler : CommandHandler<DeleteApplicationCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public DeleteApplicationCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task<Boolean> CanHandleAsync(ICommand command)
        {
            var result = await base.CanHandleAsync(command);
            if (!result)
            {
                return false;
            }

            return this.smartbarDbContext.Groups.SelectMany(g => g.Applications)
                    .Any(
                        application =>
                            application.Id ==
                            ((DeleteApplicationCommand)command).ApplicationId);
        }

        public override async Task HandleAsync(DeleteApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var deletableApplication = this.smartbarDbContext.Groups.SelectMany(g => g.Applications).Single(application => application.Id == command.ApplicationId);

            var assignedGroup = this.smartbarDbContext.Groups.Single(group => group.Applications.Any(app => app.Id == deletableApplication.Id));
            assignedGroup.Applications.Remove(deletableApplication);

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(assignedGroup, deletableApplication));
            this.PublishCommandHandlerDone(command);
        }
    }
}
