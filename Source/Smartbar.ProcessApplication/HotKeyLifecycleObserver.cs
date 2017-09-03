namespace JanHafner.Smartbar.ProcessApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Toolkit.Windows.HotKey;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof (IApplicationLifecycleObserver))]
    internal sealed class HotKeyLifecycleObserver : IDisposable, IApplicationLifecycleObserver
    {
        [NotNull] private readonly WpfHotKeyManager wpfHotKeyManager;

        [NotNull] private readonly ICommandDispatcher commandDispatcher;

        [NotNull] private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull] private readonly IDictionary<ProcessApplication, HotKeyRegistration> allObservedProcessApplications;

        [ImportingConstructor]
        public HotKeyLifecycleObserver([NotNull] ISmartbarDbContext smartbarDbContext,
            [NotNull] WpfHotKeyManager wpfHotKeyManager,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] ICommandDispatcher commandDispatcher)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            if (wpfHotKeyManager == null)
            {
                throw new ArgumentNullException(nameof(wpfHotKeyManager));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            this.wpfHotKeyManager = wpfHotKeyManager;
            this.commandDispatcher = commandDispatcher;
            this.allObservedProcessApplications =
                smartbarDbContext.Groups.SelectMany(group => group.Applications)
                    .OfType<ProcessApplication>()
                    .ToDictionary(ProcessApplication => ProcessApplication, _ => (HotKeyRegistration) null);

            this.subscriptionTokens = new List<SubscriptionToken>
            {
                eventAggregator.GetEvent<ApplicationHotKeyUpdated>().Subscribe(applicationId =>
                {
                    var processApplication =
                        this.allObservedProcessApplications.SingleOrDefault(_ => _.Key.Id == applicationId);
                    if (processApplication.Key == null)
                    {
                        return;
                    }

                    if (this.allObservedProcessApplications[processApplication.Key] != null)
                    {
                        this.wpfHotKeyManager.Unregister(processApplication.Value.HotKeyModifier,
                            processApplication.Value.HotKey);
                    }

                    if (processApplication.Key.HasHotKey)
                    {
                        this.allObservedProcessApplications[processApplication.Key] =
                            this.TryRegisterProcessApplicationHotKey(processApplication.Key);
                    }
                }, ThreadOption.PublisherThread, true),
                eventAggregator.GetEvent<ApplicationsCreated>().Subscribe(data =>
                {
                    foreach (var processApplication in data.Applications.OfType<ProcessApplication>())
                    {
                        var hotKeyRegistered = this.TryRegisterProcessApplicationHotKey(processApplication);

                        this.allObservedProcessApplications.Add(processApplication, hotKeyRegistered);
                    }
                }, ThreadOption.PublisherThread, true),
                eventAggregator.GetEvent<ApplicationsDeleted>().Subscribe(data =>
                {
                    foreach (var processApplication in data.Applications.OfType<ProcessApplication>())
                    {
                        HotKeyRegistration hotKeyRegistration = null;
                        if (this.allObservedProcessApplications.TryGetValue(processApplication, out hotKeyRegistration))
                        {
                            if (hotKeyRegistration != null)
                            {
                                this.wpfHotKeyManager.Unregister(processApplication.HotKeyModifier,
                                    processApplication.HotKey);
                            }

                            this.allObservedProcessApplications.Remove(processApplication);
                        }
                    }
                }, ThreadOption.PublisherThread, true)
            };
        }

        public void AfterInitialization()
        {
            foreach (var processApplication in this.allObservedProcessApplications.ToList())
            {
                this.allObservedProcessApplications[processApplication.Key] =
                    this.TryRegisterProcessApplicationHotKey(processApplication.Key);
            }
        }

        private HotKeyRegistration TryRegisterProcessApplicationHotKey(ProcessApplication processApplication)
        {
            if (!processApplication.HasHotKey)
            {
                return null;
            }

            try
            {
                this.wpfHotKeyManager.Register(processApplication.HotKeyModifier, processApplication.HotKey, async () =>
                {
                    await
                        this.commandDispatcher.DispatchAsync(new ExecuteProcessApplicationCommand(processApplication.Id));
                });

                return new HotKeyRegistration(processApplication.HotKeyModifier, processApplication.HotKey);
            }
            catch
            {
                return null;
            }
        }

        public void BeforeShutdown()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.wpfHotKeyManager.Dispose();
            this.subscriptionTokens.UnsubscribeAll();
        }

        private sealed class HotKeyRegistration
        {
            public HotKeyRegistration(HotKeyModifier hotKeyModifier, Key hotKey)
            {
                this.HotKeyModifier = hotKeyModifier;
                this.HotKey = hotKey;
            }

            public HotKeyModifier HotKeyModifier { get; private set; }

            public Key HotKey { get; private set; }
        }
    }
}