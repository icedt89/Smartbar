namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using System.IO;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class EditProcessApplicationViewModelSelectDirectoryCommand : DelegateCommand
    {
        public EditProcessApplicationViewModelSelectDirectoryCommand(EditProcessApplicationViewModel editProcessApplicationViewModel, IWindowService windowService)
            : base(() =>
            {
                var folderBrowserDialogModel = new FolderBrowserDialogModel
                {
                    Description = Localization.EditProcessApplication.EditProcessApplicationSelectDirectoryDialogTitle,
                    Directory = Directory.Exists(editProcessApplicationViewModel.Execute) ? editProcessApplicationViewModel.Execute : null
                };

                if (windowService.ShowFolderBrowserDialog(folderBrowserDialogModel) == MessageBoxResult.OK)
                {
                    editProcessApplicationViewModel.Execute = PathUtilities.GetExplorer();
                    editProcessApplicationViewModel.Arguments = folderBrowserDialogModel.Directory;

                    if (String.IsNullOrWhiteSpace(editProcessApplicationViewModel.Name))
                    {
                        editProcessApplicationViewModel.Name = PathUtilities.GetIdealDirectoryDisplayName(editProcessApplicationViewModel.Arguments);
                    }
                }
            })
        {
        }
    }
}
