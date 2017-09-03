namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType,
                              Object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return false;
            }

            var checkValue = value.ToString();
            var targetValue = parameter.ToString();
            return checkValue.Equals(targetValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public Object ConvertBack(Object value, Type targetType,
                                  Object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            var useValue = (Boolean)value;
            var targetValue = parameter.ToString();

            return useValue ? Enum.Parse(targetType, targetValue) : null;
        }
    }
}
