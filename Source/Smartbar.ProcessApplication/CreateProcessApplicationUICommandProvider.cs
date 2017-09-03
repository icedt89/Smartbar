namespace JanHafner.Smartbar.ProcessApplication
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof (ICreateApplicationUICommandProvider))]
    internal sealed class CreateProcessApplicationUICommandProvider : ICreateApplicationUICommandProvider
    {
        [NotNull] private readonly IWindowService windowService;

        [NotNull] private readonly ICommandDispatcher commandDispatcher;

        [NotNull] private readonly IEventAggregator eventAggregator;

        [NotNull] private readonly IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider;

        [NotNull] private readonly ISelectableIconPacksProvider selectableIconPacksProvider;

        [NotNull] private readonly WpfHotKeyManager wpfHotKeyManager;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [ImportingConstructor]
        public CreateProcessApplicationUICommandProvider([NotNull] ISmartbarService smartbarService,
            [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider,
            [NotNull] ISelectableIconPacksProvider selectableIconPacksProvider,
            [NotNull] WpfHotKeyManager wpfHotKeyManager, [NotNull] ILocalizationService localizationService)
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

            if (selectableIconPacksProvider == null)
            {
                throw new ArgumentNullException(nameof(selectableIconPacksProvider));
            }

            if (wpfHotKeyManager == null)
            {
                throw new ArgumentNullException(nameof(wpfHotKeyManager));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.eventAggregator = eventAggregator;
            this.wellKnownIconLibraryUiCommandProvider = wellKnownIconLibraryUiCommandProvider;
            this.selectableIconPacksProvider = selectableIconPacksProvider;
            this.wpfHotKeyManager = wpfHotKeyManager;
            this.localizationService = localizationService;
        }

        public IDynamicUICommand CreateUICommand(Guid groupId, Int32 column, Int32 row, Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return new CreateProcessApplicationUICommand(() => this.localizationService.Localize<Localization.ProcessApplication>(nameof(Localization.ProcessApplication.EmptyApplicationButtonCreateProcessApplication)), this.eventAggregator, this.windowService,
                this.commandDispatcher, this.wellKnownIconLibraryUiCommandProvider, this.selectableIconPacksProvider, this.wpfHotKeyManager,
                groupId, column, row, canExecute);
        }
    }
}