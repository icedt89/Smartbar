namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class IeframeWellKnownIconLibrary : WellKnownIconLibrary
    {
        public IeframeWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "ieframe.dll"), nameof(Localization.WellKnownIconContainers.Ieframe))
        {
        }
    }
}