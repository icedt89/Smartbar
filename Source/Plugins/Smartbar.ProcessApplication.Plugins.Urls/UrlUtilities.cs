namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls
{
    using System;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    internal static class UrlUtilities
    {
        [CanBeNull]
        public static String GetDefaultBrowserIcon([CanBeNull] out Int32? identifier, out IconIdentifierType identifierType)
        {
            return SafeNativeMethods.RetrieveAssociatedIcon("http", out identifier, out identifierType);
        }

        [NotNull]
        public static String GetDefaultBrowserPath()
        {
            return SafeNativeMethods.RetrieveAssociatedExecutable("http");
        }

        [NotNull]
        public static readonly String UrlFileExtension = ".url";

        public static Boolean TryCreateValidUri([NotNull] this String data, [CanBeNull] out Uri uri)
        {
            if (String.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            uri = null;
            try
            {
                var uriBuilder = new UriBuilder(data);

                uri = uriBuilder.Uri;
                return !uriBuilder.Uri.IsFile;
            }
            catch (UriFormatException)
            {
                return false;
            }
        }
    }
}
