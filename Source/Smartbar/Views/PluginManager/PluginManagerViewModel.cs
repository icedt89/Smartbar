namespace JanHafner.Smartbar.Views.PluginManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Infrastructure.Commanding.Plugins;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.PluginManager.Loading;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using ICommand = System.Windows.Input.ICommand;

    internal sealed class PluginManagerViewModel : BindableBase, IDisposable
    {
        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull]
        private readonly ObservableCollection<PluginPackageViewModel> pluginPackageViewModels;

        [CanBeNull]
        private CancellationTokenSource cancellationTokenSource;

        [NotNull]
        private readonly PluginLoader pluginLoader;

        private Boolean isOnlinePackageSourceUnavailable;

        private Boolean noPackagesAvailable;

        private Boolean installedPluginsChanged;

        private Boolean actionInProgress;

        private Boolean loadAborted;

        [NotNull]
        private readonly IModuleExplorer moduleExplorer;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        private readonly ISmartbarSettings smartbarSettings;

        public PluginManagerViewModel([NotNull] IWindowService windowService,
            [NotNull] IPluginPackageManager pluginPackageManager,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarSettings smartbarSettings, [NotNull] IModuleExplorer moduleExplorer)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (pluginPackageManager == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageManager));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (moduleExplorer == null)
            {
                throw new ArgumentNullException(nameof(moduleExplorer));
            }

            this.moduleExplorer = moduleExplorer;
            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.smartbarSettings = smartbarSettings;
            this.pluginPackageViewModels = new ObservableCollection<PluginPackageViewModel>();
            this.subscriptionTokens = new List<SubscriptionToken>
            {
                eventAggregator.GetEvent<PluginInstalled>().Subscribe(p =>
                {
                    this.InstalledPluginsChanged = true;
                    this.ActionInProgress = false;
                }, ThreadOption.UIThread, true),
                eventAggregator.GetEvent<PluginUninstalled>().Subscribe(p =>
                {
                    this.InstalledPluginsChanged = true;
                    this.ActionInProgress = false;
                }, ThreadOption.UIThread, true)
            };

            this.pluginLoader = new PluginLoader(pluginPackageManager, eventAggregator, new RunOnDispatcherProgress<PluginLoaderProgress>(
                progress =>
                {
                    if (this.ActionInProgress)
                    {
                        this.pluginPackageViewModels.Add(progress.PluginPackageViewModel);
                    }

                    if (progress.IsFinished || (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested))
                    {
                        this.ActionInProgress = false;
                    }
                }));
        }

        private void UnsetCancellationToken()
        {
            if (this.cancellationTokenSource == null)
            {
                return;
            }

            if (!this.cancellationTokenSource.IsCancellationRequested)
            {
                this.cancellationTokenSource.Cancel();
            }

            this.cancellationTokenSource = null;
        }

        [NotNull]
        public IEnumerable<PluginPackageViewModel> PluginPackageViewModels
        {
            get
            {
                return this.pluginPackageViewModels;
            }
        }

        public async Task LoadPluginPackagesAsync()
        {
            this.ActionInProgress = true;
            this.IsOnlinePackageSourceUnavailable = false;
            this.NoPackagesAvailable = false;
            this.LoadAborted = false;

            this.pluginPackageViewModels.DisposeAndClear();
            this.cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                var pluginLoaderResult = await this.pluginLoader.LoadAsync(this.cancellationTokenSource.Token);
                this.IsOnlinePackageSourceUnavailable = pluginLoaderResult.OnlinePackageSourceUnavailable;
                if (!this.isOnlinePackageSourceUnavailable && !pluginLoaderResult.PluginsLoaded)
                {
                    this.NoPackagesAvailable = true;
                }

                if (!pluginLoaderResult.PluginsLoaded)
                {
                    this.ActionInProgress = false;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is OperationCanceledException))
                {
                    this.LoadAborted = true;
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));
                }

                this.ActionInProgress = false;
            }
        }

        public Boolean LoadAborted
        {
            get { return this.loadAborted; }
            private set { this.SetProperty(ref this.loadAborted, value); }
        }

        [NotNull]
        public ICommand InstallPluginCommand
        {
            get
            {
                return new DelegateCommand<PluginPackageViewModel>(async p =>
                {
                    this.ActionInProgress = true;

                    await p.InstallAsync();
                }, p => !this.ActionInProgress).ObservesProperty(() => this.ActionInProgress);
            }
        }

        [NotNull]
        public ICommand UninstallPluginCommand
        {
            get
            {
                return new DelegateCommand<PluginPackageViewModel>(async p =>
                {
                    this.ActionInProgress = true;

                    await p.UninstallAsync();
                }, p => !this.ActionInProgress).ObservesProperty(() => this.ActionInProgress);
            }
        }

        public Boolean IsOnlinePackageSourceUnavailable
        {
            get
            {
                return this.isOnlinePackageSourceUnavailable;
            }
            private set
            {
                this.SetProperty(ref this.isOnlinePackageSourceUnavailable, value);
            }
        }

        public Boolean ActionInProgress
        {
            get
            {
                return this.actionInProgress;
            }
            private set
            {
                if (this.SetProperty(ref this.actionInProgress, value) && !value)
                {
                    this.UnsetCancellationToken();
                }
            }
        }

        public Boolean InstalledPluginsChanged
        {
            get
            {
                return this.installedPluginsChanged;
            }
            private set
            {
                this.SetProperty(ref this.installedPluginsChanged, value);
            }
        }
        
        public Boolean NoPackagesAvailable
        {
            get
            {
                return this.noPackagesAvailable;
            }
            private set
            {
                this.SetProperty(ref this.noPackagesAvailable, value);
            }
        }

        [NotNull]
        public ICommand RefreshOnlinePackageSourceCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await this.LoadPluginPackagesAsync();
                }, () => !this.ActionInProgress).ObservesProperty(() => this.ActionInProgress);
            }
        }

        public Boolean IsModuleExplorerAvailable
        {
            get { return this.smartbarSettings.IsModuleExplorerAvailable; }
        }

        [NotNull]
        public ICommand OpenModuleExplorerCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var moduleExplorerViewModel = new ModuleExplorer.ModuleExplorerViewModel(this.windowService, this.moduleExplorer);
                    await this.windowService.ShowWindowAsync<ModuleExplorer.ModuleExplorer>(moduleExplorerViewModel, result => {});
                }, () => this.smartbarSettings.IsModuleExplorerAvailable);
            }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<PluginManagerViewModel>(this, viewModel => !viewModel.ActionInProgress, this.windowService).ObservesProperty(() => this.ActionInProgress);
            }
        }

        void IDisposable.Dispose()
        {
            this.cancellationTokenSource?.Cancel();
            this.pluginPackageViewModels.ForEach(ppvm => ppvm.ToDisposable().Dispose());
            this.subscriptionTokens.UnsubscribeAll();
        }
    }
}
