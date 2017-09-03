namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource
{
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class SelectNativeResourceViewModelSelectFileCommand : DelegateCommand
    {
        public SelectNativeResourceViewModelSelectFileCommand(SelectNativeResourceViewModel selectNativeResourceViewModel, IWindowService windowService)
            : base(async () =>
            {
                var model = new OpenFileDialogModel
                {
                    File = selectNativeResourceViewModel.File,
                    Filter = Localization.SelectNativeResource.FilesWithResourcesFilterText + "|*.exe;*.dll;*.ico"
                };
                if (windowService.ShowFileDialog(model) == MessageBoxResult.OK)
                {
                    await selectNativeResourceViewModel.LoadImagesAsync(model.File);
                }
            }, () => !selectNativeResourceViewModel.IsRefreshingImages)
        {
        }
    }
}
