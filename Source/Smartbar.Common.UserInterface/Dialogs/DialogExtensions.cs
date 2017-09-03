namespace JanHafner.Smartbar.Common.UserInterface.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
    using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

    public static class DialogExtensions
    {
        private static MessageBoxResult ShowFileDialog([CanBeNull] this IWindowService windowService, [NotNull] OpenFileDialogModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var openFileDialog = new OpenFileDialog
            {
                Multiselect = model.Multiselect,
                ReadOnlyChecked = model.ReadOnlyChecked,
                ShowReadOnly = model.ShowReadOnly,
                AddExtension = model.AddExtension,
                CheckFileExists = model.CheckFileExists,
                CheckPathExists = model.CheckPathExists,
                DefaultExt = model.DefaultExt,
                DereferenceLinks = model.DereferenceLinks,
                InitialDirectory = model.InitialDirectory,
                Title = model.Title,
                FileName = model.File,
                Filter = model.Filter,
                FilterIndex = model.FilterIndex,
                RestoreDirectory = model.RestoreDirectory
            };

            var result = openFileDialog.ShowDialog();

            model.File = openFileDialog.FileName;

            return result.ToMessageBoxResult();
        }

        private static MessageBoxResult ShowFileDialog([CanBeNull] this IWindowService windowService, [NotNull] SaveFileDialogModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = model.AddExtension,
                CheckFileExists = model.CheckFileExists,
                CheckPathExists = model.CheckPathExists,
                DefaultExt = model.DefaultExt,
                DereferenceLinks = model.DereferenceLinks,
                InitialDirectory = model.InitialDirectory,
                Title = model.Title,
                FileName = model.File,
                Filter = model.Filter,
                FilterIndex = model.FilterIndex,
                RestoreDirectory = model.RestoreDirectory,
                CreatePrompt = model.CreatePrompt,
                OverwritePrompt = model.OverwritePrompt
            };

            var result = saveFileDialog.ShowDialog();

            model.File = saveFileDialog.FileName;

            return result.ToMessageBoxResult();
        }

        public static MessageBoxResult ShowFileDialog([CanBeNull] this IWindowService windowService, [NotNull] FileDialogModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var openFileDialogModel = model as OpenFileDialogModel;
            if (openFileDialogModel != null)
            {
                return windowService.ShowFileDialog(openFileDialogModel);
            }

            var saveFileDialogModel = model as SaveFileDialogModel;
            if (saveFileDialogModel != null)
            {
                return windowService.ShowFileDialog(saveFileDialogModel);
            }

            throw new ArgumentException(model.GetType().Name);
        }

        public static MessageBoxResult ShowFolderBrowserDialog([CanBeNull] this IWindowService windowService, [NotNull] FolderBrowserDialogModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.ShowNewFolderButton = model.ShowNewFolderButton;
                folderBrowserDialog.Description = model.Description;
                folderBrowserDialog.SelectedPath = model.Directory;
                folderBrowserDialog.RootFolder = model.RootFolder;

                var result = folderBrowserDialog.ShowDialog();

                model.Directory = folderBrowserDialog.SelectedPath;

                return result.ToMessageBoxResult();
            }
        }

        public static async Task<MessageBoxResult> ShowSimpleModalInfoDialog([NotNull] this IWindowService windowService, String text)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            return await windowService.ShowSimpleModalInfoDialog(text, "Information");
        }

        public static Task<MessageBoxResult> ShowSimpleModalInfoDialog([NotNull] this IWindowService windowService, String text, String caption)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            return windowService.ShowWindowAsync<SimpleDialog>(new SimpleDialogViewModel(caption, text, windowService, MessageBoxButton.OK, MessageBoxImage.Information));
        }

        public static Task<MessageBoxResult> ShowSimpleModalDialog([NotNull] this IWindowService windowService, String text,
            String caption, MessageBoxButton buttons, MessageBoxImage image)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            return windowService.ShowWindowAsync<SimpleDialog>(new SimpleDialogViewModel(caption, text, windowService, buttons, image));
        }

        public static Task<MessageBoxResult> ShowSimpleQuestionDialog([NotNull] this IWindowService windowService, String text, String caption, MessageBoxButton buttons)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            return windowService.ShowWindowAsync<SimpleDialog>(new SimpleDialogViewModel(caption, text, windowService, buttons, MessageBoxImage.Question));
        }

        public static async Task<MessageBoxResult> ShowSimpleModalErrorDialog([NotNull] this IWindowService windowService, String text)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            return await windowService.ShowSimpleModalErrorDialog(text, "Error");
        }

        public static Task<MessageBoxResult> ShowSimpleModalErrorDialog([NotNull] this IWindowService windowService, String text, String caption)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            return windowService.ShowWindowAsync<SimpleDialog>(new SimpleDialogViewModel(caption, text, windowService, MessageBoxButton.OK, MessageBoxImage.Error));
        }

        internal static MessageBoxResult ToMessageBoxResult(this DialogResult dialogResult)
        {
            switch (dialogResult)
            {
                default:
                case DialogResult.Cancel:
                    return MessageBoxResult.Cancel;
                case DialogResult.Abort:
                case DialogResult.Ignore:
                case DialogResult.Retry:
                    return MessageBoxResult.None;
                case DialogResult.None:
                    return MessageBoxResult.None;
                case DialogResult.No:
                    return MessageBoxResult.No;
                case DialogResult.OK:
                    return MessageBoxResult.OK;
                case DialogResult.Yes:
                    return MessageBoxResult.Yes;
            }
        }

        public static MessageBoxResult ToMessageBoxResult([CanBeNull] this Boolean? dialogResult)
        {
            switch (dialogResult)
            {
                case null:
                default:
                    return MessageBoxResult.None;
                case false:
                    return MessageBoxResult.Cancel;
                case true:
                    return MessageBoxResult.OK;
            }
        }
    }
}
