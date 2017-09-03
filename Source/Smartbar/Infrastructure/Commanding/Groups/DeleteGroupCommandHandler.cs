namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    internal sealed class DeleteGroupCommandHandler : CommandHandler<DeleteGroupCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public DeleteGroupCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(DeleteGroupCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Group selectedGroup = null;
            var groupsToReposition = new List<GroupRepositioned.Data>();
           
            var deletedGroup = this.smartbarDbContext.Groups.Single(group => group.Id == command.GroupId);

            // if this group is selected then select the group before or after
            if (deletedGroup.IsSelected)
            {
                deletedGroup.Unselect();

                selectedGroup = this.smartbarDbContext.Groups.Max(_ => _.Position) == deletedGroup.Position
                    ? this.smartbarDbContext.Groups.Single(_ => _.Position == deletedGroup.Position - 1)
                    : this.smartbarDbContext.Groups.Single(_ => _.Position == deletedGroup.Position + 1);

                selectedGroup.Select();
            }

            // Shift all groups right from the deleted group in their position, one to the left, so no gaps are left.
            var groupsToRepositionLeft = this.smartbarDbContext.Groups.Where(groupLeft => groupLeft.Position > deletedGroup.Position);
            foreach (var groupLeft in groupsToRepositionLeft)
            {
                var newPosition = groupLeft.Position - 1;
                groupsToReposition.Add(new GroupRepositioned.Data(groupLeft, groupLeft.Position));
                groupLeft.Reposition(newPosition);
            }

            this.smartbarDbContext.Groups.Remove(deletedGroup);

            await this.smartbarDbContext.SaveChangesAsync();

            if (selectedGroup != null)
            {
                this.EventAggregator.GetEvent<GroupUnselected>().Publish(deletedGroup);
                this.EventAggregator.GetEvent<GroupSelected>().Publish(selectedGroup);
            }

            foreach (var group in groupsToReposition)
            {
                this.EventAggregator.GetEvent<GroupRepositioned>().Publish(group);
            }

            this.EventAggregator.GetEvent<GroupDeleted>().Publish(deletedGroup);
            this.PublishCommandHandlerDone(command);
        }
    }
}
