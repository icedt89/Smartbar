namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;

    public sealed class ExchangeApplicationCommand : InternalCreateApplicationCommand
    {
        public ExchangeApplicationCommand(Guid sourceGroupId, Guid sourceApplicationId) : base(sourceGroupId, sourceApplicationId)
        {
        }
    }
}