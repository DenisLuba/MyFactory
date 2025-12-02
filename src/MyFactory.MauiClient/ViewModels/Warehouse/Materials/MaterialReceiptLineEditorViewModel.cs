using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.WarehouseMaterials;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Materials;

public partial class MaterialReceiptLineEditorViewModel : ObservableObject
{
    private readonly TaskCompletionSource<MaterialReceiptLineUpsertRequest?> _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private bool _isClosing;

    public MaterialReceiptLineEditorViewModel()
    {
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        CancelCommand = new AsyncRelayCommand(CancelAsync);
    }

    [ObservableProperty]
    private string materialIdText = string.Empty;

    [ObservableProperty]
    private string materialName = string.Empty;

    [ObservableProperty]
    private string unit = "шт";

    [ObservableProperty]
    private string quantityText = string.Empty;

    [ObservableProperty]
    private string priceText = string.Empty;

    [ObservableProperty]
    private bool isEditMode;

    public string Title => IsEditMode ? "Изменение строки" : "Новая строка";

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

    partial void OnIsEditModeChanged(bool value) => OnPropertyChanged(nameof(Title));

    public void Initialize(MaterialReceiptLineItem? line)
    {
        if (line is null)
        {
            MaterialIdText = string.Empty;
            MaterialName = string.Empty;
            Unit = "шт";
            QuantityText = string.Empty;
            PriceText = string.Empty;
            IsEditMode = false;
            return;
        }

        MaterialIdText = line.MaterialId.ToString();
        MaterialName = line.Material;
        Unit = line.Unit;
        QuantityText = line.Quantity.ToString("0.###", CultureInfo.CurrentCulture);
        PriceText = line.Price.ToString("0.##", CultureInfo.CurrentCulture);
        IsEditMode = true;
    }

    public Task<MaterialReceiptLineUpsertRequest?> WaitForResultAsync() => _completionSource.Task;

    public void NotifyClosedExternally()
    {
        if (_completionSource.Task.IsCompleted)
        {
            return;
        }

        _completionSource.TrySetResult(null);
    }

    private async Task SaveAsync()
    {
        var request = await ValidateAsync();
        if (request is null)
        {
            return;
        }

        await CloseAsync(request);
    }

    private Task CancelAsync() => CloseAsync(null);

    private async Task CloseAsync(MaterialReceiptLineUpsertRequest? result)
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

    private async Task<MaterialReceiptLineUpsertRequest?> ValidateAsync()
    {
        if (!Guid.TryParse(MaterialIdText?.Trim(), out var materialId) || materialId == Guid.Empty)
        {
            await ShowAlertAsync("Ошибка", "Введите корректный GUID материала.");
            return null;
        }

        if (!TryParseDecimal(QuantityText, out var quantity) || quantity <= 0)
        {
            await ShowAlertAsync("Ошибка", "Количество должно быть положительным числом.");
            return null;
        }

        if (string.IsNullOrWhiteSpace(Unit))
        {
            await ShowAlertAsync("Ошибка", "Единица измерения обязательна.");
            return null;
        }

        if (!TryParseDecimal(PriceText, out var price) || price <= 0)
        {
            await ShowAlertAsync("Ошибка", "Цена должна быть положительным числом.");
            return null;
        }

        return new MaterialReceiptLineUpsertRequest(materialId, quantity, Unit.Trim(), price);
    }

    private static bool TryParseDecimal(string? input, out decimal value)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = 0;
            return false;
        }

        return decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out value) ||
               decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
    }

    private static Task ShowAlertAsync(string title, string message)
        => Shell.Current.DisplayAlertAsync(title, message, "OK");
}
