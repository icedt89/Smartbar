namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class AccessibilitycplWellKnownIconLibrary : WellKnownIconLibrary
    {
        public AccessibilitycplWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "Accessibilitycpl.dll"), nameof(Localization.WellKnownIconContainers.Accessibilitycpl))
        {
        }
    }
}