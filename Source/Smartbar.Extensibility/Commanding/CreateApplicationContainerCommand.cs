namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public abstract class CreateApplicationContainerCommand : CompositeCommand, ICreateApplicationCommand
    {
        public Guid TargetGroupId { get; set; }

        public Int32 TargetRow { get; set; }

        public Int32 TargetColumn { get; set; }

        public ApplicationCreateTargetBehavior ApplicationCreateTargetBehavior { get; set; }
    }
}
