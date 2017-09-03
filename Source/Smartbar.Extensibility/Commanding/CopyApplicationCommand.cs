namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public sealed class CopyApplicationCommand : InternalCreateApplicationCommand
    {
        public CopyApplicationCommand(Guid sourceGroupId, Guid sourceApplicationId) : base(sourceGroupId, sourceApplicationId)
        {
        }
    }
}