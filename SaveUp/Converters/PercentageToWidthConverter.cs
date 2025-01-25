using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SaveUp.Converters
{
    public class PercentageToWidthConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double progressPercentage && parameter is string totalWidthStr && double.TryParse(totalWidthStr, out var totalWidth))
            {
                return progressPercentage * totalWidth;
            }
            return 0; // Standardwert bei Fehler
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
