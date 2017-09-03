namespace JanHafner.Smartbar.Views.MainWindow
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Hardcodet.Wpf.TaskbarNotification;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.Localization;
    using JetBrains.Annotations;

    partial class MainWindow
    {
        private LastReceivedNotificationType? lastReceivedNotificationType;

        private MainWindow()
        {
            this.InitializeComponent();
        }

        public MainWindow([NotNull] MainWindowViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            this.DataContext = viewModel;
            
            this.InitializeComponent();

            this.ConfigureNotificationArea(viewModel);

            this.Loaded += (sender, args) => viewModel.WpfHotKeyManager.BindToWindow(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.TaskbarIcon.Dispose();
        }

        private void ConfigureNotificationArea(MainWindowViewModel mainWindowViewModel)
        {
            mainWindowViewModel.PluginUpdatesAvailable += this.ShowPluginUpdatesAvailableBallon;
            mainWindowViewModel.SmartbarUpdateAvailable += this.ShowSmartbarUpdateAvailableBalloon;
            this.TaskbarIcon.TrayBalloonTipClicked += (o, args) =>
            {
                if (!this.lastReceivedNotificationType.HasValue)
                {
                    return;
                }

                switch (this.lastReceivedNotificationType.Value)
                {
                    case LastReceivedNotificationType.PluginUpdates:
                        mainWindowViewModel.ShowPluginManagementCommand.Execute(null);
                        break;
                    case LastReceivedNotificationType.SmartbarUpdate:
                        mainWindowViewModel.StartApplicationUpdateCommand.Execute(null);
                        break;
                }

                this.lastReceivedNotificationType = null;
            };
        }

        private void ShowSmartbarUpdateAvailableBalloon(Object sender, SmartbarUpdateAvailableArgs smartbarUpdateAvailableArgs)
        {
            this.lastReceivedNotificationType = LastReceivedNotificationType.SmartbarUpdate;

            var balloonTitle = LocalizationService.Current.Localize<Localization.MainWindow>(nameof(Localization.MainWindow.BallonNotificationSmartbarUpdateAvailableTitle));
            var balloonText = LocalizationService.Current.Localize<Localization.MainWindow>(nameof(Localization.MainWindow.BallonNotificationSmartbarUpdateAvailableText));

            this.TaskbarIcon.ShowBalloonTip(balloonTitle, String.Format(balloonText, smartbarUpdateAvailableArgs.UpdatePackage.Version), BalloonIcon.Info);
        }

        private void ShowPluginUpdatesAvailableBallon(Object sender, PluginUpdatesAvailableArgs pluginUpdatesAvailableArgs)
        {
            this.lastReceivedNotificationType = LastReceivedNotificationType.PluginUpdates;

            var balloonTitle = LocalizationService.Current.Localize<Localization.MainWindow>(nameof(Localization.MainWindow.BallonNotificationPluginUpdatesAvailableTitle));
            var balloonText = LocalizationService.Current.Localize<Localization.MainWindow>(nameof(Localization.MainWindow.BallonNotificationPluginUpdatesAvailableText));

            this.TaskbarIcon.ShowBalloonTip(balloonTitle, String.Format(balloonText, pluginUpdatesAvailableArgs.UpdatablePackages.Count()), BalloonIcon.Info);
        }

        private enum LastReceivedNotificationType
        {
            SmartbarUpdate = 0,

            PluginUpdates = 1
        }
    }
}
