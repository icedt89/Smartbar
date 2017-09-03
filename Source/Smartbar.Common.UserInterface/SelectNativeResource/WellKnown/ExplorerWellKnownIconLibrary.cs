namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class ExplorerWellKnownIconLibrary : WellKnownIconLibrary
    {
        public ExplorerWellKnownIconLibrary()
            : base(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"), nameof(Localization.WellKnownIconContainers.Explorer))
        {
        }
    }
}