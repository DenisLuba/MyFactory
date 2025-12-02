using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.Converters;

public class DecimalToStringConverter : IValueConverter
{
    public string FormatString { get; set; } = "0.##";

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return value switch
        {
            decimal decimalValue => decimalValue.ToString(FormatString, culture),
            double doubleValue => ((decimal)doubleValue).ToString(FormatString, culture),
            _ => value.ToString()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return 0m;
        }

        if (value is decimal decimalValue)
        {
            return decimalValue;
        }

        if (value is double doubleValue)
        {
            return (decimal)doubleValue;
        }

        var input = value.ToString();
        if (string.IsNullOrWhiteSpace(input))
        {
            return 0m;
        }

        return decimal.TryParse(input, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, culture, out var result)
            ? result
            : Binding.DoNothing;
    }
}
