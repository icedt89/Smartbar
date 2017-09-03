namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using JanHafner.Smartbar.Model;

    [Export(typeof(IImageVisualizationHandler))]
    public class BinaryImageVisualizationApplicationHandler : IImageVisualizationHandler
    {
        public virtual Boolean CanVisualize(ApplicationImage applicationImage)
        {
            return applicationImage is BinaryApplicationImage;
        }

        public virtual ImageSource Visualize(ApplicationImage applicationImage, Size desiredSize)
        {
            var binaryApplicationImage = applicationImage as BinaryApplicationImage;
            if (binaryApplicationImage == null || binaryApplicationImage.ImageBlob.Length == 0)
            {
                throw new ArgumentException("Invalid argument supplied.", nameof(applicationImage));
            }

            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(binaryApplicationImage.ImageBlob, false);
            image.EndInit();
            image.Freeze();

            return image;
        }
    }
}
