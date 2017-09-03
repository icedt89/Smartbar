namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;

    public sealed class SaveFileDialogModel : FileDialogModel
    {
        public SaveFileDialogModel()
        {
            this.CheckPathExists = true;
        }

        public Boolean CreatePrompt { get; set; }

        public Boolean OverwritePrompt { get; set; }
    }
}
