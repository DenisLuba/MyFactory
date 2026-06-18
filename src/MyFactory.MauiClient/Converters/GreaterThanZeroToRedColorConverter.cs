using System.Globalization;

namespace MyFactory.MauiClient.Converters;

public class GreaterThanZeroToRedColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IConvertible && value is not bool && value is not char)
        {
            var flag = System.Convert.ToDecimal(value) > 0;

            if (parameter is Color[] colors && colors.Length >= 2)
                return flag ? colors[0] : colors[1];

            if (parameter is string raw)
            {
                var parts = raw.Split([';', ','], StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2
                    && Color.TryParse(parts[0].Trim(), out var onColor)
                    && Color.TryParse(parts[1].Trim(), out var offColor))
                {
                    return flag ? onColor : offColor;
                }
            }

            return flag
                ? (Application.Current?.Resources["RemoveButtonColor"] as Color) ?? Colors.Red
                : (Application.Current?.Resources["BackButtonColor"] as Color) ?? Colors.Blue;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
