namespace Smartbar.Updater.Core
{
    using System;
    using JetBrains.Annotations;
    using NuGet;

    public sealed class LocalVersion
    {
        public LocalVersion([NotNull] String assumedSmartbarExeFile, [NotNull] SemanticVersion version,
            [NotNull] String assumedSmartbarDirectory)
        {
            if (String.IsNullOrWhiteSpace(assumedSmartbarExeFile))
            {
                throw new ArgumentNullException(nameof(assumedSmartbarExeFile));
            }

            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (String.IsNullOrWhiteSpace(assumedSmartbarDirectory))
            {
                throw new ArgumentNullException(nameof(assumedSmartbarDirectory));
            }

            this.AssumedSmartbarExeFile = assumedSmartbarExeFile;
            this.Version = version;
            this.AssumedSmartbarDirectory = assumedSmartbarDirectory;
        }

        [NotNull]
        public String AssumedSmartbarExeFile { get; private set; }

        [NotNull]
        public SemanticVersion Version { get; private set; }

        [NotNull]
        public String AssumedSmartbarDirectory { get; private set; }
    }
}