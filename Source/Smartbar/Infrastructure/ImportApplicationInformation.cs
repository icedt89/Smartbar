namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class ImportApplicationInformation
    {
        public ImportApplicationInformation([NotNull] Object data, [NotNull] PositionInformation destination,
            [NotNull] IApplicationCreationHandler applicationCreationHandler)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (applicationCreationHandler == null)
            {
                throw new ArgumentNullException(nameof(applicationCreationHandler));
            }

            this.Data = data;
            this.Destination = destination;
            this.ApplicationCreationHandler = applicationCreationHandler;
            this.ApplicationCreateTargetBehavior = ApplicationCreateTargetBehavior.OverrideTarget;
        }

        [NotNull]
        public Object Data { get; private set; }

        [NotNull]
        public PositionInformation Destination { get; private set; }

        [NotNull]
        public IApplicationCreationHandler ApplicationCreationHandler { get; private set; }

        [CanBeNull]
        public ICreateApplicationCommand CreatedCommand { get; set; }

        [CanBeNull]
        public Exception OccuredException { get; set; }

        public ApplicationCreateTargetBehavior ApplicationCreateTargetBehavior { get; set; }

        public Boolean Processable
        {
            get
            {
                return  this.OccuredException == null && this.CreatedCommand != null;
            }
        }
    }
}
