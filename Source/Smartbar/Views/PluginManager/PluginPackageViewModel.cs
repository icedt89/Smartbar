namespace JanHafner.Smartbar.Views.PluginManager
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Infrastructure.Commanding.Plugins;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using NuGet;
    using Prism.Events;
    using Prism.Mvvm;

    internal sealed class PluginPackageViewModel : BindableBase, IDisposable
    {
        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull]
        private readonly IPluginPackageManager pluginPackageManager;

        [CanBeNull]
        private SemanticVersion alreadyInstalledPackageVersion;

        private Boolean isAvailable = true;

        public PluginPackageViewModel([NotNull] IPackage package, [NotNull] IPluginPackageManager pluginPackageManager, SemanticVersion alreadyInstalledPackageVersion, IEventAggregator eventAggregator)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            if (pluginPackageManager == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageManager));
            }

            this.Package = package;
            this.pluginPackageManager = pluginPackageManager;
            this.alreadyInstalledPackageVersion = alreadyInstalledPackageVersion;

            this.subscriptionTokens = new List<SubscriptionToken>
            {
                eventAggregator.GetEvent<PluginInstalled>().Subscribe(installedPackage =>
                {
                    if (this.Package.Id == installedPackage.Id)
                    {
                        this.alreadyInstalledPackageVersion = installedPackage.Version;
                    }
                    
                    this.OnPropertyChanged(() => this.CanBeUninstalled);
                    this.OnPropertyChanged(() => this.CanBeDowngraded);
                    this.OnPropertyChanged(() => this.CanBeUpgraded);
                    this.OnPropertyChanged(() => this.CanBeInstalled);
                }, ThreadOption.BackgroundThread, true),
                eventAggregator.GetEvent<PluginUninstalled>().Subscribe(async uninstalledPackage =>
                {
                    if (this.Package.Id == uninstalledPackage.Id)
                    {
                        this.alreadyInstalledPackageVersion = null;
                    }

                    this.isAvailable = await this.pluginPackageManager.IsAvailableAsync(this.Package);

                    this.OnPropertyChanged(() => this.CanBeUninstalled);
                    this.OnPropertyChanged(() => this.CanBeDowngraded);
                    this.OnPropertyChanged(() => this.CanBeUpgraded);
                    this.OnPropertyChanged(() => this.CanBeInstalled);
                }, ThreadOption.BackgroundThread, true)
            };
        }

        [NotNull]
        public IPackage Package { get; private set; }

        public Boolean CanBeUninstalled
        {
            get
            {
                return this.alreadyInstalledPackageVersion != null && this.alreadyInstalledPackageVersion == this.Package.Version;
            }
        }

        public Boolean CanBeInstalled
        {
            get
            {
                return this.alreadyInstalledPackageVersion == null && this.isAvailable;
            }
        }

        public Boolean CanBeDowngraded
        {
            get
            {
                return this.isAvailable && this.alreadyInstalledPackageVersion != null && this.alreadyInstalledPackageVersion > this.Package.Version;
            }
        }

        public Boolean CanBeUpgraded
        {
            get
            {
                return this.isAvailable && this.alreadyInstalledPackageVersion != null && this.alreadyInstalledPackageVersion < this.Package.Version;
            }
        }

        [NotNull]
        public async Task InstallAsync()
        {
            if (this.CanBeInstalled)
            {
                await this.pluginPackageManager.InstallPluginAsync(this.Package);
            }
            else if (this.CanBeDowngraded || this.CanBeUpgraded)
            {
                await this.pluginPackageManager.UpdatePluginAsync(this.Package);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        [NotNull]
        public async Task UninstallAsync()
        {
            await this.pluginPackageManager.UninstallPluginAsync(this.Package);
        }

        [UsedImplicitly]
        public String Id
        {
            get { return this.Package.Id; }
        }

        public String Author
        {
            get { return String.Join(", ", this.Package.Authors); }
        }

        public SemanticVersion Version
        {
            get { return this.Package.Version; }
        }

        public String Description
        {
            get { return this.Package.Description ?? this.Package.Summary; }
        }

        void IDisposable.Dispose()
        {
            this.subscriptionTokens.UnsubscribeAll();
        }
    }
}
