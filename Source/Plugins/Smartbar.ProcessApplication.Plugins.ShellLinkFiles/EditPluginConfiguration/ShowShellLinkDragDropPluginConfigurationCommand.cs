namespace JanHafner.Smartbar.ProcessApplication.Plugins.ShellLinkFiles.EditPluginConfiguration
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class ShowShellLinkDragDropPluginConfigurationCommand : DynamicUICommand
    {
        public ShowShellLinkDragDropPluginConfigurationCommand([NotNull] Func<String> displayTextFactory, [NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher, IPluginConfigurationService pluginConfigurationService, Func<Boolean> canExecute) 
            : base(displayTextFactory, async () =>
            {
                var pluginConfiguration = pluginConfigurationService.GetConfigurationOrDefault<ShellLinkDragDropHandlerPluginConfiguration>();
                var viewModel = new EditPluginConfigurationViewModel(pluginConfiguration, windowService);
                await windowService.ShowWindowAsync<EditPluginConfiguration>(viewModel, async result =>
                {
                    if (result == MessageBoxResult.OK)
                    {
                        pluginConfiguration.TryDeleteSourceShellLink = viewModel.TryDeleteSourceShellLink;

                        await commandDispatcher.DispatchAsync(new[] {new PersistPluginConfigurationCommand(pluginConfiguration)});
                    }
                });
            }, canExecute)
        {
        }
    }
}