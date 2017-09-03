namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class NetshellWellKnownIconLibrary : WellKnownIconLibrary
    {
        public NetshellWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "netshell.dll"), nameof(Localization.WellKnownIconContainers.Netshell))
        {
        }
    }
}