namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories.EditPluginConfiguration
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class ShowDirectoryDragDropPluginConfigurationCommand : DynamicUICommand
    {
        public ShowDirectoryDragDropPluginConfigurationCommand([NotNull] Func<String> displayTextFactory, [NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IPluginConfigurationService pluginConfigurationService, Func<Boolean> canExecute) 
            : base(displayTextFactory, async () =>
            {
                var pluginConfiguration = pluginConfigurationService.GetConfigurationOrDefault<DirectoryDragDropHandlerPluginConfiguration>();
                var viewModel = new EditPluginConfigurationViewModel(pluginConfiguration, windowService);
                await windowService.ShowWindowAsync<EditPluginConfiguration>(viewModel, async result =>
                {
                    if (result == MessageBoxResult.OK)
                    {
                        pluginConfiguration.ProcessDesktopIni = viewModel.ProcessDesktopIni;

                        await commandDispatcher.DispatchAsync(new[] { new PersistPluginConfigurationCommand(pluginConfiguration) });
                    }
                });
            }, canExecute)
        {
        }
    }
}