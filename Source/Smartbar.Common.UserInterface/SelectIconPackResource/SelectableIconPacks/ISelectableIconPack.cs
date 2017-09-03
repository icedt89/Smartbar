namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks
{
    using System;
    using JetBrains.Annotations;

    public interface ISelectableIconPack
    {
        [NotNull]
        Type IconPackKindType { get; }

        [NotNull]
        Type IconPackType { get; }

        [NotNull]
        String DisplayName { get; }
    }
}