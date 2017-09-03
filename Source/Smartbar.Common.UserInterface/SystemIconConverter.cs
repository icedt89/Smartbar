namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;

    [ValueConversion(typeof(MessageBoxImage), typeof(ImageSource))]
    internal sealed class SystemIconConverter : IValueConverter
    {
        [CanBeNull]
        public Object Convert([CanBeNull] Object value, [CanBeNull] Type targetType, [CanBeNull] Object parameter, [CanBeNull] CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var messageBoxImage = (MessageBoxImage) value;

            var systemIconMembers = typeof (SystemIcons).GetMember(messageBoxImage.ToString());
            var systemIconMember = systemIconMembers.FirstOrDefault();

            var systemIcon = (Icon) systemIconMember?.GetValue();
            return systemIcon?.ToImageSource();
        }

        public Object ConvertBack([CanBeNull] Object value, [CanBeNull] Type targetType, [CanBeNull] Object parameter, [CanBeNull] CultureInfo culture)
        {
           throw new NotSupportedException();
        }
    }
}
