namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.Loading
{
    using System;
    using System.Drawing;
    using System.Windows.Media;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    public sealed class IconImageSourceBag
    {
        private readonly Lazy<ImageSource> imageSource;

        public IconImageSourceBag([NotNull] IconResourceBag iconResourceBag)
        {
            if (iconResourceBag == null)
            {
                throw new ArgumentNullException(nameof(iconResourceBag));
            }

            this.Height = iconResourceBag.Icon.Height;
            this.Width = iconResourceBag.Icon.Width;
            this.IconIdentifierType = iconResourceBag.IdentifierType;
            this.Identifier = iconResourceBag.Identifier;
            this.imageSource = new Lazy<ImageSource>(() => iconResourceBag.Icon.ToImageSource());
        }

        public IconImageSourceBag([NotNull] Icon icon, IconIdentifierType identifierType, Int32 identifier)
        {
            if (icon == null)
            {
                throw new ArgumentNullException(nameof(icon));
            }

            this.Width = icon.Width;
            this.Height = icon.Height;
            this.IconIdentifierType = identifierType;
            this.Identifier = identifier;
            this.imageSource = new Lazy<ImageSource>(() => icon.ToImageSource());
        }

        public Int32 Height { get; private set; }

        public Int32 Width { get; private set; }

        [NotNull]
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource.Value;
            }
        }

        public IconIdentifierType IconIdentifierType { get; private set; }

        public Int32 Identifier { get; private set; }
    }
}
