namespace JanHafner.Smartbar.Common.UserInterface.BuiltIn
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;

    internal sealed  class IconPackVisualizationUICommand : DynamicUICommand
    {
        public IconPackVisualizationUICommand([NotNull] Func<String> displayTextFactory, [NotNull] ISmartbarService smartbarService, [NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ICommandDispatcher commandDispatcher, [NotNull] ISelectableIconPacksProvider selectableIconPacksProvider, Guid applicationId, [NotNull] Func<Boolean> canExecute)
            : base(displayTextFactory, async () =>
            {
                var applicationWithImage = (IApplicationWithImage)smartbarService.GetApplication<Model.Application>(applicationId);
                var currentIconPackApplicationImage = applicationWithImage.Image as IconPackApplicationImage;

                var selectIconPackResourceViewModel = new SelectIconPackResourceViewModel(windowService, eventAggregator, selectableIconPacksProvider);
                if (currentIconPackApplicationImage != null)
                {
                    selectIconPackResourceViewModel.FillColor = currentIconPackApplicationImage.FillColor.FromXaml<Color>();

                    await selectIconPackResourceViewModel.LoadImagesAsync(currentIconPackApplicationImage.IconPackType, currentIconPackApplicationImage.IconPackKindKey);
                }
                else
                {
                    await selectIconPackResourceViewModel.LoadImagesAsync();
                }

                if (await windowService.ShowWindowAsync<SelectIconPackResource>(selectIconPackResourceViewModel) == MessageBoxResult.OK)
                {
                    var selectedIconPackKind = selectIconPackResourceViewModel.Resource.IconPackKindKey;
                    var fillColorMemoryStream = selectIconPackResourceViewModel.FillColor.ToXaml();
                    var selectedIconPackType = selectIconPackResourceViewModel.IconPack;

                    await commandDispatcher.DispatchAsync(new UpdateApplicationWithImageIconPackApplicationImageCommand(
                       applicationId, selectedIconPackType.IconPackType, selectedIconPackKind, fillColorMemoryStream));
                }
            }, canExecute)
        {
        }
    }
}