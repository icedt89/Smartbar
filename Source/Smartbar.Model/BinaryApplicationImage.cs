namespace JanHafner.Smartbar.Model
{
    using System;
    using JetBrains.Annotations;

    public sealed class BinaryApplicationImage : ApplicationImage, ICloneable
    {
        private BinaryApplicationImage()
        {
        }

        public BinaryApplicationImage([NotNull] Byte[] imageBlob)
        {
            if (imageBlob == null || imageBlob.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageBlob));
            }

            this.ImageBlob = imageBlob;
        }

        public Byte[] ImageBlob { get; private set; }

        public override void Update(ApplicationImage applicationImage)
        {
            if (applicationImage == null)
            {
                throw new ArgumentNullException(nameof(applicationImage));
            }

            var iconApplicationImage = (BinaryApplicationImage)applicationImage;
            this.ImageBlob = iconApplicationImage.ImageBlob;
        }

        public Object Clone()
        {
            return new BinaryApplicationImage(this.ImageBlob);
        }
    }
}
