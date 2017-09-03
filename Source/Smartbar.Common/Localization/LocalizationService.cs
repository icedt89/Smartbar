namespace JanHafner.Smartbar.Common.Localization
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Windows;
    using JetBrains.Annotations;

    /// <summary>
    ///  Provides methods for updating the current UI culture and notifies subscribers e.g. the ResX MarkupExtension which cannot use DependencyInjection due to instanciation in XAML.
    /// </summary>
    public sealed class LocalizationService : ILocalizationService
    {
        private readonly ConcurrentDictionary<Type, ResourceManager> cachedResourceManagers;
         
        private CultureInfo currentLanguage = Application.Current.Dispatcher.Thread.CurrentUICulture;

        [NotNull]
        private static readonly ILocalizationService current = new LocalizationService();

        private LocalizationService()
        {
            this.cachedResourceManagers = new ConcurrentDictionary<Type, ResourceManager>();
        }

        /// <summary>
        /// Just for Design-View.
        /// </summary>
        [NotNull]
        [Export(typeof(ILocalizationService))]
        public static ILocalizationService Current
        {
            get
            {
                return current;
            }
        }

        public CultureInfo CurrentLanguage
        {
            get
            {
                if (!this.currentLanguage.Equals(Application.Current.Dispatcher.Thread.CurrentUICulture))
                {
                    this.SetLanguage(this.currentLanguage);
                }

                return this.currentLanguage;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.currentLanguage = value;
                this.SetLanguage(value);
            }
        }

        private void SetLanguage([NotNull] CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException(nameof(cultureInfo));
            }

            Application.Current.Dispatcher.Thread.CurrentUICulture = cultureInfo;
            this.OnCurrentUICultureChanged(cultureInfo);
        }

        [LinqTunnel]
        public IEnumerable<CultureInfo> GetAvailableLanguages()
        {
            yield return CultureInfo.GetCultureInfo("en");

            foreach (
                var cultureInfo in Directory.EnumerateDirectories(".", "*", SearchOption.TopDirectoryOnly)
                    .Where(cultureInfo => Directory.GetFiles(cultureInfo, "Smartbar*resources*.dll", SearchOption.TopDirectoryOnly).Length > 0)
                    .Join(CultureInfo.GetCultures(CultureTypes.NeutralCultures),
                        directory => new DirectoryInfo(directory).Name,
                        cultureInfo => cultureInfo.Name, (directoryName, cultureInfo) => cultureInfo)
                )
            {
                yield return cultureInfo;
            }
        }

        public String Localize([NotNull] Type resourceType, String resourceName)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException(nameof(resourceType));
            }

            if (resourceName == null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var resourceManager = this.cachedResourceManagers.GetOrAdd(resourceType, type => new ResourceManager(type));

            var localizedResourceString = resourceManager.GetString(resourceName, this.currentLanguage);

            return localizedResourceString ?? String.Empty;
        }

        private EventHandler<UICultureChangedEventArgs> currentUICultureChanged;

        public event EventHandler<UICultureChangedEventArgs> CurrentUICultureChanged
        {
            add { this.currentUICultureChanged += value; }
            remove { this.currentUICultureChanged -= value; }
        }

        private void OnCurrentUICultureChanged(CultureInfo currentUICulture)
        {
            var handler = this.currentUICultureChanged;
            handler?.Invoke(this, new UICultureChangedEventArgs(currentUICulture));
        }
    }
}
