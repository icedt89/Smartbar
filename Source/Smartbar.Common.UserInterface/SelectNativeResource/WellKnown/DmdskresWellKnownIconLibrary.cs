namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class DmdskresWellKnownIconLibrary : WellKnownIconLibrary
    {
        public DmdskresWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "dmdskres.dll"), nameof(Localization.WellKnownIconContainers.Dmdskres))
        {
        }
    }
}