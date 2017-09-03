namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class MstscaxWellKnownIconLibrary : WellKnownIconLibrary
    {
        public MstscaxWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "mstscax.dll"), nameof(Localization.WellKnownIconContainers.Mstscax))
        {
        }
    }
}