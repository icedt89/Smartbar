namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NegatingBooleanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return !(value is Boolean && (Boolean)value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return !(value is Boolean && (Boolean)value);
        }
    }
}
