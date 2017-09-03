namespace JanHafner.Smartbar.Infrastructure.Commanding.Appearence
{
    using System;
    using System.Globalization;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    internal sealed class UpdateSmartbarSettingsCommand : ICommand
    {
        public UpdateSmartbarSettingsCommand(Int32 rows, Int32 columns, Int32 gridCellSpacing, Int32 gridCellContentSize, String accentColorScheme, CultureInfo language, Boolean deleteGroupWithConfirmation, Boolean deleteGroupWithMiddleMouseButton, Boolean showStatusbar, Boolean autoSelectCreatedGroup, Boolean hideGroupHeaderIfOnlyOneAvailable, Boolean restorePosition, Boolean directEditOfGroupHeader, Boolean snapOnScreenBorders, Boolean notificationOnPluginUpdates, Boolean notificationOnSmartbarUpdate, Boolean pinSmartbarAtPosition)
        {
            if (language == null)
            {
                throw new ArgumentNullException(nameof(language));
            }

            if (String.IsNullOrWhiteSpace(accentColorScheme))
            {
                throw new ArgumentNullException(nameof(accentColorScheme));
            }

            this.Rows = rows;
            this.Columns = columns;
            this.GridCellSpacing = gridCellSpacing;
            this.GridCellContentSize = gridCellContentSize;
            this.AccentColorScheme = accentColorScheme;
            this.Language = language;
            this.DeleteGroupWithConfirmation = deleteGroupWithConfirmation;
            this.DeleteGroupWithMiddleMouseButton = deleteGroupWithMiddleMouseButton;
            this.ShowStatusbar = showStatusbar;
            this.AutoSelectCreatedGroup = autoSelectCreatedGroup;
            this.HideGroupHeaderIfOnlyOneAvailable = hideGroupHeaderIfOnlyOneAvailable;
            this.RestorePosition = restorePosition;
            this.DirectEditOfGroupHeader = directEditOfGroupHeader;
            this.SnapOnScreenBorders = snapOnScreenBorders;
            this.NotificationOnPluginUpdates = notificationOnPluginUpdates;
            this.NotificationOnSmartbarUpdate = notificationOnSmartbarUpdate;
            this.PinSmartbarAtPosition = pinSmartbarAtPosition;
        }

        public Int32 Rows { get; private set; }

        public Int32 Columns { get; private set; }

        public Int32 GridCellSpacing { get; private set; }

        public Boolean SnapOnScreenBorders { get; private set; }

        public Boolean NotificationOnPluginUpdates { get; private set; }

        public Boolean NotificationOnSmartbarUpdate { get; private set; }

        public Boolean PinSmartbarAtPosition { get; private set; }

        public Int32 GridCellContentSize { get; private set; }

        [NotNull]
        public String AccentColorScheme { get; private set; }

        [NotNull]
        public CultureInfo Language { get; private set; }

        public Boolean DeleteGroupWithConfirmation { get; private set; }

        public Boolean DeleteGroupWithMiddleMouseButton { get; private set; }

        public Boolean ShowStatusbar { get; private set; }

        public Boolean AutoSelectCreatedGroup { get; private set; }

        public Boolean HideGroupHeaderIfOnlyOneAvailable { get; private set; }

        public Boolean RestorePosition { get; private set; }

        public Boolean DirectEditOfGroupHeader { get; private set; }
    }
}
