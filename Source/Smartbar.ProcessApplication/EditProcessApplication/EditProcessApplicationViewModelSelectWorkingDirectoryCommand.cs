namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class EditProcessApplicationViewModelSelectWorkingDirectoryCommand : DelegateCommand
    {
        public EditProcessApplicationViewModelSelectWorkingDirectoryCommand(EditProcessApplicationViewModel editProcessApplicationViewModel, IWindowService windowService)
            : base(() =>
            {
                var model = new FolderBrowserDialogModel
                {
                    Directory = editProcessApplicationViewModel.WorkingDirectory,
                    Description = Localization.EditProcessApplication.EditProcessApplicationWorkingDirectoryDialogTitle
                };
                if (windowService.ShowFolderBrowserDialog(model) == MessageBoxResult.OK)
                {
                    editProcessApplicationViewModel.WorkingDirectory = model.Directory;
                }
            })
        {
        }
    }
}
