using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.Converters;

public class GreaterThanZeroConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IConvertible convertible)
        {
            return convertible.ToDouble(culture) > 0;
        }

        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
