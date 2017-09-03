namespace JanHafner.Smartbar.Infrastructure.Composition
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using JetBrains.Annotations;

    internal sealed class RecursiveDirectoryCatalog : AggregateCatalog
    {
        public RecursiveDirectoryCatalog(String directory)
        {
            if (String.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.Catalogs.Add(this.CreateDirectoryCatalog(directory));
            foreach (var subDirectory in Directory.EnumerateDirectories(directory, "*", SearchOption.AllDirectories))
            {
                this.Catalogs.Add(this.CreateDirectoryCatalog(subDirectory));
            }
        }

        private DirectoryCatalog CreateDirectoryCatalog([NotNull] String directory)
        {
            if (String.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var filePattern = "*Smartbar*Plugins.*.dll";

            return new DirectoryCatalog(directory, filePattern);
        }
    }
}
