namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;

    public sealed class OpenFileDialogModel : FileDialogModel
    {
        public OpenFileDialogModel()
        {
            this.ShowReadOnly = true;
            this.ReadOnlyChecked = true;
            this.DereferenceLinks = true;
            this.CheckFileExists = true;
            this.CheckPathExists = true;
        }

        public Boolean Multiselect { get; set; }

        public Boolean ReadOnlyChecked { get; set; }

        public Boolean ShowReadOnly { get; set; }
    }
}
