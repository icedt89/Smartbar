namespace JanHafner.Smartbar.ProcessApplication.Commanding
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
    public sealed class UpdateProcessApplicationCommandHandler : CommandHandler<UpdateProcessApplicationCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public UpdateProcessApplicationCommandHandler([NotNull] IEventAggregator eventAggregator,
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
            return await base.CanHandleAsync(command) && this.smartbarDbContext.Groups.SelectMany(g => g.Applications).OfType<ProcessApplication>()
                        .Any(
                            application =>
                                application.Id == ((UpdateProcessApplicationCommand) command).ApplicationId);
        }

        public override async Task HandleAsync(UpdateProcessApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var updatedProcessApplication = this.smartbarDbContext.Groups.SelectMany(g => g.Applications).OfType<ProcessApplication>()
                    .Single(application => application.Id == command.ApplicationId);

            updatedProcessApplication.Update(command.Execute,
                command.WorkingDirectory, command.Arguments,
                command.Priority, updatedProcessApplication.ProcessAffinityMask,
                command.StretchSmallImage, command.WindowStyle);

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<ApplicationUpdated>().Publish(command.ApplicationId);
            this.PublishCommandHandlerDone(command);
        }
    }
}
