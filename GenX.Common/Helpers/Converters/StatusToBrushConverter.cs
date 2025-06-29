using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace GenX.Common.Helpers.Converters;

public class StatusToBrushConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            0 => Brushes.Gray,
            1 => Brushes.DodgerBlue,
            2 => Brushes.Red,
            3 => Brushes.MediumSeaGreen,
            _ => Brushes.Gray
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}