namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public sealed class AssignApplicationToGroupCommand : ICommand
    {
        public AssignApplicationToGroupCommand(Guid applicationId, Guid groupId)
        {
            this.ApplicationId = applicationId;
            this.GroupId = groupId;
        }

        public Guid ApplicationId { get; private set; }

        public Guid GroupId { get; private set; }
    }
}
