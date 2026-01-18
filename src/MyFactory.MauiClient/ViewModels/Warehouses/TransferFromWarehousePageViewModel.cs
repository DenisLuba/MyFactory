using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.Warehouses;

namespace MyFactory.MauiClient.ViewModels.Warehouses;

[QueryProperty(nameof(WarehouseIdParameter), "WarehouseId")]
[QueryProperty(nameof(WarehouseNameParameter), "WarehouseName")]
[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
[QueryProperty(nameof(ProductIdParameter), "ProductId")]
[QueryProperty(nameof(ItemName), "ItemName")]
[QueryProperty(nameof(UnitCode), "UnitCode")]
[QueryProperty(nameof(AvailableQtyParameter), "AvailableQty")]
public partial class TransferFromWarehousePageViewModel : ObservableObject
{
    private readonly IWarehousesService _warehousesService;

    private Guid? warehouseId;
    public Guid? WarehouseId
    {
        get => warehouseId;
        set => SetProperty(ref warehouseId, value);
    }

    private string? warehouseIdParameter;
    public string? WarehouseIdParameter
    {
        get => warehouseIdParameter;
        set
        {
            if (SetProperty(ref warehouseIdParameter, value))
            {
                WarehouseId = Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }

    private string? warehouseNameParameter;
    public string? WarehouseNameParameter
    {
        get => warehouseNameParameter;
        set
        {
            if (SetProperty(ref warehouseNameParameter, value))
            {
                OnPropertyChanged(nameof(SourceWarehouseName));
            }
        }
    }

    private Guid? materialId;
    public Guid? MaterialId
    {
        get => materialId;
        set => SetProperty(ref materialId, value);
    }

    private string? materialIdParameter;
    public string? MaterialIdParameter
    {
        get => materialIdParameter;
        set
        {
            if (SetProperty(ref materialIdParameter, value))
            {
                MaterialId = Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }

    private Guid? productId;
    public Guid? ProductId
    {
        get => productId;
        set => SetProperty(ref productId, value);
    }

    private string? productIdParameter;
    public string? ProductIdParameter
    {
        get => productIdParameter;
        set
        {
            if (SetProperty(ref productIdParameter, value))
            {
                ProductId = Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }

    private string? itemName;
    public string? ItemName
    {
        get => itemName;
        set => SetProperty(ref itemName, value);
    }

    private string? unitCode;
    public string? UnitCode
    {
        get => unitCode;
        set => SetProperty(ref unitCode, value);
    }

    private decimal availableQty;
    public decimal AvailableQty
    {
        get => availableQty;
        set => SetProperty(ref availableQty, value);
    }

    private string? availableQtyParameter;
    public string? AvailableQtyParameter
    {
        get => availableQtyParameter;
        set
        {
            if (SetProperty(ref availableQtyParameter, value))
            {
                AvailableQty = decimal.TryParse(value, out var qty) ? qty : 0;
            }
        }
    }

    private string? quantityInput;
    public string? QuantityInput
    {
        get => quantityInput;
        set
        {
            if (SetProperty(ref quantityInput, value))
            {
                ValidateQuantity(out _);
            }
        }
    }

    private Color quantityColor = Colors.Black;
    public Color QuantityColor
    {
        get => quantityColor;
        set => SetProperty(ref quantityColor, value);
    }

    private string? errorMessage;
    public string? ErrorMessage
    {
        get => errorMessage;
        set => SetProperty(ref errorMessage, value);
    }

    private bool isBusy;
    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value);
    }

    private WarehouseListItemResponse? selectedWarehouse;
    public WarehouseListItemResponse? SelectedWarehouse
    {
        get => selectedWarehouse;
        set => SetProperty(ref selectedWarehouse, value);
    }

    public ObservableCollection<WarehouseListItemResponse> Warehouses { get; } = new();

    public string? SourceWarehouseName => warehouseNameParameter;

    public bool IsProduct => ProductId is not null;

    public TransferFromWarehousePageViewModel(IWarehousesService warehousesService)
    {
        _warehousesService = warehousesService;
        _ = LoadAsync();
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Warehouses.Clear();

            var list = await _warehousesService.GetListAsync();
            foreach (var w in list ?? Array.Empty<WarehouseListItemResponse>())
            {
                if (WarehouseId is not null && w.Id == WarehouseId)
                    continue; // exclude source warehouse

                Warehouses.Add(w);
            }

            SelectedWarehouse ??= Warehouses.FirstOrDefault();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task TransferAsync()
    {
        if (IsBusy)
            return;

        if (WarehouseId is null)
            return;

        if (SelectedWarehouse is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Выберите склад назначения", "OK");
            return;
        }

        if (!ValidateQuantity(out var parsedQty))
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
                await Shell.Current.DisplayAlertAsync("Ошибка", ErrorMessage, "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (IsProduct)
            {
                if (parsedQty % 1 != 0)
                {
                    await Shell.Current.DisplayAlertAsync("Ошибка", "Для товаров количество должно быть целым", "OK");
                    return;
                }

                var req = new TransferProductsRequest(
                    WarehouseId.Value,
                    SelectedWarehouse.Id,
                    new[] { new TransferProductItemRequest(ProductId ?? Guid.Empty, (int)parsedQty) });

                await _warehousesService.TransferProductsAsync(req);
            }
            else
            {
                var req = new TransferMaterialsRequest(
                    WarehouseId.Value,
                    SelectedWarehouse.Id,
                    new[] { new TransferMaterialItemRequest(MaterialId ?? Guid.Empty, parsedQty) });

                await _warehousesService.TransferMaterialsAsync(req);
            }

            await Shell.Current.DisplayAlertAsync("Готово", "Перемещение выполнено", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    private bool ValidateQuantity(out decimal parsed)
    {
        parsed = 0;
        QuantityColor = Colors.Black;
        ErrorMessage = null;

        if (string.IsNullOrWhiteSpace(QuantityInput) || !decimal.TryParse(QuantityInput, out parsed))
        {
            QuantityColor = Colors.Red;
            ErrorMessage = "Введите количество";
            return false;
        }

        if (parsed <= 0 || parsed > AvailableQty)
        {
            QuantityColor = Colors.Red;
            ErrorMessage = "Количество должно быть больше 0 и не превышать остаток";
            return false;
        }

        ErrorMessage = null;
        return true;
    }
}
