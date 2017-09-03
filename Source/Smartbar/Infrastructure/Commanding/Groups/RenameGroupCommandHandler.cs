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
    internal sealed class RenameGroupCommandHandler : CommandHandler<RenameGroupCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public RenameGroupCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(RenameGroupCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var group = this.smartbarDbContext.Groups.Single(_ => _.Id == command.GroupId);

            var oldGroupName = group.Name;
            group.Rename(command.GroupName);

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<GroupRenamed>().Publish(new GroupRenamed.Data(group, oldGroupName));
            this.PublishCommandHandlerDone(command);
        }
    }
}
