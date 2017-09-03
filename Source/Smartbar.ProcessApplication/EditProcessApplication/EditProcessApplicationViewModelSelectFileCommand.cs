namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using System.IO;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class EditProcessApplicationViewModelSelectFileCommand : DelegateCommand
    {
        public EditProcessApplicationViewModelSelectFileCommand(EditProcessApplicationViewModel editProcessApplicationViewModel, IWindowService windowService)
            : base(() =>
            {
                var openFileDialogModel = new OpenFileDialogModel
                {
                    Title = Localization.EditProcessApplication.EditProcessApplicationSelectFilePathDialogTitle,
                    File = File.Exists(editProcessApplicationViewModel.Execute) ? editProcessApplicationViewModel.Execute : null
                };

                if (windowService.ShowFileDialog(openFileDialogModel) == MessageBoxResult.OK)
                {
                    editProcessApplicationViewModel.Execute = openFileDialogModel.File;

                    if (String.IsNullOrWhiteSpace(editProcessApplicationViewModel.Name))
                    {
                        editProcessApplicationViewModel.Name = PathUtilities.GetIdealFileDisplayName(openFileDialogModel.File);
                    }
                }
            })
        {
        }
    }
}
