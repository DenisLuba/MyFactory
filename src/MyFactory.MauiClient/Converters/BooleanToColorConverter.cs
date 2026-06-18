using System.Globalization;

namespace MyFactory.MauiClient.Converters;

public class BooleanToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool flag)
            return null;

        if (parameter is Color[] colors && colors.Length >= 2)
            return flag ? colors[0] : colors[1];

        if (parameter is string raw)
        {
            var parts = raw.Split([ ';', ',' ], StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2
                && Color.TryParse(parts[0].Trim(), out var onColor)
                && Color.TryParse(parts[1].Trim(), out var offColor))
            {
                return flag ? onColor : offColor;
            }
        }

        // fallback colors if no valid parameter provided
        return flag ? Colors.Green : Colors.Red;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
