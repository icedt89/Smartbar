namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Threading.Tasks;
    using System.Windows;
    using JetBrains.Annotations;
    using Newtonsoft.Json;

    [Export(typeof(ISmartbarSettings))]
    internal sealed class SmartbarSettings : JsonFileAdapter, ISmartbarSettings
    {
        [NotNull]
        private readonly String file;

        [NotNull]
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            DefaultValueHandling = DefaultValueHandling.Populate,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
            Formatting = Formatting.Indented
        };

        [NotNull]
        private readonly SerializableSmartbarSettings serializableSmartbarSettings;

        [ImportingConstructor]
        public SmartbarSettings()
        {
            this.file = SmartbarSettingsExtensions.CreateAndGetSettingsFile();

            this.serializableSmartbarSettings = new SerializableSmartbarSettings();
            if (File.Exists(this.file))
            {
                var content = File.ReadAllText(this.file);

                this.serializableSmartbarSettings = JsonConvert.DeserializeObject<SerializableSmartbarSettings>(content, SmartbarSettings.JsonSerializerSettings);
            }
        }

        public Task SaveChangesAsync()
        {
            var content = JsonConvert.SerializeObject(this.serializableSmartbarSettings, Formatting.Indented, SmartbarSettings.JsonSerializerSettings);

            return this.SaveFileToDiskSafeAsync(this.file, content);
        }

        public String SmartbarUpdaterExePath
        {
            get { return Path.GetFullPath(@".\Updater\Smartbar.Updater.exe"); }
        }

        public String AccentColorScheme
        {
            get { return this.serializableSmartbarSettings.AccentColorScheme; }
            set { this.serializableSmartbarSettings.AccentColorScheme = value; }
        }

        public Boolean AutoSelectCreatedGroup
        {
            get { return this.serializableSmartbarSettings.AutoSelectCreatedGroup; }
            set { this.serializableSmartbarSettings.AutoSelectCreatedGroup = value; }
        }

        public String BasePluginPackagesDirectoryName
        {
            get { return this.serializableSmartbarSettings.BasePluginPackagesDirectoryName; }
        }

        public Boolean PinSmartbarAtPosition
        {
            get { return this.serializableSmartbarSettings.PinSmartbarAtPosition; }
            set { this.serializableSmartbarSettings.PinSmartbarAtPosition = value; }
        }

        public Boolean IsModuleExplorerAvailable
        {
	        get { return this.serializableSmartbarSettings.IsModuleExplorerAvailable; }
	        set { this.serializableSmartbarSettings.IsModuleExplorerAvailable = value; }
        }

		public String BasePluginsDirectoryName
        {
            get { return this.serializableSmartbarSettings.BasePluginsDirectoryName; }
        }

        public Int32 Columns
        {
            get { return this.serializableSmartbarSettings.Columns; }
            set { this.serializableSmartbarSettings.Columns = value; }
        }

        public String DatabaseFileName
        {
            get { return this.serializableSmartbarSettings.DatabaseFileName; }
        }

        public String DefaultDummyGroupName
        {
            get { return this.serializableSmartbarSettings.DefaultDummyGroupName; }
        }

        public String DefaultLanguageIdentifier
        {
            get { return this.serializableSmartbarSettings.DefaultLanguageIdentifier; }
        }

        public Boolean DeleteGroupWithMiddleMouseButton
        {
            get { return this.serializableSmartbarSettings.DeleteGroupWithMiddleMouseButton; }
            set { this.serializableSmartbarSettings.DeleteGroupWithMiddleMouseButton = value; }
        }

        public Boolean IsFirstStart
        {
            get { return this.serializableSmartbarSettings.IsFirstStart; }
            set { this.serializableSmartbarSettings.IsFirstStart = value; }
        }

        public Boolean DeleteWithConfirmation
        {
            get { return this.serializableSmartbarSettings.DeleteWithConfirmation; }
            set { this.serializableSmartbarSettings.DeleteWithConfirmation = value; }
        }

        public Boolean DirectEditOfGroupHeader
        {
            get { return this.serializableSmartbarSettings.DirectEditOfGroupHeader; }
            set { this.serializableSmartbarSettings.DirectEditOfGroupHeader = value; }
        }

        public Int32 GridCellContentSize
        {
            get { return this.serializableSmartbarSettings.GridCellContentSize; }
            set { this.serializableSmartbarSettings.GridCellContentSize = value; }
        }

        public Int32 GridCellSpacing
        {
            get { return this.serializableSmartbarSettings.GridCellSpacing; }
            set { this.serializableSmartbarSettings.GridCellSpacing = value; }
        }

        public Boolean HideGroupHeaderIfOnlyOneAvailable
        {
            get { return this.serializableSmartbarSettings.HideGroupHeaderIfOnlyOneAvailable; }
            set { this.serializableSmartbarSettings.HideGroupHeaderIfOnlyOneAvailable = value; }
        }

        public String LanguageIdentifier
        {
            get { return this.serializableSmartbarSettings.LanguageIdentifier; }
            set { this.serializableSmartbarSettings.LanguageIdentifier = value; }
        }

        public IEnumerable<String> PendingPackageDeleteOperations
        {
            get { return this.serializableSmartbarSettings.PendingPackageDeleteOperations; }
        }

        public String PluginPackagesFeed
        {
            get { return this.serializableSmartbarSettings.PluginPackagesFeed; }
            set { this.serializableSmartbarSettings.PluginPackagesFeed = value; }
        }

        public Boolean RestorePosition
        {
            get { return this.serializableSmartbarSettings.RestorePosition; }
            set { this.serializableSmartbarSettings.RestorePosition = value; }
        }

        public Int32 Rows
        {
            get { return this.serializableSmartbarSettings.Rows; }
            set { this.serializableSmartbarSettings.Rows = value; }
        }

        public Boolean ShowStatusbar
        {
            get { return this.serializableSmartbarSettings.ShowStatusbar; }
            set { this.serializableSmartbarSettings.ShowStatusbar = value; }
        }

        public Int32 SnapOnScreenBordersOffset
        {
            get { return this.serializableSmartbarSettings.SnapOnScreenBordersOffset; }
        }

        public Boolean SnapOnScreenBorders
        {
            get { return this.serializableSmartbarSettings.SnapOnScreenBorders; }
            set { this.serializableSmartbarSettings.SnapOnScreenBorders = value; }
        }

        public Boolean NotificationOnSmartbarUpdate
        {
            get { return this.serializableSmartbarSettings.NotificationOnSmartbarUpdate; }
            set { this.serializableSmartbarSettings.NotificationOnSmartbarUpdate = value; }
        }

        public Boolean NotificationOnPluginUpdates
        {
            get { return this.serializableSmartbarSettings.NotificationOnPluginUpdates; }
            set { this.serializableSmartbarSettings.NotificationOnPluginUpdates = value; }
        }

        public Point InitialPosition
        {
            get { return this.serializableSmartbarSettings.InitialPosition; }
            set { this.serializableSmartbarSettings.InitialPosition = value; }
        }

        public void AddPendingPackageDeleteOperation(String packagePath)
        {
            this.serializableSmartbarSettings.PendingPackageDeleteOperations.Add(packagePath);
        }

        public void RemovePendingPackageDeleteOperation(String packagePath)
        {
            this.serializableSmartbarSettings.PendingPackageDeleteOperations.Remove(packagePath);
        }

        private sealed class SerializableSmartbarSettings
        {
            public SerializableSmartbarSettings()
            {
                this.DeleteWithConfirmation = true;
                this.DeleteGroupWithMiddleMouseButton = true;
                this.ShowStatusbar = true;
                this.AutoSelectCreatedGroup = true;
                this.HideGroupHeaderIfOnlyOneAvailable = true;
                this.RestorePosition = true;
                this.Rows = 1;
                this.Columns = 8;
                this.GridCellContentSize = 1;
                this.GridCellContentSize = 48;
                this.AccentColorScheme = "Blue";
                this.LanguageIdentifier = "EN";
                this.DefaultLanguageIdentifier = "EN";
                this.BasePluginPackagesDirectoryName = "Packages";
                this.BasePluginsDirectoryName = "Plugins";
                this.DatabaseFileName = "smartbar.json";
                this.PendingPackageDeleteOperations = new List<String>();
                this.DefaultDummyGroupName = "Applications";
                this.SnapOnScreenBorders = true;
                this.SnapOnScreenBordersOffset = 10;
                this.NotificationOnPluginUpdates = true;
                this.NotificationOnSmartbarUpdate = true;
                this.InitialPosition = new Point(0, 0);
	            this.IsModuleExplorerAvailable = true;
	            this.PluginPackagesFeed = $"http://{Guid.NewGuid()}.dummy.org";
            }

            public Boolean PinSmartbarAtPosition { get; set; }

	        public Boolean IsModuleExplorerAvailable { get; set; }

			public String AccentColorScheme { get; set; }

            public Boolean AutoSelectCreatedGroup { get; set; }

            public String BasePluginPackagesDirectoryName { get; set; }

            public String BasePluginsDirectoryName { get; set; }

            public Int32 Columns { get; set; }

            public String DatabaseFileName { get; set; }

            public String DefaultDummyGroupName { get; set; }

            public String DefaultLanguageIdentifier { get; set; }

            public Boolean DeleteGroupWithMiddleMouseButton { get; set; }

            public Boolean DeleteWithConfirmation { get; set; }

            public Boolean DirectEditOfGroupHeader { get; set; }

            public Int32 GridCellContentSize { get; set; }

            public Int32 GridCellSpacing { get; set; }

            public Boolean HideGroupHeaderIfOnlyOneAvailable { get; set; }

            public Point InitialPosition { get; set; }

            public Boolean IsFirstStart { get; set; }

            public String LanguageIdentifier { get; set; }

            public Boolean NotificationOnPluginUpdates { get; set; }

            public Boolean NotificationOnSmartbarUpdate { get; set; }

            public IList<String> PendingPackageDeleteOperations { get; set; }

            public String PluginPackagesFeed { get; set; }

            public Boolean RestorePosition { get; set; }

            public Int32 Rows { get; set; }

            public Boolean ShowStatusbar { get; set; }

            public Boolean SnapOnScreenBorders { get; set; }

            public Int32 SnapOnScreenBordersOffset { get; set; }
        }
    }
}
