namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class WmplocWellKnownIconLibrary : WellKnownIconLibrary
    {
        public WmplocWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "wmploc.dll"), nameof(Localization.WellKnownIconContainers.Wmploc))
        {
        }
    }
}