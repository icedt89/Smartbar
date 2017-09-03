namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using JetBrains.Annotations;

    [Export(typeof(ISelectableIconPacksProvider))]
    internal sealed class SelectableIconPacksProvider : ISelectableIconPacksProvider
    {
        [NotNull]
        private readonly IEnumerable<ISelectableIconPack> selectableIconPacks;

        [ImportingConstructor]
        public SelectableIconPacksProvider([NotNull, ImportMany(typeof(ISelectableIconPack))] IEnumerable<ISelectableIconPack> selectableIconPacks)
        {
            this.selectableIconPacks = selectableIconPacks;
        }

        public IEnumerable<ISelectableIconPack> GetSelectableIconPacks()
        {
            return this.selectableIconPacks;
        }
    }
}