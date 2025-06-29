using System.Globalization;
using System.Windows.Data;
using CodingSeb.Localization;

namespace GenX.Common.Helpers.Converters;

public class StatusToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            0 => Loc.Tr("Offline"),
            1 => Loc.Tr("Online"),
            2 => Loc.Tr("Busy"),
            3 => Loc.Tr("Event"),
            _ => Loc.Tr("Offline")
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}