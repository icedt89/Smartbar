namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using JanHafner.Smartbar.Infrastructure.Commanding.Plugins;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using NuGet;
    using Prism.Events;

    [Export(typeof(IPluginPackageManager))]
    internal sealed class PluginPackageManager : IPluginPackageManager
    {
        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly IPackageManager packageManager;

        [NotNull]
        private readonly Expression<Func<IPackage, Boolean>> pluginPackageFilter = p => p.Tags.Contains("smartbar") && p.Tags.Contains("plugin");

        [ImportingConstructor]
        public PluginPackageManager([NotNull] ISmartbarSettings smartbarSettings, [NotNull] IEventAggregator eventAggregator)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.eventAggregator = eventAggregator;
            var currentSmartbarSettings = smartbarSettings;

            var pluginPackagesDirectory = smartbarSettings.CreateAndGetPackagesDirectory();

            var pluginPackageRepository = PackageRepositoryFactory.Default.CreateRepository(currentSmartbarSettings.PluginPackagesFeed);
            this.packageManager = new PackageManager(pluginPackageRepository, pluginPackagesDirectory);

            this.packageManager.PackageInstalled += async (sender, args) =>
            {
                if (currentSmartbarSettings.PendingPackageDeleteOperations.Contains(args.InstallPath))
                {
                    currentSmartbarSettings.RemovePendingPackageDeleteOperation(args.InstallPath);

                    await currentSmartbarSettings.SaveChangesAsync();
                }
                
                this.eventAggregator.GetEvent<PluginInstalled>().Publish(args.Package);
            };
            this.packageManager.PackageUninstalled += async (sender, args) =>
            {
                if (args.FileSystem.DirectoryExists(args.InstallPath) && !currentSmartbarSettings.PendingPackageDeleteOperations.Contains(args.InstallPath))
                {
                    currentSmartbarSettings.AddPendingPackageDeleteOperation(args.InstallPath);

                    await currentSmartbarSettings.SaveChangesAsync();
                }

                this.eventAggregator.GetEvent<PluginUninstalled>().Publish(args.Package);
            };
        }

        public async Task<Boolean> IsInstalledAsync(IPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }
            return await Task.Run(() => this.packageManager.LocalRepository.Exists(package));
        }

        public async Task<Boolean> IsAvailableAsync(IPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            return await Task.Run(() => this.packageManager.SourceRepository.Exists(package));
        }

        public async Task<IEnumerable<IPackage>> GetAvailablePluginPackagesAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => this.packageManager.SourceRepository.GetPackages().Where(this.pluginPackageFilter).ToList(), cancellationToken);
        }

        public async Task<IEnumerable<IPackage>> GetInstalledPluginPackagesAsync()
        {
            return await Task.Run(() => this.packageManager.LocalRepository.GetPackages().Where(this.pluginPackageFilter).ToList());
        }

        public async Task<IEnumerable<IPackage>> GetAvailablePluginUpdatesAsync(IEnumerable<IPackage> packages)
        {
            return await Task.Run(() => this.packageManager.SourceRepository.GetUpdates(packages.Select(p => new PackageName(p.Id, p.Version)), false, false));
        }

        public async Task InstallPluginAsync(IPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            await Task.Run(() =>
            {
                this.packageManager.InstallPackage(package, true, false);
            });
        }

        public async Task UpdatePluginAsync(IPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            await Task.Run(() =>
            {
                this.packageManager.UpdatePackage(package, false, false);
            });
        }

        public async Task UninstallPluginAsync(IPackage package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            await Task.Run(() =>
            {
                this.packageManager.UninstallPackage(package, true, true);
            });
        }
    }
}
