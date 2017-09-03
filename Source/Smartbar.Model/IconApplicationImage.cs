namespace JanHafner.Smartbar.Model
{
    using System;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    public sealed class IconApplicationImage : ApplicationImage, ICloneable
    {
        private IconApplicationImage()
        {
        }

        public IconApplicationImage([NotNull] String file, Int32 identifier, IconIdentifierType identifierType)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            this.File = file;
            this.Identifier = identifier;
            this.IdentifierType = identifierType;
        }

        public String File { get; private set; }

        public Int32 Identifier { get; private set; }

        public IconIdentifierType IdentifierType { get; private set; }

        public override void Update(ApplicationImage applicationImage)
        {
            if (applicationImage == null)
            {
                throw new ArgumentNullException(nameof(applicationImage));
            }

            var iconApplicationImage = (IconApplicationImage)applicationImage;
            this.File = iconApplicationImage.File;
            this.Identifier = iconApplicationImage.Identifier;
            this.IdentifierType = iconApplicationImage.IdentifierType;
        }

        public Object Clone()
        {
            return new IconApplicationImage(this.File, this.Identifier, this.IdentifierType);
        }
    }
}
