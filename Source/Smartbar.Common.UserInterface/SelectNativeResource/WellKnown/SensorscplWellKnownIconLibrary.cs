namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(IWellKnownIconLibrary))]
    internal sealed class SensorscplWellKnownIconLibrary : WellKnownIconLibrary
    {
        public SensorscplWellKnownIconLibrary()
            : base(Path.Combine(Environment.SystemDirectory, "sensorscpl.dll"), nameof(Localization.WellKnownIconContainers.Sensorscpl))
        {
        }
    }
}