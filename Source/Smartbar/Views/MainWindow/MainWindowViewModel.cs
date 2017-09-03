namespace JanHafner.Smartbar.Views.MainWindow
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using global::Smartbar.Updater.Core;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Infrastructure;
    using JanHafner.Smartbar.Infrastructure.Commanding.Groups;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Smartbar.Infrastructure.Notifications;
    using JanHafner.Smartbar.Localization;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.Group;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using NuGet;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using ICommand = System.Windows.Input.ICommand;

    internal sealed class MainWindowViewModel : BindableBase, IDisposable
    {
        [NotNull] private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull] private readonly IEventAggregator eventAggregator;

        [NotNull] private readonly ISmartbarService smartbarService;

        [NotNull] private readonly ISmartbarSettings smartbarSettings;

        [NotNull] private readonly IWindowService windowService;

        [NotNull] private readonly IPluginPackageManager pluginPackageManager;

        [NotNull] private readonly ViewModelFactory viewModelFactory;

        [NotNull] private readonly IUIExtensionService uiExtensionService;

        [NotNull] private readonly IModuleExplorer moduleExplorer;

        [NotNull] private readonly ILocalizationService localizationService;

        [NotNull] private readonly ISmartbarUpdater smartbarUpdater;

        [NotNull] private readonly ICommandDispatcher commandDispatcher;

        [CanBeNull] private String hoveredApplicationName;

        private Int32 gridCellContentSize;

        private Int32 gridCellSpacing;

        private Int32 rows;

        private Int32 columns;

        private Guid selectedGroupId;

        private Boolean showStatusbar;

        private Boolean directEditOfGroupHeader;

        private Boolean snapOnScreenBorders;

        private Boolean isPinned;

        public MainWindowViewModel([NotNull] IEnumerable<GroupViewModel> groupViewModels,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarService smartbarService,
            [NotNull] ISmartbarSettings smartbarSettings, [NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IPluginPackageManager pluginPackageManager, ViewModelFactory viewModelFactory,
            [NotNull] IUIExtensionService uiExtensionService,
            [NotNull] IModuleExplorer moduleExplorer, [NotNull] ILocalizationService localizationService,
            [NotNull] IPluginConfigurationService pluginConfigurationService, [NotNull] ISmartbarUpdater smartbarUpdater,
            [NotNull] WpfHotKeyManager wpfHotKeyManager)
        {
            if (groupViewModels == null)
            {
                throw new ArgumentNullException(nameof(groupViewModels));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (pluginPackageManager == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageManager));
            }

            if (uiExtensionService == null)
            {
                throw new ArgumentNullException(nameof(uiExtensionService));
            }

            if (moduleExplorer == null)
            {
                throw new ArgumentNullException(nameof(moduleExplorer));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            if (pluginConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(pluginConfigurationService));
            }

            if (smartbarUpdater == null)
            {
                throw new ArgumentNullException(nameof(smartbarUpdater));
            }

            if (wpfHotKeyManager == null)
            {
                throw new ArgumentNullException(nameof(wpfHotKeyManager));
            }

            this.WpfHotKeyManager = wpfHotKeyManager;
            this.pluginPackageManager = pluginPackageManager;
            this.viewModelFactory = viewModelFactory;
            this.uiExtensionService = uiExtensionService;
            this.moduleExplorer = moduleExplorer;
            this.localizationService = localizationService;
            this.smartbarUpdater = smartbarUpdater;
            this.subscriptionTokens = new List<SubscriptionToken>();
            this.eventAggregator = eventAggregator;
            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.smartbarService = smartbarService;
            this.Groups = new ObservableCollection<GroupViewModel>(groupViewModels);
            this.selectedGroupId = this.Groups.Single(group => group.IsSelected).Id;
            this.smartbarSettings = smartbarSettings;

            this.rows = smartbarSettings.Rows;
            this.isPinned = smartbarSettings.PinSmartbarAtPosition;
            this.columns = smartbarSettings.Columns;
            this.gridCellContentSize = smartbarSettings.GridCellContentSize;
            this.gridCellSpacing = smartbarSettings.GridCellSpacing;
            this.showStatusbar = smartbarSettings.ShowStatusbar;
            this.snapOnScreenBorders = smartbarSettings.SnapOnScreenBorders;
            this.directEditOfGroupHeader = smartbarSettings.DirectEditOfGroupHeader;

            this.InitializeSubscriptions();
        }

        private void InitializeSubscriptions()
        {
            // TODO: If it works...
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<PluginUpdatesAvailable>().Subscribe(
                this.OnPluginUpdatesAvailable, ThreadOption.UIThread, true,
                updatablePackages => updatablePackages.Any()));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<PluginUpdatesAvailable>().Subscribe(
                this.OnPluginUpdatesAvailable, ThreadOption.UIThread, true));


            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<SmartbarUpdateAvailable>().Subscribe(
                this.OnSmartbarUpdateAvailable, ThreadOption.UIThread, true));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<CommandHandlerFaulted>().Subscribe(async data =>
            {
                data.Handled = true;

                await
                    this.windowService.ShowSimpleModalErrorDialog(
                        UserMessages.ErrorsOccuredDuringCreateOperationMessage,
                        UserMessages.ErrorsOccuredDuringCreateOperationTitle);
            }, ThreadOption.PublisherThread, true,
                data => !data.Handled && data.Command is CreateApplicationContainerCommand));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<SmartbarSettingsUpdated>()
                .Subscribe(smartbarSettings =>
                {
                    // TODO: WEG! this.SmartbarSettings = smartbarSettings;
                    // Reroute Property getter/setter to this.smartbarSettings
                    // Just call OnPropertyChange for every property declared here currently.
                    this.Rows = smartbarSettings.Rows;
                    this.Columns = smartbarSettings.Columns;
                    this.GridCellContentSize = smartbarSettings.GridCellContentSize;
                    this.GridCellSpacing = smartbarSettings.GridCellSpacing;
                    this.ShowStatusbar = smartbarSettings.ShowStatusbar;
                    this.DirectEditOfGroupHeader = smartbarSettings.DirectEditOfGroupHeader;
                    this.SnapOnScreenBorders = smartbarSettings.SnapOnScreenBorders;
                    this.IsPinned = smartbarSettings.PinSmartbarAtPosition;

                    this.OnPropertyChanged(() => this.ShowGroupHeader);
                    this.OnPropertyChanged(() => this.SaveWindowPosition);
                }));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupCreated>().Subscribe(async group =>
            {
                var groupViewModel = this.viewModelFactory.CreateGroupViewModel(group);
                this.Groups.Insert(groupViewModel.Position, groupViewModel);

                this.Groups.ForEach(g =>
                {
                    g.CanBeDeleted = this.Groups.Count > 1;
                    g.CanShiftRight = g.Position < this.Groups.Max(_ => _.Position);
                });

                if (this.smartbarSettings.AutoSelectCreatedGroup)
                {
                    await this.commandDispatcher.DispatchAsync(new SelectGroupCommand(group.Id));
                }

                this.OnPropertyChanged(() => this.ShowGroupHeader);
            }));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupDeleted>().Subscribe(group =>
            {
                var groupViewModel = this.Groups.Single(_ => _.Id == group.Id);
                this.Groups.Remove(groupViewModel);

                this.Groups.ForEach(g =>
                {
                    g.CanBeDeleted = this.Groups.Count > 1;
                    g.CanShiftRight = g.Position < this.Groups.Max(_ => _.Position);
                });

                this.OnPropertyChanged(() => this.ShowGroupHeader);
            }));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupRepositioned>().Subscribe(data =>
            {
                var currentGroup = this.Groups.Single(group => group.Id == data.Group.Id);
                currentGroup.Reposition(data.Group.Position);

                /* 
                * If the GroupRepositioned-event for the first group is catched and this group is moved from 0 to 1,
                * then the Move(...)-method changes the internal index of the group already present at index 1 to 0.
                * 
                * So when the second GroupRepositioned-event is catched it would try to move the second group, which is in the internal list 
                * already at position 0(!) from 0 to 0 which causes a ArgumentOutOfRangeException and terminates the program.
                * 
                * This this code checks if the internal index is already the index to which the group will be positioned to, if so: skip the Move(...) operation.
                */
                var currentGroupIndex = this.Groups.IndexOf(currentGroup);
                if (currentGroupIndex != data.Group.Position)
                {
                    this.Groups.Move(currentGroupIndex, data.Group.Position);
                }

                this.Groups.ForEach(group => group.CanShiftRight = group.Position < this.Groups.Max(_ => _.Position));
            }));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupSelected>().Subscribe(selectedGroup =>
            {
                this.selectedGroupId = selectedGroup.Id;

                this.OnPropertyChanged(() => this.SelectedGroup);
            }));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<UpdateStatusbarText>().Subscribe(text =>
            {
                this.HoveredApplicationName = text;
            }));
        }

        #region SmartbarUpdateAvailable

        private void OnSmartbarUpdateAvailable(IPackage updatePackage)
        {
            var handler = this.smartbarUpdateAvailable;
            handler?.Invoke(this, new SmartbarUpdateAvailableArgs(updatePackage));
        }

        private EventHandler<SmartbarUpdateAvailableArgs> smartbarUpdateAvailable;

        public event EventHandler<SmartbarUpdateAvailableArgs> SmartbarUpdateAvailable
        {
            add { this.smartbarUpdateAvailable += value; }
            remove { this.smartbarUpdateAvailable -= value; }
        }

        #endregion

        #region PluginUpdatesAvailable

        private void OnPluginUpdatesAvailable(IEnumerable<IPackage> updatablePackages)
        {
            var handler = this.pluginUpdatesAvailable;
            handler?.Invoke(this, new PluginUpdatesAvailableArgs(updatablePackages));
        }

        private EventHandler<PluginUpdatesAvailableArgs> pluginUpdatesAvailable;

        public event EventHandler<PluginUpdatesAvailableArgs> PluginUpdatesAvailable
        {
            add { this.pluginUpdatesAvailable += value; }
            remove { this.pluginUpdatesAvailable -= value; }
        }

        #endregion

        [NotNull]
        public WpfHotKeyManager WpfHotKeyManager { get; private set; }

        public Point? InitialPosition
        {
            get
            {
                if (this.smartbarSettings.RestorePosition)
                {
                    return SmartbarStartupPositionAdjuster.GetAdjustedPosition(this.smartbarSettings.InitialPosition);
                }

                return null;
            }
            set
            {
                if (this.smartbarSettings.RestorePosition && value.HasValue)
                {
                    this.smartbarSettings.InitialPosition = value.Value;

                    Task.Run(async () => await this.smartbarSettings.SaveChangesAsync());
                }
            }
        }

        [CanBeNull]
        public GroupViewModel SelectedGroup
        {
            get { return this.Groups.Single(group => group.Id == this.selectedGroupId); }
            set
            {
                if (value != null && this.SelectedGroup != value)
                {
                    Task.Run(async () => await this.commandDispatcher.DispatchAsync(new SelectGroupCommand(value.Id)))
                        .Wait();
                }
            }
        }

        [NotNull]
        public ObservableCollection<GroupViewModel> Groups { get; private set; }

        [CanBeNull]
        public String HoveredApplicationName
        {
            get { return this.hoveredApplicationName; }
            private set { this.SetProperty(ref this.hoveredApplicationName, value); }
        }

        public Boolean SnapOnScreenBorders
        {
            get { return this.snapOnScreenBorders; }
            private set { this.SetProperty(ref this.snapOnScreenBorders, value); }
        }

        public Boolean IsPinned
        {
            get { return this.isPinned; }
            private set { this.SetProperty(ref this.isPinned, value); }
        }

        public Int32 SnapOnScreenBordersOffset
        {
            get { return this.smartbarSettings.SnapOnScreenBordersOffset; }
        }

        public Boolean ShowStatusbar
        {
            get { return this.showStatusbar; }
            private set { this.SetProperty(ref this.showStatusbar, value); }
        }

        public Boolean DirectEditOfGroupHeader
        {
            get { return this.directEditOfGroupHeader; }
            private set { this.SetProperty(ref this.directEditOfGroupHeader, value); }
        }

        public Int32 GridCellContentSize
        {
            get { return this.gridCellContentSize; }
            private set { this.SetProperty(ref this.gridCellContentSize, value); }
        }

        public Int32 GridCellSpacing
        {
            get { return this.gridCellSpacing; }
            private set { this.SetProperty(ref this.gridCellSpacing, value); }
        }

        public Int32 Rows
        {
            get { return this.rows; }
            private set { this.SetProperty(ref this.rows, value); }
        }

        public Int32 Columns
        {
            get { return this.columns; }
            private set { this.SetProperty(ref this.columns, value); }
        }

        public Boolean ShowGroupHeader
        {
            get { return !(this.Groups.Count == 1 && this.smartbarSettings.HideGroupHeaderIfOnlyOneAvailable); }
        }

        public Boolean SaveWindowPosition
        {
            get { return this.smartbarSettings.RestorePosition; }
        }

        [NotNull]
        public ICommand EditSmartbarSettingsCommand
        {
            get
            {
                return new MainWindowViewModelEditSmartbarSettingsCommand(this.smartbarSettings, this.commandDispatcher,
                    this.windowService, this.uiExtensionService, this.localizationService, this.smartbarService,
                    this.smartbarUpdater);
            }
        }

        [NotNull]
        public ICommand CreateGroupCommand
        {
            get { return new MainWindowViewModelCreateGroupCommand(this.commandDispatcher, this.windowService); }
        }

        [CanBeNull]
        private ICommand showPluginManagementCommand;

        [NotNull]
        public ICommand ShowPluginManagementCommand
        {
            get
            {
                // Cache because this getter is manually called by clicking on the ballon tip.
                if (this.showPluginManagementCommand == null)
                {
                    this.showPluginManagementCommand =
                        new MainWindowViewModelOpenPluginManagerCommand(this.pluginPackageManager, this.eventAggregator,
                            this.smartbarSettings, this.moduleExplorer, this.windowService);
                }

                return this.showPluginManagementCommand;
            }
        }

        [CanBeNull]
        private ICommand startApplicationUpdateCommand;

        [NotNull]
        public ICommand StartApplicationUpdateCommand
        {
            get
            {
                // Cache because this getter is manually called by clicking on the ballon tip.
                if (this.startApplicationUpdateCommand == null)
                {
                    this.startApplicationUpdateCommand = new DelegateCommand(() =>
                    {
                        Process.Start(this.smartbarSettings.SmartbarUpdaterExePath);
                        Application.Current.Shutdown(0);
                    }, () => File.Exists(this.smartbarSettings.SmartbarUpdaterExePath));
                }

                return this.startApplicationUpdateCommand;
            }
        }

        void IDisposable.Dispose()
        {
            this.subscriptionTokens.UnsubscribeAll();
        }
    }
}