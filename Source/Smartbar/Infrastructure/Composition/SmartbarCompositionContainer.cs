namespace JanHafner.Smartbar.Infrastructure.Composition
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.ReflectionModel;
    using System.Linq;
    using JetBrains.Annotations;

    internal sealed class SmartbarCompositionContainer : CompositionContainer, IModuleExplorer
    {
        public SmartbarCompositionContainer([NotNull] String basePluginsDirectory)
            : base(new SmartbarCatalog(basePluginsDirectory), CompositionOptions.DisableSilentRejection | CompositionOptions.IsThreadSafe)
        {
        }

        public IEnumerable<Type> GetImportedTypes()
        {
            return this.Catalog.Select(_ => ReflectionModelServices.GetPartType(_).Value).ToList();
        }
    }
}
