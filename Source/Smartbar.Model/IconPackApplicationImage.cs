namespace JanHafner.Smartbar.Model
{
    using System;
    using JetBrains.Annotations;

    public sealed class IconPackApplicationImage : ApplicationImage, ICloneable
    {
        private IconPackApplicationImage()
        {
        }

        public IconPackApplicationImage([NotNull] Type iconPackType, [NotNull] Byte[] fillColor,
            [NotNull] Int32 iconPackKindKey)
        {
            if (iconPackType == null)
            {
                throw new ArgumentNullException(nameof(iconPackType));
            }

            if (fillColor == null || fillColor.Length == 0)
            {
                throw new ArgumentNullException(nameof(fillColor));
            }


            this.IconPackType = iconPackType;
            this.FillColor = fillColor;
            this.IconPackKindKey = iconPackKindKey;
        }

        [NotNull]
        public Type IconPackType { get; private set; }

        [NotNull]
        public Byte[] FillColor { get; private set; }

        public Int32 IconPackKindKey { get; private set; }

        public override void Update(ApplicationImage applicationImage)
        {
            if (applicationImage == null)
            {
                throw new ArgumentNullException(nameof(applicationImage));
            }

            var canvasApplicationImage = (IconPackApplicationImage)applicationImage;
            this.IconPackType = canvasApplicationImage.IconPackType;
            this.FillColor = canvasApplicationImage.FillColor;
            this.IconPackKindKey = canvasApplicationImage.IconPackKindKey;
        }

        public Object Clone()
        {
            return new IconPackApplicationImage(this.IconPackType, this.FillColor, this.IconPackKindKey);
        }
    }
}
