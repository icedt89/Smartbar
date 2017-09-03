namespace JanHafner.Smartbar.Common.Localization
{
    using System;
    using System.Globalization;
    using JetBrains.Annotations;

    public sealed class UICultureChangedEventArgs : EventArgs
    {
        public UICultureChangedEventArgs([NotNull] CultureInfo currentLanguage)
        {
            if (currentLanguage == null)
            {
                throw new ArgumentNullException(nameof(currentLanguage));
            }

            this.CurrentLanguage = currentLanguage;
        }

        [NotNull]
        public CultureInfo CurrentLanguage { get; private set; }
    }
}
