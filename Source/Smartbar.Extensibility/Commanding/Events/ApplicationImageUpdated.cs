namespace JanHafner.Smartbar.Extensibility.Commanding.Events
{
    using System;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class ApplicationImageUpdated : PubSubEvent<ApplicationImageUpdated.Data>
    {
        public sealed class Data
        {
            public Data(Guid applicationWithImageId, [NotNull] Type oldApplicationImageType,
                [NotNull] Type newApplicationImageType)
            {
                if (oldApplicationImageType == null)
                {
                    throw new ArgumentNullException(nameof(oldApplicationImageType));
                }

                if (newApplicationImageType == null)
                {
                    throw new ArgumentNullException(nameof(newApplicationImageType));
                }

                this.ApplicationWithImageId = applicationWithImageId;
                this.OldApplicationImageType = oldApplicationImageType;
                this.NewApplicationImageType = newApplicationImageType;
            }

            public Guid ApplicationWithImageId { get; private set; }

            public Type OldApplicationImageType { get; private set; }

            public Type NewApplicationImageType { get; private set; }
        }
    }
}
