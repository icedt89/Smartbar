namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    internal sealed class GroupRenamed : PubSubEvent<GroupRenamed.Data>
    {
        public sealed class Data
        {
            public Data([NotNull] Group group, [NotNull] String oldGroupName)
            {
                if (group == null)
                {
                    throw new ArgumentNullException(nameof(group));
                }

                if (String.IsNullOrWhiteSpace(oldGroupName))
                {
                    throw new ArgumentNullException(nameof(oldGroupName));
                }

                this.Group = group;
                this.OldGroupName = oldGroupName;
            }

            [NotNull]
            public Group Group { get; private set; }

            [NotNull]
            public String OldGroupName { get; private set; }
        }
    }
}
