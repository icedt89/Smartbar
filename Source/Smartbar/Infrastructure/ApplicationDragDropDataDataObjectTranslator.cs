namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;

    [Export(typeof(IDataObjectTranslator))]
    public sealed class ApplicationDragDropDataDataObjectTranslator : IDataObjectTranslator
    {
        public IEnumerable<Object> TranslateData(IDataObject dataObject)
        {
            var applicationDragDropData = (ApplicationDragDropData)dataObject.GetData(ApplicationDragDropData.Format);

            return applicationDragDropData.SourceApplicationIds.Select(applicationId => new ExtractedApplicationDragDropData
            {
                SourceGroupId = applicationDragDropData.SourceGroupId,
                SourceApplicationId = applicationId
            });
        }

        public Boolean CanTranslateData(IDataObject dataObject)
        {
            return dataObject.GetDataPresent(ApplicationDragDropData.Format);
        }
    }
}