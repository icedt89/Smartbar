namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class GameuxWellKnownIconLibrary : WellKnownIconLibrary
    {
        public GameuxWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "gameux.dll"), nameof(Localization.WellKnownIconContainers.Gameux))
        {
        }
    }
}