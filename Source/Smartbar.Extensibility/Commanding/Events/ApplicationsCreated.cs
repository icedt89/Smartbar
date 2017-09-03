namespace JanHafner.Smartbar.Extensibility.Commanding.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class ApplicationsCreated : PubSubEvent<ApplicationsCreated.Data>
    {
        public sealed class Data
        {
            public Data([NotNull] Group group, [NotNull] params Application[] applications)
            {
                if (group == null)
                {
                    throw new ArgumentNullException(nameof(group));
                }

                if (applications == null || !applications.Any())
                {
                    throw new ArgumentNullException(nameof(applications));
                }

                this.Applications = applications;
                this.Group = group;
            }

            [NotNull]
            public IEnumerable<Application> Applications { get; private set; }

            [NotNull]
            public Group Group { get; private set; }
        }
    }
}
