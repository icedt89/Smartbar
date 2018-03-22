namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System;
    using System.ComponentModel.Composition;
    using System.Net;
    using System.Threading.Tasks;
    using global::Smartbar.Updater.Core;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(nameof(UpdateSmartbarNotifier), typeof(BackgroundNotifier))]
    internal sealed class UpdateSmartbarNotifier : BackgroundNotifier
    {
        [NotNull]
        private readonly ISmartbarUpdater smartbarUpdater;

        [CanBeNull]
        private Update currentUpdateInformation;

        [ImportingConstructor]
        public UpdateSmartbarNotifier([NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarUpdater smartbarUpdater)
            : base(eventAggregator, TimeSpan.FromSeconds(60 * 30))
        {
            if (smartbarUpdater == null)
            {
                throw new ArgumentNullException(nameof(smartbarUpdater));
            }

            this.smartbarUpdater = smartbarUpdater;
        }

        protected override async Task AfterStartAsync()
        {
            this.currentUpdateInformation = await this.smartbarUpdater.GetUpdateAsync();
        }

        protected override async Task WorkAsync()
        {
            try
            {
                await this.smartbarUpdater.UpdateRemoteInformationAsync(this.currentUpdateInformation);
            }
            catch (Exception exception)
            {
	            if (exception is WebException || exception is UriFormatException)
	            {
		            return;
				}
			}

            if (this.currentUpdateInformation?.Remote != null)
            {
                this.EventAggregator.GetEvent<SmartbarUpdateAvailable>().Publish(this.currentUpdateInformation.Remote.UpdatePackage);
            }
        }
    }
}