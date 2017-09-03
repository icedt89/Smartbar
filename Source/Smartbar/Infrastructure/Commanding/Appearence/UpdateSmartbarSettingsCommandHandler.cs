namespace JanHafner.Smartbar.Infrastructure.Commanding.Appearence
{
    using System;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    internal sealed class UpdateSmartbarSettingsCommandHandler : CommandHandler<UpdateSmartbarSettingsCommand>
    {
        [NotNull] 
        private readonly ISmartbarSettings smartbarSettings;

        [ImportingConstructor]
        public UpdateSmartbarSettingsCommandHandler(IEventAggregator eventAggregator,
            [NotNull] ISmartbarSettings smartbarSettings)
            : base(eventAggregator)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }
        
            this.smartbarSettings = smartbarSettings;
        }

        public override async Task HandleAsync(UpdateSmartbarSettingsCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            CultureInfo toCultureChanged = null;

            if (this.smartbarSettings.LanguageIdentifier != command.Language.Name)
            {
                this.smartbarSettings.SetLanguage(command.Language);
                toCultureChanged = command.Language;
            }

            this.smartbarSettings.AccentColorScheme = command.AccentColorScheme;
            this.smartbarSettings.AutoSelectCreatedGroup = command.AutoSelectCreatedGroup;
            this.smartbarSettings.Columns = command.Columns;
            this.smartbarSettings.Rows = command.Rows;
            this.smartbarSettings.DeleteWithConfirmation = command.DeleteGroupWithConfirmation;
            this.smartbarSettings.DeleteGroupWithMiddleMouseButton = command.DeleteGroupWithMiddleMouseButton;
            this.smartbarSettings.GridCellContentSize = command.GridCellContentSize;
            this.smartbarSettings.HideGroupHeaderIfOnlyOneAvailable = command.HideGroupHeaderIfOnlyOneAvailable;
            this.smartbarSettings.GridCellSpacing = command.GridCellSpacing;
            this.smartbarSettings.RestorePosition = command.RestorePosition;
            this.smartbarSettings.ShowStatusbar = command.ShowStatusbar;
            this.smartbarSettings.DirectEditOfGroupHeader = command.DirectEditOfGroupHeader;
            this.smartbarSettings.SnapOnScreenBorders = command.SnapOnScreenBorders;
            this.smartbarSettings.PinSmartbarAtPosition = command.PinSmartbarAtPosition;
            this.smartbarSettings.NotificationOnPluginUpdates = command.NotificationOnPluginUpdates;
            this.smartbarSettings.NotificationOnSmartbarUpdate = command.NotificationOnSmartbarUpdate;

            await this.smartbarSettings.SaveChangesAsync();
            
            if (toCultureChanged != null)
            {
                this.EventAggregator.GetEvent<CurrentLanguageChanged>().Publish(toCultureChanged);
            }

            this.EventAggregator.GetEvent<SmartbarSettingsUpdated>().Publish(this.smartbarSettings);

            this.PublishCommandHandlerDone(command);

            await Task.Yield();
        }
    }
}