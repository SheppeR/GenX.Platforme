using System.Globalization;
using System.Windows.Data;

namespace GenX.Common.Helpers.Converters;

public class EmptyStringToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var str = (string)value!;
        return !string.IsNullOrEmpty(str);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}