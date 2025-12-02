using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Purchases;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

public partial class PurchaseRequestLineEditorViewModel : ObservableObject
{
    private readonly IMaterialsService _materialsService;
    private readonly TaskCompletionSource<PurchaseRequestLineEditorResult?> _completionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private Guid? _lineId;
    private bool _isClosing;
    private bool _materialsLoaded;
    private bool _suppressMaterialSelectionUpdates;

    public PurchaseRequestLineEditorViewModel(IMaterialsService materialsService)
    {
        _materialsService = materialsService;
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        CancelCommand = new AsyncRelayCommand(CancelAsync);
    }

    public ObservableCollection<MaterialLookupItem> Materials { get; } = new();

    [ObservableProperty]
    private MaterialLookupItem? selectedMaterial;

    [ObservableProperty]
    private string materialName = string.Empty;

    [ObservableProperty]
    private string unit = "шт";

    [ObservableProperty]
    private string quantityText = string.Empty;

    [ObservableProperty]
    private string priceText = string.Empty;

    [ObservableProperty]
    private string? note;

    [ObservableProperty]
    private bool isEditMode;

    public string Title => IsEditMode ? "Изменение позиции" : "Новая позиция";

    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand CancelCommand { get; }

    partial void OnIsEditModeChanged(bool value) => OnPropertyChanged(nameof(Title));

    public async Task InitializeAsync(PurchaseRequestLineItem? line)
    {
        await EnsureMaterialsLoadedAsync();

        if (line is null)
        {
            _lineId = null;
            _suppressMaterialSelectionUpdates = true;
            SelectedMaterial = null;
            _suppressMaterialSelectionUpdates = false;
            MaterialName = string.Empty;
            Unit = "шт";
            QuantityText = string.Empty;
            PriceText = string.Empty;
            Note = string.Empty;
            IsEditMode = false;
            return;
        }

        _lineId = line.LineId;
        var existingMaterial = Materials.FirstOrDefault(m => m.MaterialId == line.MaterialId);
        if (existingMaterial is null)
        {
            existingMaterial = new MaterialLookupItem(line.MaterialId, string.Empty, line.MaterialName, line.Unit, line.Price);
            Materials.Insert(0, existingMaterial);
        }

        _suppressMaterialSelectionUpdates = true;
        SelectedMaterial = existingMaterial;
        _suppressMaterialSelectionUpdates = false;

        MaterialName = line.MaterialName;
        Unit = line.Unit;
        QuantityText = line.Quantity.ToString("0.###", CultureInfo.CurrentCulture);
        PriceText = line.Price.ToString("0.##", CultureInfo.CurrentCulture);
        Note = line.Note;
        IsEditMode = true;
    }

    public Task<PurchaseRequestLineEditorResult?> WaitForResultAsync() => _completionSource.Task;

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
        var item = await ValidateAsync();
        if (item is null)
        {
            return;
        }

        await CloseAsync(new PurchaseRequestLineEditorResult(_lineId, item));
    }

    private Task CancelAsync() => CloseAsync(null);

    private async Task CloseAsync(PurchaseRequestLineEditorResult? result)
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

    private async Task<PurchaseItemRequest?> ValidateAsync()
    {
        if (SelectedMaterial is null)
        {
            await ShowAlertAsync("Ошибка", "Выберите материал из справочника.");
            return null;
        }

        var materialId = SelectedMaterial.MaterialId;
        var resolvedName = string.IsNullOrWhiteSpace(MaterialName) ? SelectedMaterial.Name : MaterialName.Trim();
        if (string.IsNullOrWhiteSpace(resolvedName))
        {
            await ShowAlertAsync("Ошибка", "Наименование материала обязательно.");
            return null;
        }

        if (!TryParseDouble(QuantityText, out var quantity) || quantity <= 0)
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

        return new PurchaseItemRequest(
                materialId,
                resolvedName,
            quantity,
            Unit.Trim(),
            price,
            string.IsNullOrWhiteSpace(Note) ? null : Note.Trim());
    }

    private static bool TryParseDouble(string? input, out double value)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            value = 0;
            return false;
        }

        return double.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out value) ||
               double.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out value);
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

    partial void OnSelectedMaterialChanged(MaterialLookupItem? value)
    {
        if (_suppressMaterialSelectionUpdates)
        {
            return;
        }

        if (value is null)
        {
            MaterialName = string.Empty;
            return;
        }

        MaterialName = value.Name;
        Unit = value.Unit;
        if (string.IsNullOrWhiteSpace(QuantityText))
        {
            QuantityText = "1";
        }

        if (value.LastPrice <= 0)
        {
            return;
        }

        if (!TryParseDecimal(PriceText, out var currentPrice) || currentPrice <= 0)
        {
            PriceText = value.LastPrice.ToString("0.##", CultureInfo.CurrentCulture);
        }
    }

    private async Task EnsureMaterialsLoadedAsync()
    {
        if (_materialsLoaded)
        {
            return;
        }

        try
        {
            var materials = await _materialsService.ListAsync();
            Materials.Clear();
            if (materials is not null)
            {
                foreach (var material in materials.OrderBy(m => m.Name))
                {
                    Materials.Add(new MaterialLookupItem(material.Id, material.Code, material.Name, material.Unit, material.LastPrice));
                }
            }

            _materialsLoaded = true;
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Материалы", $"Не удалось загрузить справочник: {ex.Message}");
        }
    }
}

public record PurchaseRequestLineEditorResult(Guid? LineId, PurchaseItemRequest Item);
