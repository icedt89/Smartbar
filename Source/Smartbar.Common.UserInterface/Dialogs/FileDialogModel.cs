namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;

    public abstract class FileDialogModel
    {
        public Boolean AddExtension { get; set; }

        public Boolean CheckFileExists { get; set; }

        public Boolean CheckPathExists { get; set; }

        public String DefaultExt { get; set; }

        public Boolean DereferenceLinks { get; set; }

        public String File { get; set; }

        public String Filter { get; set; }

        public Int32 FilterIndex { get; set; }

        public String InitialDirectory { get; set; }

        public Boolean RestoreDirectory { get; set; }

        public String Title { get; set; }
    }
}
