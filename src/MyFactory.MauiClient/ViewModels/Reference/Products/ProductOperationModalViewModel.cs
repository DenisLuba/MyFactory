using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.ProductsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Products;

public partial class ProductOperationModalViewModel : ObservableObject
{
    private readonly IProductsService _productsService;
    private readonly TaskCompletionSource<ProductOperationItemResponse?> _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _isClosing;

    public ProductOperationModalViewModel(IProductsService productsService)
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
    private string operation = string.Empty;

    [ObservableProperty]
    private string minutesText = string.Empty;

    [ObservableProperty]
    private string costText = string.Empty;

    [ObservableProperty]
    private bool isSubmitting;

    public IAsyncRelayCommand SubmitCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

    public void Initialize(Guid productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
        Operation = string.Empty;
        MinutesText = string.Empty;
        CostText = string.Empty;
        IsSubmitting = false;
    }

    public Task<ProductOperationItemResponse?> WaitForResultAsync() => _completionSource.Task;

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

        if (string.IsNullOrWhiteSpace(Operation))
        {
            await ShowAlertAsync("Ошибка", "Наименование операции обязательно");
            return;
        }

        if (!TryParseDouble(MinutesText, out var minutes) || minutes <= 0)
        {
            await ShowAlertAsync("Ошибка", "Время должно быть положительным числом");
            return;
        }

        if (!TryParseDecimal(CostText, out var cost) || cost < 0)
        {
            await ShowAlertAsync("Ошибка", "Стоимость должна быть неотрицательной");
            return;
        }

        try
        {
            IsSubmitting = true;
            var request = new ProductOperationCreateRequest(Operation.Trim(), minutes, cost);
            var created = await _productsService.AddOperationAsync(ProductId, request);
            if (created is null)
            {
                await ShowAlertAsync("Ошибка", "Сервис не вернул созданную строку");
                return;
            }

            await ShowAlertAsync("Готово", "Операция добавлена");
            await CloseAsync(created);
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Ошибка", $"Не удалось добавить операцию: {ex.Message}");
        }
        finally
        {
            IsSubmitting = false;
        }
    }

    private async Task CancelAsync() => await CloseAsync(null);

    private async Task CloseAsync(ProductOperationItemResponse? result)
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
