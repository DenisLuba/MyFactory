using MyFactory.MauiClient.Models.Products;

namespace MyFactory.MauiClient.Common;

public static class Extensions
{
    public static string RusStatus(this ProductStatus status)
        => status switch
        {
            ProductStatus.Active => "Активен",
            ProductStatus.Inactive => "Неактивен",
            ProductStatus.Development => "В разработке",
            ProductStatus.Discontinued => "Снят с производства",
            _ => "Неизвестный статус"
        };

    public static ProductStatus StatusFromRus(this string status)
        => status switch
        {
            "Активен" => ProductStatus.Active,
            "Неактивен" => ProductStatus.Inactive,
            "В разработке" => ProductStatus.Development,
            "Снят с производства" => ProductStatus.Discontinued,
            _ => throw new ArgumentException("Неизвестный статус", nameof(status))
        };

    public static string CapitalizeFirst(this string value)
        => string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value[1..];

    public static decimal StringToDecimal(this string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new InvalidOperationException("The string cannot be empty.");

        if (!decimal.TryParse(number.Replace(" ", "").Replace(".", ","), out var result))
            throw new InvalidOperationException("The string must be a number.");

        return result;
    }
}
