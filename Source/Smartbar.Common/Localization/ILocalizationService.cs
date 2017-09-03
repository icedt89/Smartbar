namespace JanHafner.Smartbar.Common.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using JetBrains.Annotations;

    public interface ILocalizationService
    {
        [NotNull]
        CultureInfo CurrentLanguage { get; set; }

        [NotNull]
        [LinqTunnel]
        IEnumerable<CultureInfo> GetAvailableLanguages();

        [NotNull]
        String Localize(Type resourceType, [NotNull] String resourceName);

        event EventHandler<UICultureChangedEventArgs> CurrentUICultureChanged;
    }
}