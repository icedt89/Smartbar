namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class DdoresWellKnownIconLibrary : WellKnownIconLibrary
    {
        public DdoresWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "ddores.dll"), nameof(Localization.WellKnownIconContainers.Ddores))
        {
        }
    }
}