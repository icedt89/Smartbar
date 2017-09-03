namespace JanHafner.Smartbar.ProcessApplication.Plugins.ShellLinkFiles.EditPluginConfiguration
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    [Export(typeof(IShowPluginConfigurationUICommandProvider))]
    internal sealed class ShowShellLinkDragDropPluginConfigurationUICommandProvider : IShowPluginConfigurationUICommandProvider
    {
        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly IPluginConfigurationService pluginConfigurationService;

        [ImportingConstructor]
        public ShowShellLinkDragDropPluginConfigurationUICommandProvider([NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IPluginConfigurationService pluginConfigurationService
            )
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (pluginConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(pluginConfigurationService));
            }

            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.pluginConfigurationService = pluginConfigurationService;
        }

        public IDynamicUICommand CreateUICommand(Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return new ShowShellLinkDragDropPluginConfigurationCommand(() => ShellLinkDragDropHandler.PluginName, this.windowService, this.commandDispatcher, this.pluginConfigurationService, canExecute);
        }
    }
}
