namespace JanHafner.Smartbar.Views.PluginManager.Loading
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using NuGet;
    using Prism.Events;

    internal sealed class PluginLoader
    {
        [NotNull]
        private readonly IPluginPackageManager pluginPackageManager;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly IProgress<PluginLoaderProgress> pluginPackageResolved;

        public PluginLoader([NotNull] IPluginPackageManager pluginPackageManager, [NotNull] IEventAggregator eventAggregator, [NotNull] IProgress<PluginLoaderProgress> pluginPackageResolved)
        {
            if (pluginPackageManager == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageManager));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (pluginPackageResolved == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageResolved));
            }

            this.pluginPackageManager = pluginPackageManager;
            this.eventAggregator = eventAggregator;
            this.pluginPackageResolved = pluginPackageResolved;
        }

        [NotNull]
        public async Task<PluginLoaderResult> LoadAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var loadedPluginPackageMetadataCount = 0;
            var loadedPluginPackageMetadata = Enumerable.Empty<IPackage>();
            Exception occuredException = null;
            try
            {
                try
                {
                    loadedPluginPackageMetadata = (await this.pluginPackageManager.GetAvailablePluginPackagesAsync(cancellationToken)).ToList();
                }
                catch (Exception ex)
                {
                    occuredException = ex;
                    throw;
                }
            }
            catch (Exception)
            {
                loadedPluginPackageMetadata = (await this.pluginPackageManager.GetInstalledPluginPackagesAsync()).ToList();
            }
      
            foreach (var pluginPackageMetadataById in loadedPluginPackageMetadata.Select(ppm => new
            {
                Package = ppm,
                IsInstalled = Task.Run(() => this.pluginPackageManager.IsInstalledAsync(ppm), cancellationToken).Result
            }).GroupBy(ppm => ppm.Package.Id).OrderBy(g => g.Key))
            {
                var installedVersion = pluginPackageMetadataById.SingleOrDefault(ppm => ppm.IsInstalled)?.Package.Version;
                foreach (var singlePackage in pluginPackageMetadataById.OrderByDescending(ppm => ppm.Package.Version))
                {
                    this.pluginPackageResolved.Report(new PluginLoaderProgress(new PluginPackageViewModel(singlePackage.Package, this.pluginPackageManager, installedVersion, this.eventAggregator), ++loadedPluginPackageMetadataCount == loadedPluginPackageMetadata.Count()));

                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            return new PluginLoaderResult(loadedPluginPackageMetadataCount > 0, occuredException != null);
        }
    }
}
