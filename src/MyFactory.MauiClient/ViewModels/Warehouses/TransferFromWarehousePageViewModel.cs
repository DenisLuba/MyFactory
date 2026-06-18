using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    [ObservableProperty]
    private Guid? warehouseId;

    [ObservableProperty]
    private string? warehouseIdParameter;

    [ObservableProperty]
    private string? warehouseNameParameter;

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private Guid? productId;

    [ObservableProperty]
    private string? productIdParameter;

    [ObservableProperty]
    private string? itemName;

    [ObservableProperty]
    private string? unitCode;

    [ObservableProperty]
    private decimal availableQty;

    [ObservableProperty]
    private string? availableQtyParameter;

    [ObservableProperty]
    private string? quantityInput;

    [ObservableProperty]
    private Color quantityColor = Colors.Black;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private WarehouseListItemResponse? selectedWarehouse;

    [ObservableProperty]
    private string? itemType;

    public ObservableCollection<WarehouseListItemResponse> Warehouses { get; } = new();

    public string? SourceWarehouseName => WarehouseNameParameter;

    public bool IsProduct => ProductId is not null;

    public TransferFromWarehousePageViewModel(IWarehousesService warehousesService)
    {
        _warehousesService = warehousesService;
    }

    partial void OnAvailableQtyParameterChanged(string? value)
    {
        AvailableQty = decimal.TryParse(value, out var qty) ? qty : 0;
    }
    partial void OnQuantityInputChanged(string? value)
    {
        ValidateQuantity(out _);
    }
    partial void OnProductIdParameterChanged(string? value)
    {
        if (Guid.TryParse(value, out var id))
        {
            ProductId = id;
            MaterialId = null; // ensure only one of them is set
            ItemType = "Товар";
        }
    }
    partial void OnMaterialIdParameterChanged(string? value)
    {
        if (Guid.TryParse(value, out var id))
        {
            MaterialId = id;
            ProductId = null; // ensure only one of them is set
            ItemType = "Материал";
        }
    }
    partial void OnWarehouseIdParameterChanged(string? value)
    {
        WarehouseId = Guid.TryParse(value, out var id) ? id : null;
    }
    partial void OnWarehouseNameParameterChanged(string? value)
    {
        OnPropertyChanged(nameof(SourceWarehouseName));
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(UnitCode))
            UnitCode = "шт."; // default unit

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Warehouses.Clear();

            var list = await _warehousesService.GetListAsync();
            foreach (var w in list ?? [])
            {
                if (WarehouseId is not null && w.Id == WarehouseId)
                    continue; // exclude source warehouse
                
                if (MaterialId is not null && w.Type is WarehouseType.Materials or WarehouseType.Aux)
                    Warehouses.Add(w);

                if (ProductId is not null && w.Type is WarehouseType.FinishedGoods)
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
