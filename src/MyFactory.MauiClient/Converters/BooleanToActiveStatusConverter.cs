using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.Converters;

public class BooleanToActiveStatusConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool flag)
        {
            return flag ? "Активен" : "Неактивен";
        }

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string text && text.Equals("Активен", StringComparison.OrdinalIgnoreCase);
}
