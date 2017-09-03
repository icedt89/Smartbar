namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;
    using JanHafner.Toolkit.Windows;

    [Export(typeof(ISystemToolMetadata))]
    internal sealed class GodModeSystemToolMetadata : ISystemToolMetadata
    {
        public GodModeSystemToolMetadata()
        {
            this.File = "shell:::{ED7BA470-8E54-465E-825C-99712043E01C}";
            this.IconFile = Path.Combine(Environment.SystemDirectory, "shell32.dll");
        }

        public String DisplayTextResourceName
        {
            get { return nameof(DisplayTexts.GodMode); }
        }

        public String File { get; }

        public String IconFile { get; }

        public Int32 IconIdentifier
        {
            get { return 22; }
        }

        public IconIdentifierType IconIdentifierType
        {
            get { return IconIdentifierType.ResourceId; }
        }

        public Boolean IsAvailable
        {
            get { return true; }
        }

        public Boolean ShouldRunPostConfigureForSystemTool
        {
            get
            {
                return false; 
            }
        }

        public void PostConfigureSystemTool()
        {
        }
    }
}
