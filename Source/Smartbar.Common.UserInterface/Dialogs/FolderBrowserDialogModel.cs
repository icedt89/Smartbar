namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;

    public sealed class FolderBrowserDialogModel
    {
        public FolderBrowserDialogModel()
        {
            this.ShowNewFolderButton = true;
            this.RootFolder = Environment.SpecialFolder.MyComputer;
        }

        public String Directory { get; set; }

        public Boolean ShowNewFolderButton { get; set; }

        public String Description { get; set; }

        public Environment.SpecialFolder RootFolder { get; set; }
    }
}
