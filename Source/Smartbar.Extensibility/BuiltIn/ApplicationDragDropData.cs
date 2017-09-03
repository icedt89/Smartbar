namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    public sealed class ApplicationDragDropData
    {
        public static readonly String Format = typeof (ApplicationDragDropData).FullName;

        public ApplicationDragDropData(Guid sourceGroupId, [NotNull] params Guid[] sourceApplicationIds)
        {
            if (sourceApplicationIds == null || !sourceApplicationIds.Any())
            {
                throw new ArgumentNullException(nameof(sourceApplicationIds));
            }

            this.SourceGroupId = sourceGroupId;
            this.SourceApplicationIds = sourceApplicationIds;
        }

        public Guid SourceGroupId { get; private set; }

        public IEnumerable<Guid> SourceApplicationIds { get; private set; }
    }
}
