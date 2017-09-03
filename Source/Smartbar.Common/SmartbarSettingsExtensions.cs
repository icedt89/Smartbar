namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using JetBrains.Annotations;

    public static class SmartbarSettingsExtensions
    {
        [NotNull]
        public static CultureInfo GetCultureInfoForLanguage([NotNull] this ISmartbarSettings smartbarSettings)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            try
            {
                return CultureInfo.GetCultureInfo(smartbarSettings.IsFirstStart 
                    ? Application.Current.Dispatcher.Thread.CurrentUICulture.TwoLetterISOLanguageName 
                    : smartbarSettings.LanguageIdentifier);
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.GetCultureInfo(smartbarSettings.DefaultLanguageIdentifier);
            }
        }

        public static String CreateAndGetPluginsDirectory(this NameValueCollection appSettingsSection)
        {
            var basePath = CreateBaseDirectory();
            basePath = Path.Combine(basePath, appSettingsSection["BasePluginsDirectoryName"]);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            return basePath;
        }

        public static String CreateAndGetSettingsFile()
        {
            return CreateAndGetFile("settings.json");
        }

        public static String CreateAndGetDatabaseFile(this ISmartbarSettings smartbarSettings)
        {
            return CreateAndGetFile(smartbarSettings.DatabaseFileName);
        }

        private static String CreateAndGetFile(String fileName)
        {
            var basePath = CreateBaseDirectory();

            return Path.GetFullPath($@"{basePath}\{fileName}");
        }

        public static String CreateAndGetPluginsDirectory(this ISmartbarSettings smartbarSettings)
        {
            var basePath = CreateBaseDirectory();
            basePath = Path.Combine(basePath, smartbarSettings.BasePluginsDirectoryName);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            return basePath;
        }

        public static String CreateAndGetPackagesDirectory(this ISmartbarSettings smartbarSettings)
        {
            var basePath = smartbarSettings.CreateAndGetPluginsDirectory();
            basePath = Path.Combine(basePath, smartbarSettings.BasePluginPackagesDirectoryName);

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            return basePath;
        }

        private static String CreateBaseDirectory()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            basePath = Path.Combine(basePath, "Smartbar");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            return basePath;
        }

        public static void SetLanguage([NotNull] this ISmartbarSettings smartbarSettings, [NotNull] CultureInfo cultureInfo)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            smartbarSettings.LanguageIdentifier = cultureInfo.TwoLetterISOLanguageName;
        }
    }
}
