namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class ProcessApplicationRenamed : PubSubEvent<ProcessApplicationRenamed.Data>
    {
        public sealed class Data
        {
            public Data(Guid applicationId, [NotNull] String name)
            {
                if (String.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                this.ApplicationId = applicationId;
                this.Name = name;
            }

            public Guid ApplicationId { get; private set; }

            public String Name { get; private set; }
        }
    }
}