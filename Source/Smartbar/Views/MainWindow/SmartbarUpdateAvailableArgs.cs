namespace JanHafner.Smartbar.Views.MainWindow
{
    using JetBrains.Annotations;
    using System;
    using NuGet;

    internal sealed class SmartbarUpdateAvailableArgs
    {
        public SmartbarUpdateAvailableArgs([NotNull] IPackage updatePackage)
        {
            if (updatePackage == null)
            {
                throw new ArgumentNullException(nameof(updatePackage));
            }

            this.UpdatePackage = updatePackage;
        }

        public IPackage UpdatePackage { get; private set; }
    }
}
