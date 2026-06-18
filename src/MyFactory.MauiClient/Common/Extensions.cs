using CommunityToolkit.Maui;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Services.Common;
using SkiaSharp;

namespace MyFactory.MauiClient.Common;

public static class Extensions
{
    #region CONVERTORS
    #region PRODUCT STATUS CONVERTERS
    #region RUS PRODUCT STATUS
    public static string ProductRusStatus(this ProductStatus status)
        => status switch
        {
            ProductStatus.Active => "Активен",
            ProductStatus.Inactive => "Неактивен",
            ProductStatus.Development => "В разработке",
            ProductStatus.Discontinued => "Снят с производства",
            _ => "Неизвестный статус"
        };
    #endregion

    #region PRODUCT STATUS FROM RUS
    public static ProductStatus ProductStatusFromRus(this string status)
        => status switch
        {
            "Активен" => ProductStatus.Active,
            "Неактивен" => ProductStatus.Inactive,
            "В разработке" => ProductStatus.Development,
            "Снят с производства" => ProductStatus.Discontinued,
            _ => throw new ArgumentException("Неизвестный статус", nameof(status))
        };
    #endregion
    #endregion

    #region PRODUCTION ORDER STATUS CONVERTERS
    #region RUS PRODUCTION STATUS
    public static string ProductionOrderRusStatus(this ProductionOrderStatus status)
        => status switch
        {
            ProductionOrderStatus.New => "Новый ПЗ",
            ProductionOrderStatus.MaterialIssued => "Выданы материалы",
            ProductionOrderStatus.Cutting => "В раскрое",
            ProductionOrderStatus.Sewing => "В пошиве",
            ProductionOrderStatus.Packaging => "В упаковке",
            ProductionOrderStatus.Finished => "Закончено",
            ProductionOrderStatus.Cancelled => "Отклонено",
            _ => "Неизвестный статус"
        };
    #endregion

    #region PRODUCTION ORDER STATUS FROM RUS
    public static ProductionOrderStatus? ProductionOrderStatusFromRus(this string status)
        => status switch
        {
            "Новый ПЗ" => ProductionOrderStatus.New,
            "Выданы материалы" => ProductionOrderStatus.MaterialIssued,
            "В раскрое" => ProductionOrderStatus.Cutting,
            "В пошиве" => ProductionOrderStatus.Sewing,
            "В упаковке" => ProductionOrderStatus.Packaging,
            "Закончено" => ProductionOrderStatus.Finished,
            "Отклонено" => ProductionOrderStatus.Cancelled,
            _ => null
        };
    #endregion
    #endregion

    #region RUS SALES ORDER STATUS CONVERTERS
    #region RUS SALES ORDER STATUS
    public static string SalesOrderRusStatus(this SalesOrderStatus status)
        => status switch
        {
            SalesOrderStatus.New => "Новый",
            SalesOrderStatus.Confirmed => "Подтвержден",
            SalesOrderStatus.PartiallyFulfilled => "Частично выполнено",
            SalesOrderStatus.Fulfilled => "Выполнено",
            SalesOrderStatus.Cancelled => "Отклонено",
            _ => "Неизвестный статус"
        };
    #endregion

    #region SALES ORDER STATUS FROM RUS
    public static SalesOrderStatus? SalesOrderStatusFromRus(this string status)
        => status switch
        {
            "Новый" => SalesOrderStatus.New,
            "Подтвержден" => SalesOrderStatus.Confirmed,
            "Частично выполнено" => SalesOrderStatus.PartiallyFulfilled,
            "Выполнено" => SalesOrderStatus.Fulfilled,
            "Отклонено" => SalesOrderStatus.Cancelled,
            _ => null
        };
    #endregion
    #endregion
    #endregion

    #region CAPITALIZE FIRST
    public static string CapitalizeFirst(this string value)
        => string.IsNullOrEmpty(value)
            ? value
            : char.ToUpper(value[0]) + value[1..];
    #endregion

    #region STRING TO DECIMAL
    public static decimal StringToDecimal(this string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new InvalidOperationException("The string cannot be empty.");

        if (!decimal.TryParse(number.Replace(" ", "").Replace(".", ","), out var result))
            throw new InvalidOperationException("The string must be a number.");

        return result;
    }
    #endregion

    #region BACK
    public static async Task BackSafeAsync(
        this Shell shell,
        string? fallbackRoute = null,
        IDictionary<string, object>? parameters = null,
        bool animate = true)
    {
        if (shell is null)
            return;

        var nav = shell.Navigation;

        // Есть экран в стеке -> обычный back
        if (nav?.NavigationStack?.Count > 1)
        {
            await shell.GoToAsync("..", animate);
            return;
        }

        // Нет back-стека -> fallback
        if (!string.IsNullOrWhiteSpace(fallbackRoute))
        {
            if (parameters is not null && parameters.Count > 0)
                await shell.GoToAsync(fallbackRoute, animate, parameters);
            else
                await shell.GoToAsync(fallbackRoute, animate);
        }
    }
    #endregion

    #region RUN SAFE
    public static async Task RunSafeAsync(
        this Func<Task> action,
        Func<bool> getIsBusy,
        Action<bool> setIsBusy,
        Action<string?> setError,
        Func<string, Task>? showError = null)
    {
        if (getIsBusy())
            return;

        try
        {
            setIsBusy(true);
            setError(null);
            await action();
        }
        catch (Exception ex)
        {
            setError(ex.Message);
            if (showError is not null)
                await showError(ex.Message);
        }
        finally
        {
            setIsBusy(false);
        }
    }
    #endregion

    #region COMPRESS PHOTO
    public static async Task<byte[]> CompressPhotoAsync(
            this Stream input,
            int maxWidth = 1920,
            int maxHeight = 1920,
            int quality = 75,
            CancellationToken ct = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));
        quality = Math.Clamp(quality, 1, 100);

        using var source = new MemoryStream();
        if (input.CanSeek) input.Position = 0;
        await input.CopyToAsync(source, ct);
        source.Position = 0;

        using var original = SKBitmap.Decode(source);
        if (original is null)
            return source.ToArray();

        var scale = Math.Min(
            (float)maxWidth / original.Width,
            (float)maxHeight / original.Height);

        scale = Math.Min(scale, 1f); // не увеличиваем маленькие изображения

        var newWidth = Math.Max(1, (int)Math.Round(original.Width * scale));
        var newHeight = Math.Max(1, (int)Math.Round(original.Height * scale));

        using var resized = original.Resize(
            new SKImageInfo(newWidth, newHeight),
            new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None))
            ?? original.Copy();


        using var image = SKImage.FromBitmap(resized);
        using var encoded = image.Encode(SKEncodedImageFormat.Jpeg, quality);

        return encoded?.ToArray() ?? source.ToArray();
    }
    #endregion

    #region ENTRY FOCUSED
    public static async Task<Guid> EntryFocusedAsync<T>(this IPopupService popupService, IGetListService<T> service, string? name = null, string? type = null, bool? isActive = null) where T : ListItemResponse
    {
        var query = new Dictionary<string, object>
        {
            ["Search"] = new SearchPageSource<T>(service: service, isActive: isActive),
            ["Name"] = name ?? string.Empty,
            ["Type"] = type ?? string.Empty
        };

        return await popupService.EntryFocusedAsync(query);
    }

    public static async Task<Guid> EntryFocusedAsync(this IPopupService popupService, ISearchPageSouce source, string? name = null, string? type = null)
    {
        var query = new Dictionary<string, object>
        {
            ["Search"] = source,
            ["Name"] = name ?? string.Empty,
            ["Type"] = type ?? string.Empty
        };

        return await popupService.EntryFocusedAsync(query);
    }

    private static async Task<Guid> EntryFocusedAsync(this IPopupService popupService, IDictionary<string, object> query)
    {
        var selected = await popupService.ShowPopupAsync<SearchViewModel, string>(Shell.Current, PopupOptions.Empty, query);

        if (selected is not { Result: string result })
            return Guid.Empty;

        if (!Guid.TryParse(result, out var id))
            return Guid.Empty;

        return id;
    }
    #endregion
}
