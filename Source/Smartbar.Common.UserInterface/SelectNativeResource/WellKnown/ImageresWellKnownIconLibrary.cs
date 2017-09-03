namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class ImageresWellKnownIconLibrary : WellKnownIconLibrary
    {
        public ImageresWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "imageres.dll"), nameof(Localization.WellKnownIconContainers.Imageres))
        {
        }
    }
}