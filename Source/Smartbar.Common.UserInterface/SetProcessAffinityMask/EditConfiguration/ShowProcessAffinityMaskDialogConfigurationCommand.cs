namespace JanHafner.Smartbar.Common.UserInterface.SetProcessAffinityMask.EditConfiguration
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class ShowProcessAffinityMaskDialogConfigurationCommand : DynamicUICommand
    {
        public ShowProcessAffinityMaskDialogConfigurationCommand([NotNull] Func<String> displayTextFunc, [NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher, IPluginConfigurationService pluginConfigurationService, Func<Boolean> canExecute) 
            : base(displayTextFunc, async () =>
            {
                var pluginConfiguration = pluginConfigurationService.GetConfigurationOrDefault<ProcessAffinityMaskDialogConfiguration>();
                var viewModel = new EditPluginConfigurationViewModel(pluginConfiguration, windowService);
                await windowService.ShowWindowAsync<EditPluginConfiguration>(viewModel, async result =>
                {
                    if (result == MessageBoxResult.OK)
                    {
                        pluginConfiguration.ShowOnlyAvailableProcessors = viewModel.ShowOnlyAvailableProcessors;

                        await commandDispatcher.DispatchAsync(new[] {new PersistPluginConfigurationCommand(pluginConfiguration)});
                    }
                });
            }, canExecute)
        {
        }
    }
}