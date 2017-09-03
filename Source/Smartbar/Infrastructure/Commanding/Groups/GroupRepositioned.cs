namespace JanHafner.Smartbar.Infrastructure.Commanding.Groups
{
    using System;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    internal sealed class GroupRepositioned : PubSubEvent<GroupRepositioned.Data>
    {
        public sealed class Data
        {
            public Data([NotNull] Group group, Int32 oldPosition)
            {
                if (@group == null)
                {
                    throw new ArgumentNullException(nameof(@group));
                }

                this.Group = group;
                this.OldPosition = oldPosition;
            }

            public Group Group { get; private set; }

            public Int32 OldPosition { get; private set; }
        }
    }
}
