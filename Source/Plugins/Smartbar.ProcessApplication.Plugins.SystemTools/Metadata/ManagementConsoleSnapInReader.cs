namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using System.Xml;
    using JetBrains.Annotations;

    internal sealed class ManagementConsoleSnapInReader : IDisposable
    {
        [NotNull]
        private readonly XmlReader mmcReader;

        public ManagementConsoleSnapInReader([NotNull] String managementConsoleSnapInFile)
        {
            if (String.IsNullOrWhiteSpace(managementConsoleSnapInFile))
            {
                throw new ArgumentNullException(nameof(managementConsoleSnapInFile));
            }

            this.mmcReader = XmlReader.Create(managementConsoleSnapInFile, new XmlReaderSettings
            {
                ValidationType = ValidationType.None
            });
        }

        public ManagementConsoleMetadata GetManagementConsoleMetadata()
        {
            var result = new ManagementConsoleMetadata();
            while (this.mmcReader.Read())
            {
                if (this.mmcReader.Name != "Icon" || !this.mmcReader.HasAttributes)
                {
                    continue;
                }

                result.IconFile = this.mmcReader.GetAttribute("File");
                if (!String.IsNullOrWhiteSpace(result.IconFile))
                {
                    result.IconFile = Environment.ExpandEnvironmentVariables(result.IconFile);
                }

				if (Int32.TryParse(this.mmcReader.GetAttribute("Index"), out int iconIndex))
				{
					result.IconIndex = iconIndex;
				}

				break;
            }

            return result;
        }

        public void Dispose()
        {
            this.mmcReader.Dispose();
        }
    }
}
