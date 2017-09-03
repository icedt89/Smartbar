namespace JanHafner.Smartbar.Infrastructure.Notifications
{
    using System;
    using System.ComponentModel.Composition;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Prism.Events;

    internal abstract class BackgroundNotifier : IDisposable
    {
        [NotNull]
        protected readonly IEventAggregator EventAggregator;

        [CanBeNull]
        private CancellationTokenSource cancellationTokenSource;

        private readonly TimeSpan waitTime;

        [ImportingConstructor]
        protected BackgroundNotifier([NotNull] IEventAggregator eventAggregator, TimeSpan waitTime)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.EventAggregator = eventAggregator;
            this.waitTime = waitTime;
        }

        public Boolean IsRunning { get; private set; }

        public void Start()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                this.IsRunning = true;

                await this.AfterStartAsync();

                while (!this.cancellationTokenSource.IsCancellationRequested)
                {
                    await this.WorkAsync();

                    await Task.Delay(this.waitTime, this.cancellationTokenSource.Token);
                }
            }, this.cancellationTokenSource.Token).ContinueWith(previousTask =>
            {
                this.IsRunning = false;
                this.cancellationTokenSource = null;
            }, TaskContinuationOptions.NotOnFaulted);
        }

        public void Cancel()
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.cancellationTokenSource?.Cancel();
        }

        protected virtual Task AfterStartAsync()
        {
            return Task.CompletedTask;
        }

        protected abstract Task WorkAsync();

        public virtual void Dispose()
        {
            this.Cancel();
            this.cancellationTokenSource?.Dispose();
        }
    }
}
