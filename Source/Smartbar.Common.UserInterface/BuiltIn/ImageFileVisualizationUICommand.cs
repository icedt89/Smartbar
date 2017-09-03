namespace JanHafner.Smartbar.Common.UserInterface.BuiltIn
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Common.UserInterface.Localization;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class ImageFileVisualizationUICommand : DynamicUICommand
    {
        public ImageFileVisualizationUICommand([NotNull] Func<String> displayTextFactory, [NotNull] ISmartbarService smartbarService, [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher, Guid applicationId, [NotNull] Func<Boolean> canExecute)
            : base(displayTextFactory, async () =>
            {
                var application = (IApplicationWithImage)smartbarService.GetApplication<Model.Application>(applicationId);
                var currentFileApplicationImage = application.Image as FileApplicationImage;

                var selectFileModel = new OpenFileDialogModel
                {
                    Filter = MenuItems.FrameworkSupportedImageFilesFilterText + "|*.bmp;*gif;*.jpg;*.png;*.tiff"
                };
                if (currentFileApplicationImage != null)
                {
                    selectFileModel.File = currentFileApplicationImage.File;
                }

                if (windowService.ShowFileDialog(selectFileModel) == MessageBoxResult.OK)
                {
                    await commandDispatcher.DispatchAsync(new UpdateApplicationWithImageFileApplicationImageCommand(
                        applicationId, selectFileModel.File));
                }
            }, canExecute)
        {
        }
    }
}