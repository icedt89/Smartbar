namespace JanHafner.Smartbar.Views.MainWindow
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Common;
    using global::Smartbar.Updater.Core;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Infrastructure.Commanding.Appearence;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.EditSmartbarSettings;
    using Prism.Commands;
    using EditSmartbarSettings = JanHafner.Smartbar.Views.EditSmartbarSettings.EditSmartbarSettings;

    internal sealed class MainWindowViewModelEditSmartbarSettingsCommand : DelegateCommand
    {
        public MainWindowViewModelEditSmartbarSettingsCommand(ISmartbarSettings smartbarSettings, ICommandDispatcher commandDispatcher, IWindowService windowService, IUIExtensionService uiExtensionService, ILocalizationService localizationService, ISmartbarService smartbarService, ISmartbarUpdater smartbarUpdater)
            : base(async () =>
            {
                var editSmartbarSettingsViewModel = new EditSmartbarSettingsViewModel(smartbarSettings, windowService, uiExtensionService, localizationService, smartbarService, smartbarUpdater);
                if (await windowService.ShowWindowAsync<EditSmartbarSettings>(editSmartbarSettingsViewModel) != MessageBoxResult.OK)
                {
                    return;
                }

                var commands = new List<ICommand>();
                if (editSmartbarSettingsViewModel.AreThereApplicationsThatWillBeDeletedDueToSmallerSize)
                {
                    commands.AddRange(editSmartbarSettingsViewModel.ApplicationsWhichWillBeDeleted.Select(applicationIdWhichWillbeDeleted => new DeleteApplicationCommand(applicationIdWhichWillbeDeleted)));
                }

                commands.Add(new UpdateSmartbarSettingsCommand(editSmartbarSettingsViewModel.Rows,
                    editSmartbarSettingsViewModel.Columns, editSmartbarSettingsViewModel.GridCellSpacing,
                    editSmartbarSettingsViewModel.GridCellContentSize,
                    editSmartbarSettingsViewModel.AccentColorScheme,
                    editSmartbarSettingsViewModel.SelectedLanguage,
                    editSmartbarSettingsViewModel.DeleteWithConfirmation,
                    editSmartbarSettingsViewModel.DeleteGroupWithMiddleMouseButton,
                    editSmartbarSettingsViewModel.ShowStatusbar,
                    editSmartbarSettingsViewModel.AutoSelectCreatedGroup,
                    editSmartbarSettingsViewModel.HideGroupHeaderIfOnlyOneAvailable,
                    editSmartbarSettingsViewModel.RestorePosition,
                    editSmartbarSettingsViewModel.DirectEditOfGroupHeader,
                    editSmartbarSettingsViewModel.SnapOnScreenBorders,
                    editSmartbarSettingsViewModel.NotificationOnPluginUpdates,
                    editSmartbarSettingsViewModel.NotificationOnSmartbarUpdate,
                    editSmartbarSettingsViewModel.PinSmartbarAtPosition));

                await commandDispatcher.DispatchAsync(commands);
            })
        {
        }
    }
}
