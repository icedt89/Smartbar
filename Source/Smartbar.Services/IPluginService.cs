namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Application = JanHafner.Smartbar.Model.Application;

    public interface IPluginService
    {
        [CanBeNull]
        IApplicationCreationHandler TryFindHandler([NotNull] Object data);

        [CanBeNull]
        IImageVisualizationHandler TryFindHandler([NotNull] ApplicationImage data);

        [CanBeNull]
        IApplicationExecutionHandler TryFindHandler([NotNull] Application application);

        [NotNull]
        [LinqTunnel]
        IEnumerable<IDataObjectTranslator> FindDataObjectTranslators(IDataObject dataObject);
    }
}
