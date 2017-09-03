namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using System;
    using System.ComponentModel.Composition;
    using Common;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IApplicationButtonProvider))]
    internal sealed class ProcessApplicationButtonProvider : IApplicationButtonProvider
    {
        [NotNull] 
        private readonly ISmartbarService smartbarService;

        [NotNull] 
        private readonly IPluginService pluginService;

        [NotNull] 
        private readonly IWindowService windowService;

        [NotNull] 
        private readonly IEventAggregator eventAggregator;

        [NotNull] 
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly IUIExtensionService uiExtensionService;

        [NotNull]
        private readonly WpfHotKeyManager wpfHotKeyManager;

        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [ImportingConstructor]
        public ProcessApplicationButtonProvider([NotNull] ISmartbarService smartbarService,
            [NotNull] IPluginService pluginService, [NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] ISmartbarSettings smartbarSettings, [NotNull] IUIExtensionService uiExtensionService,
            [NotNull] WpfHotKeyManager wpfHotKeyManager)
        {
            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (pluginService == null)
            {
                throw new ArgumentNullException(nameof(pluginService));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (uiExtensionService == null)
            {
                throw new ArgumentNullException(nameof(uiExtensionService));
            }

            if (wpfHotKeyManager == null)
            {
                throw new ArgumentNullException(nameof(wpfHotKeyManager));
            }

            this.smartbarService = smartbarService;
            this.pluginService = pluginService;
            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.commandDispatcher = commandDispatcher;
            this.uiExtensionService = uiExtensionService;
            this.wpfHotKeyManager = wpfHotKeyManager;
            this.smartbarSettings = smartbarSettings;
        }

        public Boolean CanCreateApplicationButton(Application application)
        {
            return application is ProcessApplication;
        }

        public ApplicationButton CreateApplicationButton([NotNull] Application application)
        {
            var processApplication = application as ProcessApplication;
            if (processApplication == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            var processApplicationViewModel = new ProcessApplicationButtonViewModel(processApplication, this.smartbarService,
                this.pluginService, this.windowService, this.eventAggregator, this.smartbarSettings,
                this.commandDispatcher, this.uiExtensionService, this.wpfHotKeyManager);

            return new ProcessApplicationButton(processApplicationViewModel);
        }
    }
}
