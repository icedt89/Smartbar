namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
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
    internal sealed class RepositionGroupCommandHandler : CommandHandler<RepositionGroupCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public RepositionGroupCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(RepositionGroupCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var groupOne = this.smartbarDbContext.Groups.Single(_ => _.Id == command.GroupId);
            if (groupOne.Position == command.Position)
            {
                return;
            }

            var groupTwo = this.smartbarDbContext.Groups.Single(_ => _.Position == command.Position);

            var groupOneArguments = new GroupRepositioned.Data(groupOne, groupOne.Position);
            var groupTwoArguments = new GroupRepositioned.Data(groupTwo, groupTwo.Position);

            groupOne.Reposition(command.Position);
            groupTwo.Reposition(groupOneArguments.OldPosition);

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<GroupRepositioned>().Publish(groupOneArguments);
            this.EventAggregator.GetEvent<GroupRepositioned>().Publish(groupTwoArguments);
            this.PublishCommandHandlerDone(command);
        }
    }
}
