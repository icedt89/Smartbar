namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.Loading
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    internal sealed class SelectIconPackResourcesLoader
    {
        [NotNull]
        private readonly IProgress<SelectIconPackResourcesLoaderProgress> iconExtracted;

        public SelectIconPackResourcesLoader([NotNull] IProgress<SelectIconPackResourcesLoaderProgress> iconExtracted)
        {
            if (iconExtracted == null)
            {
                throw new ArgumentNullException(nameof(iconExtracted));
            }

            this.iconExtracted = iconExtracted;
        }

        [NotNull]
        public Task<Int32> Load([NotNull] Type iconPackType, Type iconPackKindType, CancellationToken cancellationToken)
        {
            if (iconPackType == null)
            {
                throw new ArgumentNullException(nameof(iconPackType));
            }

            if (iconPackKindType == null)
            {
                throw new ArgumentNullException(nameof(iconPackKindType));
            }

            var extractedIconsCount = 0;
            var extractedIcons = (iconPackKindType.GetEnumValues().Cast<Int32>()).Where(enumValue => enumValue > 0).Select(enumValue => new IconPackResourceBag(enumValue, iconPackType)).ToList();
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var iconPackResourceBag in extractedIcons)
            {
                this.iconExtracted.Report(new SelectIconPackResourcesLoaderProgress(iconPackResourceBag, ++extractedIconsCount == extractedIcons.Count));

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            return Task.FromResult(extractedIconsCount);
        }
    }
}
