namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;

    internal sealed class ClearGroupCommand : ICommand
    {
        public ClearGroupCommand(Guid groupId)
        {
            this.GroupId = groupId;
        }

        public Guid GroupId { get; private set; }
    }
}
