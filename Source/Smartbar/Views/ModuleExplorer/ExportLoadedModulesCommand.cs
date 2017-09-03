namespace JanHafner.Smartbar.Views.ModuleExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using Prism.Commands;
    using Services;

    internal sealed class ExportLoadedModulesCommand : DelegateCommand
    {
        public ExportLoadedModulesCommand(IEnumerable<ModuleViewModel> modules, IWindowService windowService)
             : base(() =>
             {
                 var fileContent = new StringBuilder();
                 fileContent.AppendLine($"Export of loaded modules from {DateTime.Now}");
                 fileContent.AppendLine("------------------------");

                 foreach (var loadedModule in modules)
                 {
                     fileContent.AppendLine($@"{loadedModule.Name} ===> {loadedModule.File}");
                 }

                 var saveFileDialogModel = new SaveFileDialogModel
                 {
                     Title = Localization.ModuleExplorer.ExportLoadedModulesDialogTitle,
                     DefaultExt = "txt",
                     File = $"Smartbar module export {DateTime.Now.ToUnixTimestamp()}"
                 };
                 if (windowService.ShowFileDialog(saveFileDialogModel) == MessageBoxResult.OK)
                 {
                     System.IO.File.WriteAllText(saveFileDialogModel.File, fileContent.ToString());
                 }
             })
        {
        }
    }
}
