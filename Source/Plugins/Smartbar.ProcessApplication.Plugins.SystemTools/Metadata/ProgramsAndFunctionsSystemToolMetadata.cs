namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class ProgramsAndFunctionsSystemToolMetadata : SystemToolMetadata
    {
        public ProgramsAndFunctionsSystemToolMetadata()
            : base(Path.Combine(Environment.SystemDirectory, "appwiz.cpl"), nameof(DisplayTexts.ProgramsAndFunctions))
        {
        }

        public override Boolean ShouldRunPostConfigureForSystemTool
        {
            get { return false; }
        }
    }
}
