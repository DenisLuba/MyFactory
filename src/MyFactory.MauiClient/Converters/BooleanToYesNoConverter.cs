using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.Converters;

public class BooleanToYesNoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool flag)
        {
            return flag ? "Да" : "Нет";
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
