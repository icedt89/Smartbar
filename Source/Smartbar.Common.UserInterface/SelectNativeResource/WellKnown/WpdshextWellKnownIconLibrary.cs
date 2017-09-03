namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class WpdshextWellKnownIconLibrary : WellKnownIconLibrary
    {
        public WpdshextWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "wpdshext.dll"), nameof(Localization.WellKnownIconContainers.Wpdshext))
        {
        }
    }
}