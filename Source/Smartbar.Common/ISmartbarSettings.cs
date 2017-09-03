namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;

    public interface ISmartbarSettings
    {
        String SmartbarUpdaterExePath { get; }

        String AccentColorScheme { get; set; }

        Boolean AutoSelectCreatedGroup { get; set; }

        String BasePluginPackagesDirectoryName { get; }

        String BasePluginsDirectoryName { get; }

        Int32 Columns { get; set; }

        String DatabaseFileName { get; }

        String DefaultDummyGroupName { get; }

        String DefaultLanguageIdentifier { get; }

        Boolean DeleteGroupWithMiddleMouseButton { get; set; }

        Boolean IsFirstStart { get; set; }

        Boolean DeleteWithConfirmation { get; set; }

        Boolean DirectEditOfGroupHeader { get; set; }

        Int32 GridCellContentSize { get; set; }

        Int32 GridCellSpacing { get; set; }

        Boolean HideGroupHeaderIfOnlyOneAvailable { get; set; }

        String LanguageIdentifier { get; set; }

        IEnumerable<String> PendingPackageDeleteOperations { get; }

        String PluginPackagesFeed { get; }

        Boolean RestorePosition { get; set; }

        Int32 Rows { get; set; }

        Boolean ShowStatusbar { get; set; }

        Int32 SnapOnScreenBordersOffset { get; }

        Boolean SnapOnScreenBorders { get; set; }

        Boolean NotificationOnSmartbarUpdate { get; set; }

        Boolean NotificationOnPluginUpdates { get; set; }

        Point InitialPosition { get; set; }

        Boolean PinSmartbarAtPosition { get; set; }

        Boolean IsModuleExplorerAvailable { get; set; }

        void AddPendingPackageDeleteOperation(String packagePath);

        void RemovePendingPackageDeleteOperation(String packagePath);

        Task SaveChangesAsync();
    }
}