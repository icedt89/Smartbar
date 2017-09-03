namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class PniduiWellKnownIconLibrary : WellKnownIconLibrary
    {
        public PniduiWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "pnidui.dll"), nameof(Localization.WellKnownIconContainers.Pnidui))
        {
        }
    }
}