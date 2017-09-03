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
    internal sealed class CreateGroupCommandHandler : CommandHandler<CreateGroupCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public CreateGroupCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(CreateGroupCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var newGroup = new Group(command.GroupName)
            {
                Id = command.GroupId
            };

            var groupPosition = this.smartbarDbContext.Groups.Select(_ => _.Position).DefaultIfEmpty(-1).Max() + 1;
            newGroup.Reposition(groupPosition);

            if (!this.smartbarDbContext.Groups.Any())
            {
                newGroup.Select();
            }

            this.smartbarDbContext.Groups.Add(newGroup);

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<GroupCreated>().Publish(newGroup);
            this.PublishCommandHandlerDone(command);
        }
    }
}
