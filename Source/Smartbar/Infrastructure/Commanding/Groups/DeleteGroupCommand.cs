namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;

    internal sealed class DeleteGroupCommand : ICommand
    {
        public DeleteGroupCommand(Guid groupId)
        {
            this.GroupId = groupId;
        }

        public Guid GroupId { get; private set; }
    }
}
