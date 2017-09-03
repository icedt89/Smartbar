namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class DsuiextWellKnownIconLibrary : WellKnownIconLibrary
    {
        public DsuiextWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "dsuiext.dll"), nameof(Localization.WellKnownIconContainers.Dsuiext))
        {
        }
    }
}