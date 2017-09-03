namespace JanHafner.Smartbar.Common.UserInterface.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public abstract class EnumToPackIconKindValueConverter<TIconPackKind> : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (TIconPackKind)value;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}