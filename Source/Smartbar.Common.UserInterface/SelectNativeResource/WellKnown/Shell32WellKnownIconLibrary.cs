namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class Shell32WellKnownIconLibrary : WellKnownIconLibrary
    {
        public Shell32WellKnownIconLibrary() 
            : base(Path.Combine(Environment.SystemDirectory, "shell32.dll"), nameof(Localization.WellKnownIconContainers.Shell32))
        {
        }
    }
}