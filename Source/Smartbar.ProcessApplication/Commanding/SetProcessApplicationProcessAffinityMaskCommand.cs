namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public sealed class SetProcessApplicationProcessAffinityMaskCommand : ICommand
    {
        public SetProcessApplicationProcessAffinityMaskCommand(Guid applicationId, [CanBeNull] Int64? processAffinityMask)
        {
            if (processAffinityMask.HasValue && processAffinityMask.Value == 0)
            {
                throw new ArgumentNullException(nameof(processAffinityMask));
            }

            this.ApplicationId = applicationId;
            this.ProcessAffinityMask = processAffinityMask;
        }

        public Guid ApplicationId { get; private set; }

        [CanBeNull]
        public Int64? ProcessAffinityMask { get; private set; }
    }
}
