namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using Common;
    using global::Smartbar.Updater.Core;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.Group;
    using JanHafner.Smartbar.Views.MainWindow;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ViewModelFactory))]
    internal sealed class ViewModelFactory
    {
        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly ISmartbarService smartbarService;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly IApplicationButtonFactory applicationButtonFactory;

        [NotNull]
        private readonly IPluginPackageManager pluginPackageManager;

        [NotNull] private readonly IUIExtensionService uiExtensionService;

        [NotNull]
        private readonly IModuleExplorer moduleExplorer;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [NotNull]
        private readonly IPluginService pluginService;

        [NotNull]
        private readonly IPluginConfigurationService pluginConfigurationService;

        [NotNull]
        private readonly ISmartbarUpdater smartbarUpdater;

        [NotNull]
        private readonly WpfHotKeyManager wpfHotKeyManager;

        [ImportingConstructor]
        public ViewModelFactory([NotNull] ISmartbarSettings smartbarSettings, [NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarService smartbarService,
            [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IApplicationButtonFactory applicationButtonFactory, [NotNull] IPluginPackageManager pluginPackageManager, [NotNull] IUIExtensionService uiExtensionService,
            [NotNull] IModuleExplorer moduleExplorer, [NotNull] ILocalizationService localizationService,
            [NotNull] IPluginService pluginService, [NotNull] IPluginConfigurationService pluginConfigurationService,
            [NotNull] ISmartbarUpdater smartbarUpdater, [NotNull] WpfHotKeyManager wpfHotKeyManager)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (applicationButtonFactory == null)
            {
                throw new ArgumentNullException(nameof(applicationButtonFactory));
            }

            if (pluginPackageManager == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageManager));
            }

            if (uiExtensionService == null)
            {
                throw new ArgumentNullException(nameof(uiExtensionService));
            }

            if (moduleExplorer == null)
            {
                throw new ArgumentNullException(nameof(moduleExplorer));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            if (pluginService == null)
            {
                throw new ArgumentNullException(nameof(pluginService));
            }

            if (pluginConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(pluginConfigurationService));
            }

            if (smartbarUpdater == null)
            {
                throw new ArgumentNullException(nameof(smartbarUpdater));
            }

            if (wpfHotKeyManager == null)
            {
                throw new ArgumentNullException(nameof(wpfHotKeyManager));
            }

            this.smartbarSettings = smartbarSettings;
            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.smartbarService = smartbarService;
            this.commandDispatcher = commandDispatcher;
            this.applicationButtonFactory = applicationButtonFactory;
            this.pluginPackageManager = pluginPackageManager;
            this.uiExtensionService = uiExtensionService;
            this.moduleExplorer = moduleExplorer;
            this.localizationService = localizationService;
            this.pluginService = pluginService;
            this.pluginConfigurationService = pluginConfigurationService;
            this.smartbarUpdater = smartbarUpdater;
            this.wpfHotKeyManager = wpfHotKeyManager;
        }

        public MainWindowViewModel CreateMainWindowViewModel()
        {
            var groups = this.smartbarService.GetGroups().ToList();
            var groupViewModels = groups.Select(group => this.CreateGroupViewModel(@group, groups)).ToList();

            return new MainWindowViewModel(groupViewModels, this.eventAggregator,
                this.smartbarService, this.smartbarSettings, this.windowService, this.commandDispatcher,
                this.pluginPackageManager, this, this.uiExtensionService,
                this.moduleExplorer, this.localizationService, this.pluginConfigurationService, this.smartbarUpdater, this.wpfHotKeyManager);
        }

        public GroupViewModel CreateGroupViewModel(Group group)
        {
            return new GroupViewModel(group, this.eventAggregator,
                this.windowService, this.commandDispatcher, this.smartbarService,
                this.smartbarSettings, this.applicationButtonFactory, this.pluginService);
        }

        private GroupViewModel CreateGroupViewModel(Group group, IReadOnlyCollection<Group> groups)
        {
            return new GroupViewModel(group, this.eventAggregator, this.windowService, this.commandDispatcher,
                this.smartbarService, this.smartbarSettings, this.applicationButtonFactory, this.pluginService)
            {
                CanBeDeleted = groups.Count > 1,
                CanShiftRight = @group.Position < groups.Max(_ => _.Position)
            };
        }
    }
}
