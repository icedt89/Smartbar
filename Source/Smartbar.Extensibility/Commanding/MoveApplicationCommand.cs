namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public sealed class MoveApplicationCommand : InternalCreateApplicationCommand
    {
        public MoveApplicationCommand(Guid sourceGroupId, Guid sourceApplicationId) : base(sourceGroupId, sourceApplicationId)
        {
        }
    }
}