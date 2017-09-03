namespace Smartbar.Updater.Core
{
    using System;
    using JetBrains.Annotations;
    using NuGet;

    public sealed class RemoteVersion
    {
        public RemoteVersion([NotNull] SemanticVersion version, [NotNull] IPackage updatePackage)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (updatePackage == null)
            {
                throw new ArgumentNullException(nameof(updatePackage));
            }

            this.Version = version;
            this.UpdatePackage = updatePackage;
        }

        [NotNull]
        public SemanticVersion Version { get; private set; }

        [NotNull]
        public IPackage UpdatePackage { get; private set; }
    }
}