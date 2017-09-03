namespace JanHafner.Smartbar.Views.EditSmartbarSettings
{
    using global::Smartbar.Updater.Core;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.About;
    using Prism.Commands;

    internal sealed class EditSmartbarSettingsOpenAboutDialogCommand : DelegateCommand
    {
        public EditSmartbarSettingsOpenAboutDialogCommand(ISmartbarUpdater smartbarUpdater, IWindowService windowService, ISmartbarSettings smartbarSettings)
            : base(async () =>
            {
                var aboutViewModel = new AboutViewModel(windowService, smartbarUpdater, smartbarSettings);
#pragma warning disable 4014
                aboutViewModel.CheckForUpdateAsync();
#pragma warning restore 4014

                await windowService.ShowWindowAsync<About>(aboutViewModel);
            })
        {
        }
    }
}
