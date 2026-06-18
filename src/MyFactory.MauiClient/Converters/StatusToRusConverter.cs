using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.SalesOrders;
using System.Globalization;

namespace MyFactory.MauiClient.Converters;

public sealed class StatusToRusConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SalesOrderStatus salesOrderStatus)
        {
            return salesOrderStatus.SalesOrderRusStatus();
        }
        if (value is ProductionOrderStatus productionOrderStatus)
        {
            return productionOrderStatus.ProductionOrderRusStatus();
        }
        if (value is ProductStatus productStatus)
            return productStatus.ProductRusStatus();

        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
