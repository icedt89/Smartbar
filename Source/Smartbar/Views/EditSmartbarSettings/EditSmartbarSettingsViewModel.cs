namespace JanHafner.Smartbar.Views.EditSmartbarSettings
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using global::Smartbar.Updater.Core;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using MahApps.Metro;
    using Prism.Mvvm;

    internal sealed class EditSmartbarSettingsViewModel : BindableBase
    {
        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [NotNull]
        private readonly ISmartbarService smartbarService;

        [NotNull]
        private readonly ISmartbarUpdater smartbarUpdater;

        [NotNull]
        private CultureInfo selectedLanguage;

        [CanBeNull]
        private String accentColorScheme;

        private Int32 rows;

        private Int32 columns;

        private Boolean areThereApplicationsWillbeDeletedDueToSmallerSize;

        [NotNull]
        private IEnumerable<PositionInformation> positionInformations;

        public EditSmartbarSettingsViewModel([NotNull] ISmartbarSettings smartbarSettings,
            [NotNull] IWindowService windowService, [NotNull] IUIExtensionService uiExtensionService,
            [NotNull] ILocalizationService localizationService, [NotNull] ISmartbarService smartbarService,
            [NotNull] ISmartbarUpdater smartbarUpdater)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (uiExtensionService == null)
            {
                throw new ArgumentNullException(nameof(uiExtensionService));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (smartbarUpdater == null)
            {
                throw new ArgumentNullException(nameof(smartbarUpdater));
            }

            this.smartbarSettings = smartbarSettings;
            this.windowService = windowService;
            this.localizationService = localizationService;
            this.smartbarService = smartbarService;
            this.smartbarUpdater = smartbarUpdater;

            this.AvailableAccentColorSchemes = ThemeManager.Accents.Select(accent => accent.Name).ToList();
            this.AvailableLanguages = localizationService.GetAvailableLanguages();

            this.Rows = smartbarSettings.Rows;
            this.Columns = smartbarSettings.Columns;
            this.GridCellContentSize = smartbarSettings.GridCellContentSize;
            this.GridCellSpacing = smartbarSettings.GridCellSpacing;
            this.DirectEditOfGroupHeader = smartbarSettings.DirectEditOfGroupHeader;
            this.AccentColorScheme = smartbarSettings.AccentColorScheme;
            this.DeleteWithConfirmation = smartbarSettings.DeleteWithConfirmation;
            this.DeleteGroupWithMiddleMouseButton = smartbarSettings.DeleteGroupWithMiddleMouseButton;
            this.ShowStatusbar = smartbarSettings.ShowStatusbar;
            this.AutoSelectCreatedGroup = smartbarSettings.AutoSelectCreatedGroup;
            this.HideGroupHeaderIfOnlyOneAvailable = smartbarSettings.HideGroupHeaderIfOnlyOneAvailable;
            this.RestorePosition = smartbarSettings.RestorePosition;
            this.SnapOnScreenBorders = smartbarSettings.SnapOnScreenBorders;
            this.SelectedLanguage = smartbarSettings.GetCultureInfoForLanguage();
            this.NotificationOnPluginUpdates = smartbarSettings.NotificationOnPluginUpdates;
            this.NotificationOnSmartbarUpdate = smartbarSettings.NotificationOnSmartbarUpdate;
            this.PinSmartbarAtPosition = smartbarSettings.PinSmartbarAtPosition;
            this.PluginConfigurationUICommands = uiExtensionService.GetConfigurablePluginsUICommands(() => true).ToList();
        }

        [NotNull]
        public IEnumerable<CultureInfo> AvailableLanguages { get; set; }

        public Boolean LanguageSelectionAvailable
        {
            get
            {
                return this.AvailableLanguages.Count() > 1;
            }
        }

        [NotNull]
        public CultureInfo SelectedLanguage
        {
            get
            {
                return this.selectedLanguage;
            }
            set
            {
                if (this.SetProperty(ref this.selectedLanguage, value))
                {
                    this.localizationService.CurrentLanguage = value;
                }
            }
        }

        [NotNull]
        public IEnumerable<String> AvailableAccentColorSchemes { get; set; }

        public Int32 Rows
        {
            get { return this.rows; }
            set
            {
                if(this.SetProperty(ref this.rows, value) && this.columns > 0)
                {
                    this.positionInformations = this.smartbarService.GetOutOfRangeApplicationPositions(this.columns - 1, this.rows - 1);
                    this.AreThereApplicationsThatWillBeDeletedDueToSmallerSize = this.ApplicationsWhichWillBeDeleted.Any();
                }
            }
        }

        public Int32 Columns
        {
            get { return this.columns; }
            set
            {
                if (this.SetProperty(ref this.columns, value) && this.rows > 0)
                {
                    this.positionInformations = this.smartbarService.GetOutOfRangeApplicationPositions(this.columns - 1, this.rows - 1);
                    this.AreThereApplicationsThatWillBeDeletedDueToSmallerSize = this.ApplicationsWhichWillBeDeleted.Any();
                }
            }
        }

        public Boolean NotificationOnSmartbarUpdate { get; set; }

        public Boolean NotificationOnPluginUpdates { get; set; }

        public Boolean PinSmartbarAtPosition { get; set; }

        public Int32 GridCellSpacing { get; set; }

        public Int32 GridCellContentSize { get; set; }

        public Boolean SnapOnScreenBorders { get; set; }

        [NotNull]
        public String AccentColorScheme
        {
            get { return this.accentColorScheme; }
            set
            {
                if (this.SetProperty(ref this.accentColorScheme, value))
                {
                    ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(value), ThemeManager.GetAppTheme("BaseLight"));
                }
            }
        }

        public Boolean DeleteWithConfirmation { get; set; }

        public Boolean DeleteGroupWithMiddleMouseButton { get; set; }

        public Boolean ShowStatusbar { get; set; }

        public Boolean DirectEditOfGroupHeader { get; set; }

        public Boolean AutoSelectCreatedGroup { get; set; }

        public Boolean HideGroupHeaderIfOnlyOneAvailable { get; set; }

        public Boolean RestorePosition { get; set; }

        public Boolean HasNoConfigurablePlugins
        {
            get { return !this.PluginConfigurationUICommands.Any(); }
        }

        [NotNull]
        public IEnumerable<IDynamicUICommand> PluginConfigurationUICommands { get; private set; }
            
        [NotNull]
        public ICommand CloseCommand
        {
            get { return new CommonOKCommand<EditSmartbarSettingsViewModel>(this, this.windowService); }
        }

        [NotNull]
        public ICommand OpenAboutDialogCommand
        {
            get { return new EditSmartbarSettingsOpenAboutDialogCommand(this.smartbarUpdater, this.windowService, this.smartbarSettings); }
        }

        public Boolean AreThereApplicationsThatWillBeDeletedDueToSmallerSize
        {
            get { return this.areThereApplicationsWillbeDeletedDueToSmallerSize; }
            private set
            {
                this.SetProperty(ref this.areThereApplicationsWillbeDeletedDueToSmallerSize, value);
            }
        }

        [NotNull]
        public IEnumerable<Guid> ApplicationsWhichWillBeDeleted
        {
            get { return this.positionInformations.Where(a => !a.IsFree).Select(a => a.AssignedApplicationId.Value); }
        } 
    }
}