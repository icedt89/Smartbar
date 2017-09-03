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
    public sealed class SetProcessApplicationProcessAffinityMaskCommandHandler : CommandHandler<SetProcessApplicationProcessAffinityMaskCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public SetProcessApplicationProcessAffinityMaskCommandHandler([NotNull] IEventAggregator eventAggregator,
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
                                application.Id == ((SetProcessApplicationProcessAffinityMaskCommand) command).ApplicationId);
        }

        public override async Task HandleAsync(SetProcessApplicationProcessAffinityMaskCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var updatedApplication = this.smartbarDbContext.Groups.SelectMany(g => g.Applications).OfType<ProcessApplication>()
                    .Single(application => application.Id == command.ApplicationId);

            updatedApplication.Update(updatedApplication.Execute, updatedApplication.WorkingDirectory,
                updatedApplication.Arguments, updatedApplication.Priority, command.ProcessAffinityMask, updatedApplication.StretchSmallImage, updatedApplication.WindowStyle);

            await this.smartbarDbContext.SaveChangesAsync();

            this.PublishCommandHandlerDone(command);
        }
    }
}
