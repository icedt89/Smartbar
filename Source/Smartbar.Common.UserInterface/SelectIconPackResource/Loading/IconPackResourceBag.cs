namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.Loading
{
    using System;
    using JetBrains.Annotations;

    public sealed class IconPackResourceBag
    {
        public IconPackResourceBag([NotNull] Int32 iconPackKindKey, [NotNull] Type iconPackType)
        {
            if (iconPackType == null)
            {
                throw new ArgumentNullException(nameof(iconPackType));
            }

            this.IconPackKindKey = iconPackKindKey;
            this.IconPackType = iconPackType;
        }

        [NotNull]
        public Int32 IconPackKindKey { get; private set; }

        public Type IconPackType { get; private set; }
    }
}