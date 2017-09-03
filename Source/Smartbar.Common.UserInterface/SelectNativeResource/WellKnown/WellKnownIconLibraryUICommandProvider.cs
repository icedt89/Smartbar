namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;

    [Export(typeof(IWellKnownIconLibraryUICommandProvider))]
    internal sealed class WellKnownIconLibraryUICommandProvider : IWellKnownIconLibraryUICommandProvider
    {
        [NotNull]
        private readonly IEnumerable<IWellKnownIconLibrary> wellKnownIconLibraries;

        [NotNull]
        private readonly ILocalizationService localizationService;
        
        [ImportingConstructor]
        public WellKnownIconLibraryUICommandProvider([NotNull] [ImportMany(typeof(IWellKnownIconLibrary))] IEnumerable<IWellKnownIconLibrary> wellKnownIconLibraries,
            [NotNull] ILocalizationService localizationService)
        {
            if (wellKnownIconLibraries == null)
            {
                throw new ArgumentNullException(nameof(wellKnownIconLibraries));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            this.wellKnownIconLibraries = wellKnownIconLibraries;
            this.localizationService = localizationService;
        }

        public IEnumerable<IDynamicUICommand> GetWellKnownIconLibraries(SelectNativeResourceViewModel selectNativeResourceViewModel)
        {
            if (selectNativeResourceViewModel == null)
            {
                throw new ArgumentNullException(nameof(selectNativeResourceViewModel));
            }

            return this.wellKnownIconLibraries.Where(wkil => wkil.IsAvailable).OrderBy(wkil => wkil.GetType().Name).Select(wellKnownIconLibrary => new WellKnownIconLibraryUICommand(() => this.localizationService.Localize<Localization.WellKnownIconContainers>(wellKnownIconLibrary.DisplayTextResourceName), selectNativeResourceViewModel, wellKnownIconLibrary)).ToList();
        }
    }
}
