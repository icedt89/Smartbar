namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class NetcenterWellKnownIconLibrary : WellKnownIconLibrary
    {
        public NetcenterWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "netcenter.dll"), nameof(Localization.WellKnownIconContainers.Netcenter))
        {
        }
    }
}