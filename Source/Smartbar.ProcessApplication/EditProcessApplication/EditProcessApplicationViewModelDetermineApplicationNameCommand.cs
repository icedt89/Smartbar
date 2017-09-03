namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System.IO;
    using JanHafner.Smartbar.Common;
    using JetBrains.Annotations;
    using Prism.Commands;

    internal sealed class EditProcessApplicationViewModelDetermineApplicationNameCommand : DelegateCommand
    {
        public EditProcessApplicationViewModelDetermineApplicationNameCommand(
            [NotNull] EditProcessApplicationViewModel editProcessApplicationViewModel)
            : base(() =>
            {
                if (Directory.Exists(editProcessApplicationViewModel.Execute))
                {
                    editProcessApplicationViewModel.Name = PathUtilities.GetIdealDirectoryDisplayName(editProcessApplicationViewModel.Execute);
                }
                else if (File.Exists(editProcessApplicationViewModel.Execute))
                {
                    editProcessApplicationViewModel.Name = PathUtilities.GetIdealFileDisplayName(editProcessApplicationViewModel.Execute);
                }
            }, () => PathUtilities.PathExists(editProcessApplicationViewModel.Execute))
        {
        }
    }
}
