namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls.CreateApplicationStrategy
{
    using System;
    using System.IO;
    using JetBrains.Annotations;

    internal static class CreateApplicationStrategySelector
    {
        [CanBeNull] 
        public static ICreateApplicationStrategy SelectCreateApplicationStrategy(String data)
        {
            if (CreateApplicationStrategySelector.CanCreateFromUrl(data))
            {
                return new CreateFromUrlStringStrategy();
            }

            return CreateApplicationStrategySelector.CanCreateFromUrlFile(data) ? new CreateFromUrlFileStrategy() : null;
        }

        private static Boolean CanCreateFromUrl(String data)
        {
            Uri uri;
            return data.TryCreateValidUri(out uri);
        }

        private static Boolean CanCreateFromUrlFile(String data)
        {
            return File.Exists(data) && Path.GetExtension(data).Equals(UrlUtilities.UrlFileExtension, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}