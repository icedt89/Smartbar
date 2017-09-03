namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;
    using Prism.Events;

    public static class Extensions
    {
        public static void DisposeAndClear<T>([NotNull] this ICollection<T> items)
           where T : IDisposable
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            items.ForEach(item => item.Dispose());
            items.Clear();
        }

        [NotNull]
        public unsafe static SecureString ToSecureString([NotNull] this String input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            fixed (Char* chars = input)
            {
                return new SecureString(chars, input.Length);
            }
        }

        [NotNull]
        public unsafe static String FromSecureString([NotNull] this SecureString input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var inputPointer = Marshal.SecureStringToBSTR(input);

            try
            {
                return new String((Char*) inputPointer);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(inputPointer);
            }
        }

        public static void UnsubscribeAll([NotNull] this ICollection<SubscriptionToken> subscriptionTokens, [CallerMemberName] String caller = null)
        {
            if (subscriptionTokens == null)
            {
                throw new ArgumentNullException(nameof(subscriptionTokens));
            }

            if (!subscriptionTokens.Any())
            {
                return;
            }

            Debug.WriteLine($"Unsubscribing from {subscriptionTokens.Count} subscriptions of {caller}.");

            subscriptionTokens.DisposeAndClear();
        }

        /// <summary>
        /// Creates an <see cref="ImageSource"/> from the supplied <see cref="Icon"/> by creating a <see cref="Bitmap"/> before.
        /// The supplied <see cref="Icon"/> and the created <see cref="Bitmap"/> are disposed.
        /// </summary>
        /// <param name="icon">The <see cref="Icon"/> from which the <see cref="ImageSource"/> is created.</param>
        /// <returns>The created <see cref="ImageSource"/>.</returns>
        [NotNull]
        public static ImageSource ToImageSource([NotNull] this Icon icon)
        {
            if (icon == null)
            {
                throw new ArgumentNullException(nameof(icon));
            }

            using (var windowsIconBitmap = icon.ToBitmap())
            {
                return windowsIconBitmap.ToImageSource();
            }
        }

        /// <summary>
        /// Creates an <see cref="ImageSource"/> from the supplied <see cref="Bitmap"/>.
        /// The supplied <see cref="Bitmap"/> gets disposed.
        /// The resulting <see cref="ImageSource"/> gets <see cref="Freeze()"/>-d.
        /// </summary>
        /// <param name="bitmap">The <see cref="Bitmap"/> from which the <see cref="ImageSource"/> is created.</param>
        /// <returns>The created <see cref="ImageSource"/>.</returns>
        [NotNull]
        public static ImageSource ToImageSource([NotNull] this Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            var result = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            result.Freeze();

            return result;
        }

        [CanBeNull]
        public static String Localize<TResourceType>(this ILocalizationService localizationService, [NotNull] String resourceName)
        {
            if (resourceName == null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            return localizationService.Localize(typeof(TResourceType), resourceName);
        }
    }
}
