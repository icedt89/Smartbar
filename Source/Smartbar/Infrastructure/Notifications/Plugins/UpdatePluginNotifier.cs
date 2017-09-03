namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Infrastructure.Commanding.Plugins;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using NuGet;
    using Prism.Events;

    [Export(nameof(UpdatePluginNotifier), typeof(BackgroundNotifier))]
    internal sealed class UpdatePluginNotifier : BackgroundNotifier
    {
        [NotNull]
        private readonly IPluginPackageManager pluginPackageManager;

        [NotNull]
        private readonly IPluginNotificationFilter pluginNotificationFilter;

        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull]
        private IEnumerable<IPackage> currentlyInstalledPluginPackages;

        [ImportingConstructor]
        public UpdatePluginNotifier([NotNull] IEventAggregator eventAggregator,
            [NotNull] IPluginPackageManager pluginPackageManager,
            [NotNull] IPluginNotificationFilter pluginNotificationFilter,
            [NotNull] ISmartbarSettings smartbarSettings)
            : base(eventAggregator, TimeSpan.FromSeconds(60))
        {
            if (pluginPackageManager == null)
            {
                throw new ArgumentNullException(nameof(pluginPackageManager));
            }

            if (pluginNotificationFilter == null)
            {
                throw new ArgumentNullException(nameof(pluginNotificationFilter));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            this.pluginPackageManager = pluginPackageManager;
            this.pluginNotificationFilter = pluginNotificationFilter;

            this.subscriptionTokens = new List<SubscriptionToken>
            {
                eventAggregator.GetEvent<PluginInstalled>().Subscribe(async package =>
                {
                    this.currentlyInstalledPluginPackages = await this.pluginPackageManager.GetInstalledPluginPackagesAsync();
                }, ThreadOption.BackgroundThread, true, package => smartbarSettings.NotificationOnPluginUpdates)
            };
        }

        protected override async Task AfterStartAsync()
        {
            this.currentlyInstalledPluginPackages = await this.pluginPackageManager.GetInstalledPluginPackagesAsync();
        }

        protected override async Task WorkAsync()
        {
            IEnumerable<IPackage> availableUpdates;
            try
            {
                availableUpdates = (await this.pluginPackageManager.GetAvailablePluginUpdatesAsync(this.currentlyInstalledPluginPackages)).ToList();
            }
            catch (WebException)
            {
                return;
            }

            availableUpdates = this.pluginNotificationFilter.Filter(availableUpdates).ToList();
            if (availableUpdates.Any())
            {
                this.EventAggregator.GetEvent<PluginUpdatesAvailable>().Publish(availableUpdates);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.subscriptionTokens.UnsubscribeAll();
        }
    }
}
