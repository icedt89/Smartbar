namespace JanHafner.Smartbar.Common.UserInterface.BuiltIn
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;
    using JanHafner.Smartbar.Common.UserInterface.Localization;

    [Export(typeof (IImageVisualizationUICommandProvider))]
    internal sealed class IconImageVisualizationUICommandProvider : IImageVisualizationUICommandProvider
    {
        [NotNull] private readonly ISmartbarService smartbarService;

        [NotNull] private readonly IWindowService windowService;

        [NotNull] private readonly ICommandDispatcher commandDispatcher;

        [NotNull] private readonly IEventAggregator eventAggregator;

        [NotNull] private readonly IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider;

        [NotNull] private readonly ILocalizationService localizationService;

        [ImportingConstructor]
        public IconImageVisualizationUICommandProvider([NotNull] ISmartbarService smartbarService,
            [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider,
            [NotNull] ILocalizationService localizationService)
        {
            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (wellKnownIconLibraryUiCommandProvider == null)
            {
                throw new ArgumentNullException(nameof(wellKnownIconLibraryUiCommandProvider));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            this.smartbarService = smartbarService;
            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.eventAggregator = eventAggregator;
            this.wellKnownIconLibraryUiCommandProvider = wellKnownIconLibraryUiCommandProvider;
            this.localizationService = localizationService;
        }

        public IDynamicUICommand CreateUICommand(Guid applicationId, Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return new IconImageVisualizationUICommand(() => this.localizationService.Localize<MenuItems>(nameof(MenuItems.ProcessApplicationButtonChangeDisplayImageFromIcon)), this.smartbarService, this.eventAggregator, this.windowService, this.commandDispatcher, this.wellKnownIconLibraryUiCommandProvider, applicationId, canExecute);
        }
    }
}