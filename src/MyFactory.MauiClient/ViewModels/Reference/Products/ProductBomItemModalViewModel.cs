using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.ProductsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Products;

public partial class ProductBomItemModalViewModel : ObservableObject
{
    private readonly IProductsService _productsService;
    private readonly TaskCompletionSource<ProductBomItemResponse?> _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _isClosing;

    public ProductBomItemModalViewModel(IProductsService productsService)
    {
        _productsService = productsService;
        SubmitCommand = new AsyncRelayCommand(SaveAsync, CanSubmit);
        CancelCommand = new AsyncRelayCommand(CancelAsync);
    }

    [ObservableProperty]
    private Guid productId;

    [ObservableProperty]
    private string productName = string.Empty;

    [ObservableProperty]
    private string material = string.Empty;

    [ObservableProperty]
    private string qtyText = string.Empty;

    [ObservableProperty]
    private string unit = "м";

    [ObservableProperty]
    private string priceText = string.Empty;

    [ObservableProperty]
    private bool isSubmitting;

    public IAsyncRelayCommand SubmitCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

    public void Initialize(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
        Material = string.Empty;
        QtyText = string.Empty;
        Unit = "м";
        PriceText = string.Empty;
        IsSubmitting = false;
    }

    public Task<ProductBomItemResponse?> WaitForResultAsync() => _completionSource.Task;

    public void NotifyClosedExternally()
    {
        if (_completionSource.Task.IsCompleted)
        {
            return;
        }

        _completionSource.TrySetResult(null);
    }

    partial void OnIsSubmittingChanged(bool value) => SubmitCommand.NotifyCanExecuteChanged();

    private bool CanSubmit() => !IsSubmitting;

    private async Task SaveAsync()
    {
        if (IsSubmitting)
        {
            return;
        }

        if (ProductId == Guid.Empty)
        {
            await ShowAlertAsync("Ошибка", "Изделие не выбрано");
            return;
        }

        if (string.IsNullOrWhiteSpace(Material))
        {
            await ShowAlertAsync("Ошибка", "Наименование материала обязательно");
            return;
        }

        if (!TryParseDouble(QtyText, out var qty) || qty <= 0)
        {
            await ShowAlertAsync("Ошибка", "Расход должен быть положительным числом");
            return;
        }

        if (string.IsNullOrWhiteSpace(Unit))
        {
            await ShowAlertAsync("Ошибка", "Единица измерения обязательна");
            return;
        }

        if (!TryParseDecimal(PriceText, out var price) || price < 0)
        {
            await ShowAlertAsync("Ошибка", "Цена должна быть неотрицательным числом");
            return;
        }

        try
        {
            IsSubmitting = true;
            var request = new ProductBomCreateRequest(Material.Trim(), qty, Unit.Trim(), price);
            var created = await _productsService.AddBomItemAsync(ProductId, request);
            if (created is null)
            {
                await ShowAlertAsync("Ошибка", "Сервис не вернул созданную строку");
                return;
            }

            await ShowAlertAsync("Готово", "Материал добавлен");
            await CloseAsync(created);
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Ошибка", $"Не удалось добавить материал: {ex.Message}");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private async Task CancelAsync() => await CloseAsync(null);

    private async Task CloseAsync(ProductBomItemResponse? result)
    {
        if (_isClosing)
        {
            return;
        }

        _isClosing = true;
        _completionSource.TrySetResult(result);

        if (Shell.Current?.Navigation is not null)
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }

    private static bool TryParseDouble(string? value, out double number)
        => double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out number) ||
           double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number);

    private static bool TryParseDecimal(string? value, out decimal number)
        => decimal.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out number) ||
           decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number);

    private static Task ShowAlertAsync(string title, string message)
        => Shell.Current.DisplayAlertAsync(title, message, "OK");
}
