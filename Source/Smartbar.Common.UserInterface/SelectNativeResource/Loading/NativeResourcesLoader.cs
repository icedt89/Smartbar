namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.Loading
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JanHafner.Toolkit.Windows;
    using JanHafner.Toolkit.Windows.Icons;
    using JetBrains.Annotations;

    internal sealed class NativeResourcesLoader
    {
        [NotNull]
        private readonly IProgress<NativeResourcesLoaderProgress> iconExtracted;

        public NativeResourcesLoader([NotNull] IProgress<NativeResourcesLoaderProgress> iconExtracted)
        {
            if (iconExtracted == null)
            {
                throw new ArgumentNullException(nameof(iconExtracted));
            }

            this.iconExtracted = iconExtracted;
        }

        [NotNull]
        public async Task<Int32> LoadAsync([NotNull] String file, CancellationToken cancellationToken)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }
            
            var extractedIconsCount = 0;
            var extractedIcons = new List<IconImageSourceBag>();

            IconExtractor iconExtractor;
            if (IconExtractor.CouldBeIconFile(file) && IconExtractor.TryCreate(file, out iconExtractor))
            {
                using (iconExtractor)
                {
                    var icons = await iconExtractor.EnumerateIconsAsync(cancellationToken);

                    extractedIcons.AddRange(icons.Select((icon, index) => new IconImageSourceBag(icon, IconIdentifierType.Index, index))
                        .OrderByDescending(iconImageSourceBag => iconImageSourceBag.Height + iconImageSourceBag.Width));
                }
            }
            else
            {
                using (var nativeExecutable = new NativeExecutable(file))
                {
                    extractedIcons.AddRange((await nativeExecutable.ExtractIconsAsync(cancellationToken))
                        .Select(iconResourceBag => new IconImageSourceBag(iconResourceBag))
                        .OrderByDescending(iconImageSourceBag => iconImageSourceBag.Height + iconImageSourceBag.Width)
                        .ThenBy(iconImageSourceBag => iconImageSourceBag.Identifier));
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            foreach (var icon in extractedIcons)
            {
                this.iconExtracted.Report(new NativeResourcesLoaderProgress(icon, ++extractedIconsCount == extractedIcons.Count));

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }
            
            return extractedIconsCount;
        }
    }
}
