namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.Localization;
    using MahApps.Metro.IconPacks;

    [Export(typeof(ISelectableIconPack))]
    internal sealed class SelectableIconPackFontAwesome : SelectableIconPack<PackIconFontAwesome, PackIconFontAwesomeKind>
    {
        public override String DisplayName
        {
            get { return SelectIconPackResource.IconPackFontAwesome; }
        }
    }
}