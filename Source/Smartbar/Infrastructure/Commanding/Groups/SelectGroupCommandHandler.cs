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
    internal sealed class SelectGroupCommandHandler : CommandHandler<SelectGroupCommand>
    {
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public SelectGroupCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(SelectGroupCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var selectedGroup = this.smartbarDbContext.Groups.Single(group => group.Id == command.GroupId);
            var unselectedGroup = this.smartbarDbContext.Groups.Single(_ => _.IsSelected);
            unselectedGroup.Unselect();

            selectedGroup.Select();

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<GroupUnselected>().Publish(unselectedGroup);
            this.EventAggregator.GetEvent<GroupSelected>().Publish(selectedGroup);
            this.PublishCommandHandlerDone(command);
        }
    }
}
