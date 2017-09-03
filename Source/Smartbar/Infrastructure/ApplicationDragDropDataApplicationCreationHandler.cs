using System;

namespace JanHafner.Smartbar.Infrastructure
{
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;

    [Export(typeof(IApplicationCreationHandler))]
    internal sealed class ApplicationDragDropDataApplicationCreationHandler : IApplicationCreationHandler
    {
        public Boolean CanCreate(Object data)
        {
            return data is ExtractedApplicationDragDropData;
        }

        public ICreateApplicationCommand CreateCommand(Object data)
        {
            var applicationDragDropData = (ExtractedApplicationDragDropData) data;

            if (DragDropHelper.IsCopyAction)
            {
                return new CopyApplicationCommand(applicationDragDropData.SourceGroupId, applicationDragDropData.SourceApplicationId);
            }

            if (DragDropHelper.IsExchangeAction)
            {
                return new ExchangeApplicationCommand(applicationDragDropData.SourceGroupId, applicationDragDropData.SourceApplicationId);
            }

            return new MoveApplicationCommand(applicationDragDropData.SourceGroupId, applicationDragDropData.SourceApplicationId);
        }
    }
}
