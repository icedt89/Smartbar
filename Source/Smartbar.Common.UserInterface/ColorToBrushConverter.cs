namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    internal sealed class ColorToBrushConverter : IValueConverter
    {
        private static readonly IDictionary<Color, SolidColorBrush> brushCache = new Dictionary<Color, SolidColorBrush>();

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var color = (Color)value;
            SolidColorBrush brush;
            if (!brushCache.TryGetValue(color, out brush))
            {
                brush = new SolidColorBrush(color);
                brushCache.Add(color, brush);
            }

            return brush;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var brush = (SolidColorBrush)value;
            return brush.Color;
        }
    }
}
