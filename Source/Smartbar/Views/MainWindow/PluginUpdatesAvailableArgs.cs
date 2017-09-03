namespace JanHafner.Smartbar.Views.MainWindow
{
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using NuGet;

    internal sealed class PluginUpdatesAvailableArgs
    {
        public PluginUpdatesAvailableArgs([NotNull] IEnumerable<IPackage> updatablePackages)
        {
            if (updatablePackages == null)
            {
                throw new ArgumentNullException(nameof(updatablePackages));
            }

            this.UpdatablePackages = updatablePackages;
        }

        public IEnumerable<IPackage> UpdatablePackages { get; private set; }
    }
}
