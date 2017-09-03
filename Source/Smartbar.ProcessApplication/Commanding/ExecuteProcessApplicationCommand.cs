namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;

    public sealed class ExecuteProcessApplicationCommand : ICommand
    {
        public ExecuteProcessApplicationCommand(Guid applicationId)
        {
            this.ApplicationId = applicationId;
        }

        public Guid ApplicationId { get; private set; }
    }
}
