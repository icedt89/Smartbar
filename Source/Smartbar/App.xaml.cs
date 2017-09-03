namespace JanHafner.Smartbar
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Configuration;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;

    internal partial class App : IDisposable
    {
        [Import, CanBeNull]
        private Bootstrapper bootstrapper;

        [NotNull] 
        private readonly CompositionContainer compositionContainer;

        [CanBeNull, ImportMany(typeof(IApplicationLifecycleObserver))]
        private IEnumerable<IApplicationLifecycleObserver> applicationLifecycleObservers;

        public App()
        {
            var basePluginsDirectory = ConfigurationManager.AppSettings.CreateAndGetPluginsDirectory();
            this.compositionContainer = new SmartbarCompositionContainer(basePluginsDirectory);
            this.compositionContainer.ComposeExportedValue((IModuleExplorer) this.compositionContainer);
        }

        protected override async void OnStartup([CanBeNull] StartupEventArgs e)
        {
            // Force construction of IEventAggregator on UI thread to enable usage of ThreadOption.UIThread!
            this.compositionContainer.ComposeParts(this);

            await this.bootstrapper.BootstrapAsync(this, e.Args);

            this.MainWindow.Loaded += (sender, args) => this.applicationLifecycleObservers.ForEach(applicationLifecycleObserver => applicationLifecycleObserver.AfterInitialization());
            this.MainWindow.Closing += (sender, args) => this.Dispose();

            this.MainWindow.Show();
        }

        public void Dispose()
        {
            this.applicationLifecycleObservers.ForEach(applicationLifecycleObserver => applicationLifecycleObserver.BeforeShutdown());

            this.compositionContainer.Dispose();
            this.bootstrapper.Dispose();
        }
    }
}