namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(BackgroundNotifierAggregator))]
    internal sealed class BackgroundNotifierAggregator : IDisposable
    {
        [NotNull]
        private readonly BackgroundNotifier updateSmartbarNotifier;

        [NotNull]
        private readonly BackgroundNotifier updatePluginNotifier;

        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;
            
            
        [ImportingConstructor]
        public BackgroundNotifierAggregator([NotNull] IEventAggregator eventAggregator,
            [NotNull, Import(nameof(UpdateSmartbarNotifier), typeof (BackgroundNotifier))] BackgroundNotifier updateSmartbarNotifier,
            [NotNull, Import(nameof(UpdatePluginNotifier), typeof (BackgroundNotifier))] BackgroundNotifier UpdatePluginNotifier,
            ISmartbarSettings smartbarSettings)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (updateSmartbarNotifier == null)
            {
                throw new ArgumentNullException(nameof(updateSmartbarNotifier));
            }

            if (UpdatePluginNotifier == null)
            {
                throw new ArgumentNullException(nameof(UpdatePluginNotifier));
            }

            this.updateSmartbarNotifier = updateSmartbarNotifier;
            this.updatePluginNotifier = UpdatePluginNotifier;
            this.smartbarSettings = smartbarSettings;
            this.subscriptionTokens = new List<SubscriptionToken>
            {
               eventAggregator.GetEvent<SmartbarSettingsUpdated>().Subscribe(_ =>
               {
                   this.ToggleUpdatePluginNotifier();
                   this.ToggleUpdateSmartbarNotifier();
               }, ThreadOption.BackgroundThread, true)
            };
        }

        private void ToggleUpdatePluginNotifier()
        {
            if (this.smartbarSettings.NotificationOnPluginUpdates)
            {
                if (!this.updatePluginNotifier.IsRunning)
                {
                    this.updatePluginNotifier.Start();
                }
            }
            else if (this.updatePluginNotifier.IsRunning)
            {
                this.updatePluginNotifier.Cancel();
            }
        }

        private void ToggleUpdateSmartbarNotifier()
        {
            if (this.smartbarSettings.NotificationOnSmartbarUpdate)
            {
                if (!this.updateSmartbarNotifier.IsRunning)
                {
                    this.updateSmartbarNotifier.Start();
                }
            }
            else if (this.updateSmartbarNotifier.IsRunning)
            {
                this.updateSmartbarNotifier.Cancel();
            }
        }

        public void Start()
        {
            this.ToggleUpdatePluginNotifier();
            this.ToggleUpdateSmartbarNotifier();
        }

        public void Dispose()
        {
            this.updatePluginNotifier.Dispose();
            this.updateSmartbarNotifier.Dispose();

            this.subscriptionTokens.UnsubscribeAll();
        }
    }
}
