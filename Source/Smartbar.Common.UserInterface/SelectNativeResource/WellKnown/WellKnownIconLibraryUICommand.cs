namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;

    internal sealed class WellKnownIconLibraryUICommand : DynamicUICommand
    {
        public WellKnownIconLibraryUICommand([NotNull] Func<String> displayTextFactory, [NotNull] SelectNativeResourceViewModel selectNativeResourceViewModel,
            [NotNull] IWellKnownIconLibrary wellKnownIconLibrary)
            : base(displayTextFactory, async () =>
            {
                await selectNativeResourceViewModel.LoadImagesAsync(wellKnownIconLibrary.File);
            }, () => wellKnownIconLibrary.IsAvailable)
        {
        }
    }
}
