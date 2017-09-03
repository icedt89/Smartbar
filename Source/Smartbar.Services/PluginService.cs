namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;
    using Application = JanHafner.Smartbar.Model.Application;

    [Export(typeof(IPluginService))]
    internal sealed class PluginService : IPluginService
    {
        [NotNull]
        private readonly IEnumerable<IApplicationExecutionHandler> applicationExecutionHandler;

        [NotNull]
        private readonly IEnumerable<IImageVisualizationHandler> imageVisualizationHandler;

        [NotNull]
        private readonly IEnumerable<IApplicationCreationHandler> applicationCreationHandler;

        [NotNull]
        private readonly IEnumerable<IDataObjectTranslator> dataObjectTranslator;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public PluginService(
            [NotNull, ImportMany(typeof (IApplicationExecutionHandler))] IEnumerable<IApplicationExecutionHandler> applicationExecutionHandler,
            [NotNull, ImportMany(typeof (IImageVisualizationHandler))] IEnumerable<IImageVisualizationHandler> imageVisualizationHandler,
            [NotNull, ImportMany(typeof (IApplicationCreationHandler))] IEnumerable<IApplicationCreationHandler> applicationCreationHandler,
            [NotNull, ImportMany(typeof (IDataObjectTranslator))] IEnumerable<IDataObjectTranslator> dataObjectTranslator,
            [NotNull] IEventAggregator eventAggregator)
        {
            if (applicationExecutionHandler == null)
            {
                throw new ArgumentNullException(nameof(applicationExecutionHandler));
            }

            if (imageVisualizationHandler == null)
            {
                throw new ArgumentNullException(nameof(imageVisualizationHandler));
            }

            if (applicationCreationHandler == null)
            {
                throw new ArgumentNullException(nameof(applicationCreationHandler));
            }

            if (dataObjectTranslator == null)
            {
                throw new ArgumentNullException(nameof(dataObjectTranslator));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.applicationExecutionHandler = applicationExecutionHandler;
            this.imageVisualizationHandler = imageVisualizationHandler;
            this.applicationCreationHandler = applicationCreationHandler;
            this.dataObjectTranslator = dataObjectTranslator;
            this.eventAggregator = eventAggregator;
        }

        public IApplicationCreationHandler TryFindHandler(Object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var foundHandler = this.applicationCreationHandler.Where(handler =>
            {
                try
                {
                    return handler.CanCreate(data);
                }
                catch (Exception ex)
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                    return false;
                }
            }).ToList();

            return foundHandler.Overridden().SingleOrDefault();
        }

        public IImageVisualizationHandler TryFindHandler(ApplicationImage applicationImage)
        {
            if (applicationImage == null)
            {
                throw new ArgumentNullException(nameof(applicationImage));
            }

            var foundHandler = this.imageVisualizationHandler.Where(handler =>
            {
                try
                {
                    return handler.CanVisualize(applicationImage);

                }
                catch (Exception ex)
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                    return false;
                }
            }).ToList();

            return foundHandler.Overridden().SingleOrDefault();
        }

        public IApplicationExecutionHandler TryFindHandler(Application application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var foundHandler = this.applicationExecutionHandler.Where(handler =>
            {
                try
                {
                    return handler.CanExecute(application);
                }
                catch (Exception ex)
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                    return false;
                }
            }).ToList();

            return foundHandler.Overridden().SingleOrDefault();
        }

        [LinqTunnel]
        public IEnumerable<IDataObjectTranslator> FindDataObjectTranslators(IDataObject dataObject)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject));
            }

            return this.dataObjectTranslator.Where(dataObjectDataTranslator =>
            {
                try
                {
                    return dataObjectDataTranslator.CanTranslateData(dataObject);
                }
                catch (Exception ex)
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                    return false;
                }
            });
        }
    }
}