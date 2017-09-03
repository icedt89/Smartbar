namespace JanHafner.Smartbar.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using NuGet;

    public interface IPluginPackageManager
    {
        Task<Boolean> IsInstalledAsync([NotNull] IPackage package);

        Task<Boolean> IsAvailableAsync([NotNull] IPackage package);

        [NotNull]
        Task<IEnumerable<IPackage>> GetAvailablePluginPackagesAsync(CancellationToken cancellationToken);

        [NotNull]
        Task<IEnumerable<IPackage>> GetAvailablePluginUpdatesAsync(IEnumerable<IPackage> packages);

        [NotNull]
        Task<IEnumerable<IPackage>> GetInstalledPluginPackagesAsync();

        Task InstallPluginAsync([NotNull] IPackage package);

        Task UninstallPluginAsync([NotNull] IPackage package);

        Task UpdatePluginAsync(IPackage package);
    }
}