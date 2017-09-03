namespace JanHafner.Smartbar.Infrastructure.Composition
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.ComponentModel.Composition.Registration;
    using System.IO;
    using System.Linq;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Events;

    internal sealed class SmartbarCatalog : ComposablePartCatalog
    {
        [NotNull]
        private readonly String basePluginsDirectory;

        [NotNull]
        private readonly AggregateCatalog aggregateCatalog;

        public SmartbarCatalog([NotNull] String basePluginsDirectory)
        {
            if (String.IsNullOrWhiteSpace(basePluginsDirectory))
            {
                throw new ArgumentNullException(nameof(basePluginsDirectory));
            }

            this.basePluginsDirectory = basePluginsDirectory;
            this.aggregateCatalog = new AggregateCatalog();
            
            this.Initialize();
        }

        [NotNull]
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return this.aggregateCatalog.Parts; }
        }

        public override IEnumerator<ComposablePartDefinition> GetEnumerator()
        {
            return this.aggregateCatalog.GetEnumerator();
        }

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            return this.aggregateCatalog.GetExports(definition).Distinct(n => n.Item1.ToString());
        }

        private void Initialize()
        {
            this.RegisterThirdPartyTypes();
            this.RegisterApplicationProvidedTypes();
            this.RegisterTypesFromPlugins();
        }

        private void RegisterApplicationProvidedTypes()
        {
            this.aggregateCatalog.Catalogs.Add(new ApplicationCatalog());
        }

        /// <summary>
        /// Registers third party types which have no <see cref="ExportAttribute"/>.
        /// </summary>
        private void RegisterThirdPartyTypes()
        {
            var registrationBuilder = new RegistrationBuilder();

            registrationBuilder.ForType<EventAggregator>().Export<IEventAggregator>().SetCreationPolicy(CreationPolicy.Shared);
            registrationBuilder.ForType<WpfHotKeyManager>().Export<WpfHotKeyManager>().SetCreationPolicy(CreationPolicy.Shared).SelectConstructor(constructorInfos => constructorInfos.Single(constructorInfo => constructorInfo.GetParameters().Length == 0));
         
            this.aggregateCatalog.Catalogs.Add(new TypeCatalog(new[]
            {
                typeof (EventAggregator),
                typeof(WpfHotKeyManager)
            }, registrationBuilder));
        }

        /// <summary>
        /// Registers types from the configured plugin directories.
        /// </summary>
        private void RegisterTypesFromPlugins()
        {
            var fullPluginPath = Path.GetFullPath(this.basePluginsDirectory);

            this.aggregateCatalog.Catalogs.Add(new RecursiveDirectoryCatalog(fullPluginPath));
        }

        protected override void Dispose(Boolean disposing)
        {
            this.aggregateCatalog.Catalogs.ForEach(catalog => catalog.Dispose());
            this.aggregateCatalog.Dispose();

            base.Dispose(disposing);
        }
    }
}
