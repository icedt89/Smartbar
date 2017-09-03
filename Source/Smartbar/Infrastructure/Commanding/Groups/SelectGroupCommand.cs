namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;

    internal sealed class SelectGroupCommand : ICommand
    {
        public SelectGroupCommand(Guid groupId)
        {
            this.GroupId = groupId;
        }

        public Guid GroupId { get; private set; }
    }
}
