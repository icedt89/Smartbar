namespace JanHafner.Smartbar.Views.MainWindow
{
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.PluginManager;
    using Prism.Commands;
    using Prism.Events;

    internal sealed class MainWindowViewModelOpenPluginManagerCommand : DelegateCommand
    {
        public MainWindowViewModelOpenPluginManagerCommand(IPluginPackageManager pluginPackageManager, IEventAggregator eventAggregator, ISmartbarSettings smartbarSettings, IModuleExplorer moduleExplorer, IWindowService windowService)
            : base(async () =>
            {
                var pluginManagerViewModel = new PluginManagerViewModel(windowService, pluginPackageManager, eventAggregator, smartbarSettings, moduleExplorer);
#pragma warning disable 4014
                pluginManagerViewModel.LoadPluginPackagesAsync();
#pragma warning restore 4014

                await windowService.ShowWindowAsync<PluginManager>(pluginManagerViewModel, result => {});
            })
        {
        }
    }
}
