namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public interface ISelectableIconPacksProvider
    {
        [NotNull]
        IEnumerable<ISelectableIconPack> GetSelectableIconPacks();
    }
}