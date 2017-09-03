namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IUIExtensionService))]
    internal sealed class UIExtensionService : IUIExtensionService
    {
        [NotNull]
        private readonly IEnumerable<IImageVisualizationUICommandProvider> imageVisualizationUiCommandProviders;

        [NotNull]
        private readonly IEnumerable<ICreateApplicationUICommandProvider> createApplicationUiCommandProviders;

        [NotNull]
        private readonly IEnumerable<IShowPluginConfigurationUICommandProvider> showPluginConfigurationUiCommandProviders;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public UIExtensionService([ImportMany(typeof(IImageVisualizationUICommandProvider))] [NotNull] IEnumerable<IImageVisualizationUICommandProvider> imageVisualizationUICommandProviders,
            [ImportMany(typeof(ICreateApplicationUICommandProvider))] [NotNull] IEnumerable<ICreateApplicationUICommandProvider> createApplicationUICommandProviders,
            [NotNull, ImportMany(typeof (IShowPluginConfigurationUICommandProvider))] IEnumerable<IShowPluginConfigurationUICommandProvider> showPluginConfigurationUICommandProviders,
            [NotNull] IEventAggregator eventAggregator)
        {
            if (imageVisualizationUICommandProviders == null)
            {
                throw new ArgumentNullException(nameof(imageVisualizationUICommandProviders));
            }

            if (createApplicationUICommandProviders == null)
            {
                throw new ArgumentNullException(nameof(createApplicationUICommandProviders));
            }

            if (showPluginConfigurationUICommandProviders == null)
            {
                throw new ArgumentNullException(nameof(showPluginConfigurationUICommandProviders));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.imageVisualizationUiCommandProviders = imageVisualizationUICommandProviders;
            this.createApplicationUiCommandProviders = createApplicationUICommandProviders;
            this.showPluginConfigurationUiCommandProviders = showPluginConfigurationUICommandProviders;
            this.eventAggregator = eventAggregator;
        }

        [LinqTunnel]
        public IEnumerable<IDynamicUICommand> CreateImageVisualizationCommands(Guid applicationId, Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return this.imageVisualizationUiCommandProviders.Select(imageVisualizationUiCommandProvider =>
            {
                try
                {
                    return imageVisualizationUiCommandProvider.CreateUICommand(applicationId, canExecute);
                }
                catch (Exception ex)
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                    return null;
                }
                
            }).Where(uiCommand => uiCommand != null);
        }

        [LinqTunnel]
        public IEnumerable<IDynamicUICommand> CreateCreateApplicationCommands(Guid groupId, Int32 column, Int32 row, Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return this.createApplicationUiCommandProviders.Select(createApplicationUiCommandProvider =>
            {
                try
                {
                    return createApplicationUiCommandProvider.CreateUICommand(groupId, column, row, canExecute);
                }
                catch (Exception ex)
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                    return null;
                }
            }).Where(uiCommand => uiCommand != null);
        }

        [LinqTunnel]
        public IEnumerable<IDynamicUICommand> GetConfigurablePluginsUICommands([NotNull] Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return
                this.showPluginConfigurationUiCommandProviders.Select(
                    showPluginConfigurationUiCommandProvider =>
                    {
                        try
                        {
                            return showPluginConfigurationUiCommandProvider.CreateUICommand(canExecute);
                        }
                        catch (Exception ex)
                        {
                            this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                            return null;
                        }
                    }).Where(uiCommand => uiCommand != null);
        }
    }
}
