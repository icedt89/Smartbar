namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System.Collections.Generic;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JetBrains.Annotations;

    public interface IWellKnownIconLibraryUICommandProvider
    {
        [NotNull]
        IEnumerable<IDynamicUICommand> GetWellKnownIconLibraries([NotNull] SelectNativeResourceViewModel selectNativeResourceViewModel);
    }
}