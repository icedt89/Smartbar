namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler;
    using JanHafner.Smartbar.ProcessApplication.Plugins.Urls.CreateApplicationStrategy;

    [Export(typeof(IApplicationCreationHandler))]
    [HandlerOverride(typeof(ProcessApplicationDragDropHandler))]
    public class UrlDragDropHandler : IApplicationCreationHandler
    {
        public Boolean CanCreate(Object data)
        {
            var stringData = data as String;
            if (stringData == null)
            {
                return false;
            }

            return CreateApplicationStrategySelector.SelectCreateApplicationStrategy(stringData) != null;
        }

        public ICreateApplicationCommand CreateCommand(object data)
        {
            var stringData = (String)data;

            var createApplicationStrategy = CreateApplicationStrategySelector.SelectCreateApplicationStrategy(stringData);

            return createApplicationStrategy.CreateContainerCommand(stringData);
        }
    }
}
