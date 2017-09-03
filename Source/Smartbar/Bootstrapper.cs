namespace JanHafner.Smartbar
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Infrastructure;
    using JanHafner.Smartbar.Infrastructure.CommandLine;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Smartbar.Infrastructure.Notifications;
    using JanHafner.Smartbar.Views.MainWindow;
    using JetBrains.Annotations;
    using MahApps.Metro;
    using Prism.Events;

    [Export(typeof(Bootstrapper))]
    internal sealed class Bootstrapper : IDisposable
    {
        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull]
        private readonly DatabaseRepairer databaseRepairer;

        [NotNull]
        private readonly BackgroundNotifierAggregator backgroundNotifierAggregator;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [NotNull]
        private readonly ViewModelFactory viewModelFactory;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [NotNull]
        private readonly CommandLineHandler commandLineHandler;

        [ImportingConstructor]
        public Bootstrapper([NotNull] DatabaseRepairer databaseRepairer, [NotNull] BackgroundNotifierAggregator backgroundNotifierAggregator, [NotNull] ILocalizationService localizationService, [NotNull] ViewModelFactory viewModelFactory, [NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarSettings smartbarSettings,
            [NotNull] CommandLineHandler commandLineHandler)
        {
            if (databaseRepairer == null)
            {
                throw new ArgumentNullException(nameof(databaseRepairer));
            }

            if (backgroundNotifierAggregator == null)
            {
                throw new ArgumentNullException(nameof(backgroundNotifierAggregator));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            if (viewModelFactory == null)
            {
                throw new ArgumentNullException(nameof(viewModelFactory));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (commandLineHandler == null)
            {
                throw new ArgumentNullException(nameof(commandLineHandler));
            }

            this.databaseRepairer = databaseRepairer;
            this.backgroundNotifierAggregator = backgroundNotifierAggregator;
            this.localizationService = localizationService;
            this.viewModelFactory = viewModelFactory;
            this.commandLineHandler = commandLineHandler;
            this.eventAggregator = eventAggregator;
            this.smartbarSettings = smartbarSettings;

            this.subscriptionTokens = new List<SubscriptionToken>
            {
                this.eventAggregator.GetEvent<CurrentLanguageChanged>().Subscribe(this.UpdateCurrentUICulture, true),
                this.eventAggregator.GetEvent<ExceptionNotification>().Subscribe(data => Trace.WriteLine(data.ToString()))
            };

            this.ConfigureTypeResolutionFallbackWorkaround();
            this.localizationService.CurrentLanguage = this.smartbarSettings.GetCultureInfoForLanguage();
        }

        public async Task<IDisposable> BootstrapAsync([NotNull] Application application, String[] commandLineArguments)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            await this.DeleteDirectoriesWithPendingDeleteOperationsAsync();

            await this.databaseRepairer.RepairAsync();

            application.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<SmartbarSettingsUpdated>().Subscribe(updatedSmartbarSettings => this.UpdateAppearence(application), true));
            await this.ConfigureAppThemeAsync(application);
            
            var mainWindowViewModel = this.viewModelFactory.CreateMainWindowViewModel();
            application.MainWindow = new MainWindow(mainWindowViewModel);

            this.commandLineHandler.HandleCommandLine(commandLineArguments);

            application.MainWindow.Loaded += (sender, args) =>
            {
                this.backgroundNotifierAggregator.Start();
            };

            this.smartbarSettings.IsFirstStart = false;

            await this.smartbarSettings.SaveChangesAsync();

            return this;
        }

        private async Task DeleteDirectoriesWithPendingDeleteOperationsAsync()
        {
            foreach (var pendingPackageDeleteOperation in this.smartbarSettings.PendingPackageDeleteOperations.ToList())
            {
                if (Directory.Exists(pendingPackageDeleteOperation))
                {
                    Directory.Delete(pendingPackageDeleteOperation, true);
                }

                this.smartbarSettings.RemovePendingPackageDeleteOperation(pendingPackageDeleteOperation);
            }

            await this.smartbarSettings.SaveChangesAsync();
        }

        private async Task ConfigureAppThemeAsync(Application application)
        {
            try
            {
                ThemeManager.ChangeAppStyle(application, ThemeManager.GetAccent(this.smartbarSettings.AccentColorScheme), ThemeManager.GetAppTheme("BaseLight"));
            }
            catch
            {
                // Theme/Accent could not be loaded? Revert to defaults!
                this.smartbarSettings.AccentColorScheme = "Blue";

                await this.smartbarSettings.SaveChangesAsync();

                ThemeManager.ChangeAppStyle(application, ThemeManager.GetAccent(this.smartbarSettings.AccentColorScheme), ThemeManager.GetAppTheme("BaseLight"));
            }
        }

        private void ConfigureTypeResolutionFallbackWorkaround()
        {
            AppDomain.CurrentDomain.TypeResolve += MefAssemlyBindingWorkaroundResolver.CurrentDomainOnTypeResolve;
            AppDomain.CurrentDomain.AssemblyResolve += MefAssemlyBindingWorkaroundResolver.CurrentDomainOnAssemblyResolve;
        }

        private void UpdateAppearence(Application application)
        {
            ThemeManager.ChangeAppStyle(application, ThemeManager.GetAccent(this.smartbarSettings.AccentColorScheme), ThemeManager.GetAppTheme("BaseLight"));
        }

        private void UpdateCurrentUICulture(CultureInfo newCulture)
        {
            this.localizationService.CurrentLanguage = newCulture;
        }

        public void Dispose()
        {
            this.backgroundNotifierAggregator.Dispose();
            this.subscriptionTokens.UnsubscribeAll();
        }
    }
}
