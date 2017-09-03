namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class SetupapiWellKnownIconLibrary : WellKnownIconLibrary
    {
        public SetupapiWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "setupapi.dll"), nameof(Localization.WellKnownIconContainers.Setupapi))
        {
        }
    }
}