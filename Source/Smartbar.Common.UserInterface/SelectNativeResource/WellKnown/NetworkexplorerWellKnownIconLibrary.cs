namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class NetworkexplorerWellKnownIconLibrary : WellKnownIconLibrary
    {
        public NetworkexplorerWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "networkexplorer.dll"), nameof(Localization.WellKnownIconContainers.Networkexplorer))
        {
        }
    }
}