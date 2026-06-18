using System.Globalization;
using MyFactory.MauiClient.ViewModels.Products;

namespace MyFactory.MauiClient.Converters;

public class MaterialOptionsToNamesConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IEnumerable<ProductEditPageViewModel.MaterialOptionViewModel> options)
        {
            return options.Select(m => m.Name).ToList();
        }
        return new List<string>();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
