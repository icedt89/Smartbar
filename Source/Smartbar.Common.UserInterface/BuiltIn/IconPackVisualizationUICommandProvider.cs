namespace JanHafner.Smartbar.Common.UserInterface.BuiltIn
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Common.UserInterface.Localization;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IImageVisualizationUICommandProvider))]
    internal sealed class IconPackVisualizationUICommandProvider : IImageVisualizationUICommandProvider
    {
        [NotNull]
        private readonly ISmartbarService smartbarService;

        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly ISelectableIconPacksProvider selectableIconPacksProvider;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [ImportingConstructor]
        public IconPackVisualizationUICommandProvider([NotNull] ISmartbarService smartbarService,
            [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] ISelectableIconPacksProvider selectableIconPacksProvider,
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

            if (selectableIconPacksProvider == null)
            {
                throw new ArgumentNullException(nameof(selectableIconPacksProvider));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            this.smartbarService = smartbarService;
            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.eventAggregator = eventAggregator;
            this.selectableIconPacksProvider = selectableIconPacksProvider;
            this.localizationService = localizationService;
        }

        public IDynamicUICommand CreateUICommand(Guid applicationId, Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return new IconPackVisualizationUICommand(() => this.localizationService.Localize<MenuItems>(nameof(MenuItems.ProcessApplicationButtonChangeDisplayImageFromIconPackResource)), this.smartbarService, this.windowService, this.eventAggregator, this.commandDispatcher, this.selectableIconPacksProvider, applicationId, canExecute);
        }
    }
}