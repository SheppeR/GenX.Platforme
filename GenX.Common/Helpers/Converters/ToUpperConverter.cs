using System.Globalization;
using System.Windows.Data;

namespace GenX.Common.Helpers.Converters;

public class ToUpperConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var strValue = value?.ToString();

        return strValue?.ToUpperInvariant();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}