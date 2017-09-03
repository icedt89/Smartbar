namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
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
    internal sealed class ClearGroupCommandHandler : CommandHandler<ClearGroupCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public ClearGroupCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(ClearGroupCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var clearGroup = this.smartbarDbContext.Groups.Single(group => group.Id == command.GroupId);

            clearGroup.Applications.Clear();

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<GroupCleared>().Publish(clearGroup);
            this.PublishCommandHandlerDone(command);
        }
    }
}
