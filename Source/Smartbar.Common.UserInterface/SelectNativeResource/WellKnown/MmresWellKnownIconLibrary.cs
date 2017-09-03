namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class MmresWellKnownIconLibrary : WellKnownIconLibrary
    {
        public MmresWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "mmres.dll"), nameof(Localization.WellKnownIconContainers.Mmres))
        {
        }
    }
}