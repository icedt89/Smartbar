namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks
{
    using System;
    using ControlzEx;

    internal abstract class SelectableIconPack<TIconPack, TIconPackKind> : ISelectableIconPack
        where TIconPack : PackIconBase<TIconPackKind>
    {
        public Type IconPackKindType
        {
            get { return typeof (TIconPackKind); }
        }

        public Type IconPackType
        {
            get { return typeof (TIconPack); }
        }
        
        public abstract String DisplayName { get; }
    }
}
