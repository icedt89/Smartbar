namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Net.Cache;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IImageVisualizationHandler))]
    public class FileApplicationImageVisualizationHandler : IImageVisualizationHandler
    {
        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public FileApplicationImageVisualizationHandler([NotNull] IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.eventAggregator = eventAggregator;
        }

        public virtual Boolean CanVisualize(ApplicationImage applicationImage)
        {
            return applicationImage is FileApplicationImage;
        }

        /// <summary>
        /// This implementation ignores the <param name="desiredSize" /> parameter and uses the actual resource size from the file.
        /// </summary>
        /// <param name="applicationImage">The <see cref="ApplicationImage"/> to visualize.</param>
        /// <param name="desiredSize">Is ignored.</param>
        public virtual ImageSource Visualize(ApplicationImage applicationImage, Size desiredSize)
        {
            var fileApplicationImage = applicationImage as FileApplicationImage;
            if (fileApplicationImage == null)
            {
                throw new ArgumentException("Invalid argument supplied.", nameof(applicationImage));
            }
            
            if (!File.Exists(fileApplicationImage.File))
            {
                return null;
            }

            try
            {
                var image = new BitmapImage(new Uri(fileApplicationImage.File), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable));
                image.Freeze();

                return image;
            }
            catch (Exception ex)
            {
                this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                return null;
            }
        }
    }
}
