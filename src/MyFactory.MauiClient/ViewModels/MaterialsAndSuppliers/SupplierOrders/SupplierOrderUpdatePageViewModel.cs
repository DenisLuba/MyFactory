using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Suppliers;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

[QueryProperty(nameof(SupplierIdParameter), "SupplierId")]
[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
[QueryProperty(nameof(PurchaseOrderIdParameter), "PurchaseOrderId")]
public partial class SupplierOrderUpdatePageViewModel : ObservableObject
{
    private readonly IMaterialPurchaseOrdersService _ordersService;
    private readonly ISuppliersService _suppliersService;
    private readonly IMaterialsService _materialsService;

    [ObservableProperty]
    private Guid? supplierId;

    [ObservableProperty]
    private string? supplierIdParameter;

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private Guid? purchaseOrderId;

    [ObservableProperty]
    private string? purchaseOrderIdParameter;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool hasSelectedItems;

    [ObservableProperty]
    private ObservableCollection<object?> selectedItems = [];

    partial void OnSelectedItemsChanged(ObservableCollection<object?> selectedItems)
    {
        HasSelected = !HasSelected;
    }

    public ObservableCollection<SupplierListItemResponse> SupplierOptions { get; } = new();
    public ObservableCollection<string> MaterialTypeOptions { get; } = new();
    public ObservableCollection<MaterialListItemResponse> MaterialOptions { get; } = new();
    public ObservableCollection<OrderItemViewModel> Items { get; } = new();

    private List<MaterialListItemResponse> _allMaterials = new();
    private List<SupplierListItemResponse> _allSuppliers = new();

    public SupplierOrderUpdatePageViewModel(
        IMaterialPurchaseOrdersService ordersService,
        ISuppliersService suppliersService,
        IMaterialsService materialsService)
    {
        _ordersService = ordersService;
        _suppliersService = suppliersService;
        _materialsService = materialsService;
        _ = LoadAsync();
    }

    partial void OnSupplierIdChanged(Guid? value)
    {
        if (value is not null)
        {
            foreach (var item in Items)
            {
                item.Supplier = _allSuppliers.FirstOrDefault(s => s.Id == value.Value) ?? item.Supplier;
            }
        }
    }

    partial void OnSupplierIdParameterChanged(string? value)
    {
        SupplierId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        if (value is not null)
        {
            var material = _allMaterials.FirstOrDefault(m => m.Id == value.Value);
            if (material is not null && Items.Count == 0)
            {
                Items.Add(new OrderItemViewModel(this)
                {
                    Supplier = _allSuppliers.FirstOrDefault(s => s.Id == SupplierId) ?? _allSuppliers.FirstOrDefault(),
                    MaterialType = material.MaterialType,
                    Material = material,
                    Qty = 1,
                    Price = 0
                });
            }
        }
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnPurchaseOrderIdParameterChanged(string? value)
    {
        PurchaseOrderId = Guid.TryParse(value, out var id) ? id : null;
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

            SupplierOptions.Clear();
            MaterialOptions.Clear();
            MaterialTypeOptions.Clear();
            Items.Clear();

            var suppliers = await _suppliersService.GetListAsync();
            _allSuppliers = suppliers?.ToList() ?? new();
            foreach (var s in _allSuppliers)
                SupplierOptions.Add(s);

            var materials = await _materialsService.GetListAsync();
            _allMaterials = materials?.ToList() ?? new();
            foreach (var m in _allMaterials)
                MaterialOptions.Add(m);

            foreach (var t in _allMaterials.Select(m => m.MaterialType).Distinct().OrderBy(t => t))
                MaterialTypeOptions.Add(t);

            if (PurchaseOrderId is not null)
            {
                var details = await _ordersService.GetDetailsAsync(PurchaseOrderId.Value);
                if (details is not null)
                {
                    SupplierId = details.SupplierId;

                    foreach (var item in details.Items)
                    {
                        var material = _allMaterials.FirstOrDefault(m => m.Id == item.MaterialId);
                        Items.Add(new OrderItemViewModel(this)
                        {
                            Id = item.Id,
                            Supplier = _allSuppliers.FirstOrDefault(s => s.Id == details.SupplierId),
                            MaterialType = material?.MaterialType ?? string.Empty,
                            Material = material,
                            Qty = item.Qty,
                            Price = item.UnitPrice
                        });
                    }
                }
            }

            if (Items.Count == 0)
            {
                Items.Add(new OrderItemViewModel(this)
                {
                    Supplier = SupplierOptions.FirstOrDefault(s => s.Id == SupplierId) ?? SupplierOptions.FirstOrDefault(),
                    MaterialType = MaterialTypeOptions.FirstOrDefault(),
                    Material = MaterialOptions.FirstOrDefault(),
                    Qty = 1,
                    Price = 0
                });
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private Task AddItemAsync()
    {
        Items.Add(new OrderItemViewModel(this)
        {
            Supplier = SupplierOptions.FirstOrDefault(s => s.Id == SupplierId) ?? SupplierOptions.FirstOrDefault(),
            MaterialType = MaterialTypeOptions.FirstOrDefault(),
            Material = MaterialOptions.FirstOrDefault(),
            Qty = 1,
            Price = 0
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (Items.Count == 0)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Добавьте хотя бы одну позицию", "OK");
            return;
        }

        var supplier = Items.First().Supplier;
        if (supplier is null)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Выберите поставщика", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Guid orderId;
            if (PurchaseOrderId is null)
            {
                var createResponse = await _ordersService.CreateAsync(new CreateMaterialPurchaseOrderRequest(supplier.Id, DateTime.UtcNow));
                if (createResponse is null)
                    throw new InvalidOperationException("Не удалось создать заказ");
                orderId = createResponse.Id;
                PurchaseOrderId = orderId;
            }
            else
            {
                orderId = PurchaseOrderId.Value;
            }

            foreach (var item in Items)
            {
                if (item.Material is null)
                    continue;

                if (item.Id.HasValue)
                {
                    await _ordersService.UpdateItemAsync(item.Id.Value, new UpdateMaterialPurchaseOrderItemRequest(item.Qty, item.Price));
                }
                else
                {
                    var request = new AddMaterialPurchaseOrderItemRequest(item.Material.Id, item.Qty, item.Price);
                    await _ordersService.AddItemAsync(orderId, request);
                }
            }

            await _ordersService.ConfirmAsync(orderId);
            await Shell.Current.DisplayAlert("Успех", "Заказ сохранен", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeleteOrderAsync()
    {
        if (PurchaseOrderId is null)
            return;

        var confirm = await Shell.Current.DisplayAlert("Удалить", "Удалить заказ?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            await _ordersService.CancelAsync(PurchaseOrderId.Value);
            await Shell.Current.DisplayAlert("Готово", "Заказ удален", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CompleteAsync()
    {
        if (PurchaseOrderId is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderCompletePage", new Dictionary<string, object>
        {
            { "PurchaseOrderId", PurchaseOrderId.Value.ToString() }
        });
    }

    [RelayCommand]
    private async Task PrintAsync()
    {
        await Shell.Current.DisplayAlert("Печать", "Функция печати недоступна", "OK");
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public partial class OrderItemViewModel : ObservableObject
    {
        private readonly SupplierOrderUpdatePageViewModel _parent;

        public OrderItemViewModel(SupplierOrderUpdatePageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty]
        private Guid? id;

        [ObservableProperty]
        private SupplierListItemResponse? supplier;

        [ObservableProperty]
        private string? materialType;

        [ObservableProperty]
        private MaterialListItemResponse? material;

        [ObservableProperty]
        private decimal qty;

        [ObservableProperty]
        private decimal price;

        partial void OnMaterialTypeChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var material = _parent._allMaterials.FirstOrDefault(m => m.MaterialType.Equals(value, StringComparison.OrdinalIgnoreCase));
            if (material is not null)
            {
                Material = material;
            }
        }
    }
}