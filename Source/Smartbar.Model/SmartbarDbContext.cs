namespace JanHafner.Smartbar.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Common;
    using JetBrains.Annotations;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [Export(typeof(ISmartbarDbContext))]
    internal sealed class SmartbarDbContext : JsonFileAdapter, ISmartbarDbContext
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
            Binder = new UnresolvedTypeBinder(),
            ContractResolver = new PrivateSettersEnabledContractResolver(),
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Full
        };

        [NotNull]
        private readonly Database database;

        [ImportingConstructor]
        public SmartbarDbContext([NotNull] ISmartbarSettings smartbarSettings)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            this.file = smartbarSettings.CreateAndGetDatabaseFile();

            this.database = new Database();
            if (File.Exists(this.file))
            {
                var content = File.ReadAllText(this.file);

                this.database = JsonConvert.DeserializeObject<Database>(content, SmartbarDbContext.JsonSerializerSettings);
            }
        }

        public ICollection<Group> Groups
        {
            get
            { 
                return this.database.Groups;
            }
        }

        public ICollection<PluginConfiguration> PluginConfigurations
        {
            get
            {
                return this.database.PluginConfigurations;
            }
        }

        public Task SaveChangesAsync()
        {
            var content = JsonConvert.SerializeObject(this.database, Formatting.None, SmartbarDbContext.JsonSerializerSettings);

            return this.SaveFileToDiskSafeAsync(this.file, content);
        }

        private sealed class PrivateSettersEnabledContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var result = base.CreateProperty(member, memberSerialization);
                result.Writable = (member as PropertyInfo)?.GetSetMethod(true) != null;

                return result;
            }
        }

        private sealed class Database
        {
            public Database()
            {
                this.Groups = new List<Group>();
                this.PluginConfigurations = new List<PluginConfiguration>();
            }

            public ICollection<Group> Groups { get; private set; }

            public ICollection<PluginConfiguration> PluginConfigurations { get; private set; }
        }
    }
}
