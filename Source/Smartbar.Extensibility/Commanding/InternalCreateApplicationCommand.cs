namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public abstract class InternalCreateApplicationCommand : ICreateApplicationCommand
    {
        protected InternalCreateApplicationCommand(Guid sourceGroupId, Guid sourceApplicationId)
        {
            if (sourceGroupId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(sourceGroupId));
            }

            if (sourceApplicationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(sourceApplicationId));
            }

            this.SourceGroupId = sourceGroupId;
            this.SourceApplicationId = sourceApplicationId;
        }

        public Guid SourceGroupId { get; private set; }

        public Guid SourceApplicationId { get; private set; }

        public Guid TargetGroupId { get; set; }

        public Int32 TargetRow { get; set; }

        public Int32 TargetColumn { get; set; }

        public ApplicationCreateTargetBehavior ApplicationCreateTargetBehavior { get; set; }
    }
}