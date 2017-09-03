namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    internal sealed class RenameGroupCommand : ICommand
    {
        public RenameGroupCommand(Guid groupId, [NotNull] String groupName)
        {
            if (String.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            this.GroupId = groupId;
            this.GroupName = groupName;
        }

        public Guid GroupId { get; private set; }

        [NotNull]
        public String GroupName { get; private set; }
    }
}
