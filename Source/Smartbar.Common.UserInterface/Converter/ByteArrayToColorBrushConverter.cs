namespace JanHafner.Smartbar.Common.UserInterface.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using JanHafner.Smartbar.Extensibility;

    public sealed class ByteArrayToColorBrushConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var colorBytes = (Byte[]) value;
            var color = colorBytes.FromXaml<Color>();

            return new SolidColorBrush(color);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
