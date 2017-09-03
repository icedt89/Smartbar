namespace JanHafner.Smartbar.Common.UserInterface.BuiltIn
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;

    internal sealed  class IconImageVisualizationUICommand : DynamicUICommand
    {
        public IconImageVisualizationUICommand([NotNull] Func<String> displayTextFactory, [NotNull] ISmartbarService smartbarService, [NotNull] IEventAggregator eventAggregator, [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider, Guid applicationId, [NotNull] Func<Boolean> canExecute)
            : base(displayTextFactory, async () =>
            {
                var application = (IApplicationWithImage)smartbarService.GetApplication<Model.Application>(applicationId);
                var currentIconApplicationImage = application.Image as IconApplicationImage;

                var selectNativeResourceViewModel = new SelectNativeResourceViewModel(windowService, eventAggregator, wellKnownIconLibraryUiCommandProvider);
                if (currentIconApplicationImage != null)
                {
                    await selectNativeResourceViewModel.LoadImagesAsync(currentIconApplicationImage.File, currentIconApplicationImage.Identifier, currentIconApplicationImage.IdentifierType);
                }

                if (await windowService.ShowWindowAsync<SelectNativeResource>(selectNativeResourceViewModel) ==
                    MessageBoxResult.OK)
                {
                    await commandDispatcher.DispatchAsync(new UpdateApplicationWithImageIconApplicationImageCommand(
                        applicationId, selectNativeResourceViewModel.File,
                        selectNativeResourceViewModel.Icon.Identifier,
                        selectNativeResourceViewModel.Icon.IconIdentifierType));
                }
            }, canExecute)
        {
        }
    }
}