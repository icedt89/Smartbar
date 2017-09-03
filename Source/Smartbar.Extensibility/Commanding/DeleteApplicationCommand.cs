namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public sealed class DeleteApplicationCommand : ICommand
    {
        public DeleteApplicationCommand(Guid applicationId)
        {
            this.ApplicationId = applicationId;
        }

        public Guid ApplicationId { get; private set; }
    }
}
