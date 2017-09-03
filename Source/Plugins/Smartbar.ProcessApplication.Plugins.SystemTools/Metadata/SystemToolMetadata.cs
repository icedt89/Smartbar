namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    internal abstract class SystemToolMetadata : ISystemToolMetadata
    {
        protected SystemToolMetadata([NotNull] String file,
            [NotNull] String displayTextResourceName)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (String.IsNullOrEmpty(displayTextResourceName))
            {
                throw new ArgumentNullException(nameof(displayTextResourceName));
            }

            // resolveDisplayText-func is needed because derivations of SystemToolMetadata are typically created as singletons, 
            // but the application language can change at runtime.
            this.File = file;
            this.IsAvailable = System.IO.File.Exists(file);
            this.IconFile = this.File;
            this.IconIdentifierType = IconIdentifierType.Index;
            this.DisplayTextResourceName = displayTextResourceName;
        }

        public String DisplayTextResourceName { get; private set; }

        public String File { get; private set;  }

        public String IconFile { get; protected set;  }

        public Int32 IconIdentifier { get; protected set; }

        public IconIdentifierType IconIdentifierType { get; protected set;  }

        public Boolean IsAvailable { get; private set;  }

        public virtual Boolean ShouldRunPostConfigureForSystemTool
        {
            get
            {
                return true;
            }
        }

        public virtual void PostConfigureSystemTool()
        {
            using (var managementConsoleSnapInReader = new ManagementConsoleSnapInReader(this.File))
            {
                var metadata = managementConsoleSnapInReader.GetManagementConsoleMetadata();
                this.IconFile = metadata.IconFile;
                this.IconIdentifier = metadata.IconIndex;
            }

            this.IconIdentifierType = IconIdentifierType.Index;
        }
    }
}
