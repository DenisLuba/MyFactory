using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.Converters;

public class DecimalToCurrencyConverter : IValueConverter
{
    public string FormatString { get; set; } = "N2";
    public string CurrencySymbol { get; set; } = "₽";
    public string? Suffix { get; set; }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var amount = value switch
        {
            decimal decimalValue => decimalValue,
            double doubleValue => (decimal)doubleValue,
            _ => (decimal?)null
        };

        if (amount is null)
        {
            return value.ToString();
        }

        var formatted = amount.Value.ToString(FormatString, culture);

        if (!string.IsNullOrWhiteSpace(CurrencySymbol))
        {
            formatted = $"{formatted} {CurrencySymbol}".Trim();
        }

        var suffix = parameter as string ?? Suffix;
        if (!string.IsNullOrWhiteSpace(suffix))
        {
            formatted = $"{formatted} {suffix}".Trim();
        }

        return formatted;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => Binding.DoNothing;
}
