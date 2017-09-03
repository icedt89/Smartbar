namespace JanHafner.Smartbar.Common.UserInterface.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public sealed class StretchSmallImageValueConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Boolean) value ? StretchDirection.Both : StretchDirection.DownOnly;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
