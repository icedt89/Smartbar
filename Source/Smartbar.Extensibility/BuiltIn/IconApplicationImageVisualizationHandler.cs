namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Media;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Model;
    using JanHafner.Toolkit.Windows;
    using JanHafner.Toolkit.Windows.Icons;
    using Size = System.Windows.Size;

    [Export(typeof(IImageVisualizationHandler))]
    public class IconApplicationImageVisualizationHandler : IImageVisualizationHandler
    {
        private static readonly ICollection<String> forcedFallbackLookup = new HashSet<String>();
            
        public virtual Boolean CanVisualize(ApplicationImage applicationImage)
        {
            return applicationImage is IconApplicationImage;
        }

        /// <summary>
        /// This implementation ignores the <param name="desiredSize" /> parameter and uses the actual resource size from the file.
        /// </summary>
        /// <param name="applicationImage">The <see cref="ApplicationImage"/> to visualize.</param>
        /// <param name="desiredSize">Is ignored.</param>
        public virtual ImageSource Visualize(ApplicationImage applicationImage, Size desiredSize)
        {
            var iconApplicationImage = applicationImage as IconApplicationImage;
            if (iconApplicationImage == null)
            {
                throw new ArgumentException("Invalid argument supplied.", nameof(applicationImage));
            }

            var icon = this.GetIcon(iconApplicationImage, desiredSize);
            if (icon == null)
            {
                return null;
            }

            using (icon)
            {
                return icon.ToImageSource();
            }
        }

        protected virtual Icon GetIcon(IconApplicationImage iconApplicationImage, Size desiredSize)
        {
            var fallbackFunctionKey = $"{iconApplicationImage.File}:{iconApplicationImage.Identifier}";

            Func<Icon> retrieveIconAtIndexZero = () => SafeNativeMethods.ExtractIcon(iconApplicationImage.File, 0);
            if (IconApplicationImageVisualizationHandler.forcedFallbackLookup.Contains(fallbackFunctionKey))
            {
                return retrieveIconAtIndexZero();
            }

            try
            {
                switch (iconApplicationImage.IdentifierType)
                {
                    case IconIdentifierType.Index:
                        IconExtractor iconExtractor;
                        if (IconExtractor.CouldBeIconFile(iconApplicationImage.File) && IconExtractor.TryCreate(iconApplicationImage.File, out iconExtractor))
                        {
                            using (iconExtractor)
                            {
                                return iconExtractor.EnumerateIcons().Where((icon, index) => index == iconApplicationImage.Identifier).FirstOrDefault();
                            }
                        }

                        return SafeNativeMethods.ExtractIcon(iconApplicationImage.File, iconApplicationImage.Identifier);
                    case IconIdentifierType.ResourceId:
                        using (var nativeExecutable = new NativeExecutable(iconApplicationImage.File))
                        {
                            return nativeExecutable.ExtractIconResource((UInt32)Math.Abs(iconApplicationImage.Identifier));
                        }
                    default:
                    case IconIdentifierType.Unknown:
                        return retrieveIconAtIndexZero();
                }
            }
            catch (Win32Exception)
            {
                IconApplicationImageVisualizationHandler.forcedFallbackLookup.Add(fallbackFunctionKey);

                // If this fails, try to get the icon at index 0...
                return retrieveIconAtIndexZero();
            }
        }
    }
}
