using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;
using MyFactory.MauiClient.Services.Auth;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Warehouses;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

[QueryProperty(nameof(PurchaseOrderIdParameter), "PurchaseOrderId")]
public partial class SupplierOrderCompletePageViewModel : ObservableObject
{
    private readonly IMaterialPurchaseOrdersService _ordersService;
    private readonly IWarehousesService _warehousesService;
    private readonly IMaterialsService _materialsService;
    private readonly IAuthService _authService;
    private bool _isRecalculating;

    [ObservableProperty]
    private Guid? purchaseOrderId;

    [ObservableProperty]
    private string? purchaseNumberText;

    [ObservableProperty]
    private Guid? supplierId;

    [ObservableProperty]
    private string? supplierName;

    [ObservableProperty]
    private WarehouseListItemResponse? selectedWarehouse;

    [ObservableProperty]
    private string? purchaseOrderIdParameter;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private Guid? currentUserId;

    public ObservableCollection<WarehouseListItemResponse> Warehouses { get; } = new();
    public ObservableCollection<PurchaseOrderItemViewModel> Items { get; } = new();
    public ObservableCollection<WarehouseAllocationViewModel> SelectedWarehouses { get; } = new();

    [ObservableProperty]
    private WarehouseAllocationViewModel? selectedWarehouseAllocation;

    public bool IsSelectedWarehouse => SelectedWarehouseAllocation is not null;

    partial void OnSelectedWarehouseAllocationChanged(WarehouseAllocationViewModel? value)
    {
        OnPropertyChanged(nameof(IsSelectedWarehouse));
    }

    public SupplierOrderCompletePageViewModel(
        IMaterialPurchaseOrdersService ordersService,
        IWarehousesService warehousesService,
        IMaterialsService materialsService,
        IAuthService authService)
    {
        _ordersService = ordersService;
        _warehousesService = warehousesService;
        _materialsService = materialsService;
        _authService = authService;
        CurrentUserId = _authService.CurrentUserId;
    }

    partial void OnPurchaseOrderIdParameterChanged(string? value)
    {
        PurchaseOrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        if (PurchaseOrderId is null)
            return;

        Warehouses.Clear();
        Items.Clear();
        SelectedWarehouses.Clear();

        var whs = await _warehousesService.GetListAsync(warehouseTypes: ["Materials", "Aux"]);
        foreach (var w in whs ?? [])
            Warehouses.Add(w);

        var details = await _ordersService.GetDetailsAsync(PurchaseOrderId.Value);
        if (details?.Items.Any() != true)
            return;

        SupplierId = details.SupplierId;
        SupplierName = details.SupplierName;
        PurchaseNumberText = details.PurchaseNumber.ToString("0");

        var materials = (await _materialsService.GetListAsync())?.Items;

        foreach (var item in details.Items)
        {
            var materialInfo = materials?.FirstOrDefault(m => m.Id == item.MaterialId);
            var vm = new PurchaseOrderItemViewModel(this)
            {
                ItemId = item.Id,
                MaterialId = item.MaterialId,
                MaterialType = materialInfo?.MaterialType ?? string.Empty,
                MaterialName = item.MaterialName,
                UnitCode = item.UnitCode,
                TotalQty = item.Qty,
                RemainingQty = item.Qty
            };

            vm.Lines.Add(new ReceiveLineViewModel(vm)
            {
                Warehouse = Warehouses.FirstOrDefault(),
                Qty = 0
            });

            Items.Add(vm);
        }

        if (Warehouses.Count > 0)
        {
            var warehouseAllocation = new WarehouseAllocationViewModel(this)
            {
                Warehouse = Warehouses.First(),
                ShippingCost = 0
            };

            SelectedWarehouses.Add(warehouseAllocation);
            SelectedWarehouseAllocation = warehouseAllocation;
        }
    });

    private List<ReceiveMaterialPurchaseOrderAllocationRequest> BuildAllocations() => [.. SelectedWarehouses
            .Where(w => w.Warehouse is not null && w.Warehouse.Id != Guid.Empty)
            .Select(w => new ReceiveMaterialPurchaseOrderAllocationRequest(
                w.Warehouse!.Id,
                w.ShippingCost,
                [.. w.Lines
                    .Where(l => l.Item is not null && l.Item.MaterialId != Guid.Empty && l.Qty > 0)
                    .Select(l => new ReceiveMaterialPurchaseOrderItemRequest(l.Item!.MaterialId, l.Qty))]
            ))];

    // [RelayCommand]
    // private async Task AddLineAsync(PurchaseOrderItemViewModel? item) => await RunSafeActionAsync(async () =>
    // {
    //     if (item is null)
    //         return;

    //     item.Lines.Add(new ReceiveLineViewModel(item)
    //     {
    //         Warehouse = Warehouses.FirstOrDefault(),
    //         Qty = 0
    //     });
    // });

    [RelayCommand]
    private async Task AddWarehouseAsync() => await RunSafeActionAsync(async () =>
    {
        var allocation = new WarehouseAllocationViewModel(this)
        {
            Warehouse = Warehouses.FirstOrDefault(),
            ShippingCost = 0
        };

        SelectedWarehouses.Insert(0, allocation);
        SelectedWarehouseAllocation = allocation;
    });

    [RelayCommand]
    private async Task RemoveWarehouseAsync() => await RunSafeActionAsync(async () =>
    {
        if (SelectedWarehouseAllocation is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление склада", "Удалить выбранный склад?", "Удалить", "Отмена");
        if (!confirm)
            return;

        SelectedWarehouses.Remove(SelectedWarehouseAllocation);
        SelectedWarehouseAllocation = null;
        RecalculateFromAllocations();
    });

    [RelayCommand]
    private async Task AddMaterialToWarehouseAsync(WarehouseAllocationViewModel? warehouse) => await RunSafeActionAsync(async () =>
    {
        var target = warehouse ?? SelectedWarehouseAllocation;
        if (target is null)
            return;

        var item = Items.FirstOrDefault(i => i.RemainingQty > 0) ?? Items.FirstOrDefault();
        if (item is null)
            return;

        var line = new MaterialAllocationViewModel(this)
        {
            Item = item,
            Qty = 0
        };

        target.Lines.Insert(0, line);
        target.SelectedMaterial = line;
        RecalculateFromAllocations();
    });

    [RelayCommand]
    private async Task RemoveMaterialFromWarehouseAsync(WarehouseAllocationViewModel? warehouse) => await RunSafeActionAsync(async () =>
    {
        var target = warehouse ?? SelectedWarehouseAllocation;
        if (target?.SelectedMaterial is null)
            return;

        var selected = target.SelectedMaterial;
        target.Lines.Remove(selected);
        target.SelectedMaterial = null;
        RecalculateFromAllocations();
    });

    private void RecalculateFromAllocations()
    {
        if (_isRecalculating)
            return;

        _isRecalculating = true;

        try
        {
            foreach (var item in Items)
            {
                var allocated = SelectedWarehouses
                    .SelectMany(w => w.Lines)
                    .Where(l => l.Item == item)
                    .Sum(l => l.Qty);

                item.RemainingQty = item.TotalQty - allocated;
                item.RemainingColor = item.RemainingQty < 0 ? Colors.Red : Colors.Black;
            }
        }
        finally
        {
            _isRecalculating = false;
        }
    }

    [RelayCommand]
    private async Task CompleteAsync() => await RunSafeActionAsync(async () =>
    {
        if (PurchaseOrderId is null)
            return;

        if (Items.Count == 0)
            return;

        RecalculateFromAllocations();

        // Ensure all items fully allocated
        foreach (var item in Items)
        {
            if (item.RemainingQty != 0)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка", "Распределите все количества по складам", "OK");
                return;
            }
        }

        var allocations = BuildAllocations();

        if (allocations.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите распределение по складам", "OK");
            return;
        }

        // Defensive validation: ensure no empty MaterialId got into allocations before sending
        var bad = allocations
            .SelectMany(a => a.MaterialItems ?? Array.Empty<ReceiveMaterialPurchaseOrderItemRequest>())
            .Where(mi => mi.MaterialId == Guid.Empty)
            .ToList();

        if (bad.Count > 0)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Найдены строки распределения без выбранного материала. Пожалуйста, выберите материал в каждой строке.", "OK");
            return;
        }
        var receiverId = CurrentUserId ?? _authService.CurrentUserId ?? throw new InvalidOperationException("Не удалось получить Id пользователя");

        var request = new ReceiveMaterialPurchaseOrderRequest(
            ReceivedByUserId: receiverId,
            Allocations: allocations);

        await _ordersService.ReceiveAsync(PurchaseOrderId.Value, request);
        await Shell.Current.DisplayAlertAsync("Готово", "Заказ завершен", "OK");
        await BackNavigationAsync();
    });

    [RelayCommand]
    private async Task BackAsync() => await BackNavigationAsync();

    private async Task BackNavigationAsync() => await Shell.Current.BackSafeAsync(
            fallbackRoute: nameof(SupplierDetailsPage),
            parameters: new Dictionary<string, object> { { "SupplierId", SupplierId?.ToString() ?? string.Empty } },
            animate: true);

    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }

    public partial class PurchaseOrderItemViewModel : ObservableObject
    {
        private readonly SupplierOrderCompletePageViewModel _parent;

        public PurchaseOrderItemViewModel(SupplierOrderCompletePageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty]
        private Guid itemId;

        [ObservableProperty]
        private Guid materialId;

        [ObservableProperty]
        private string materialType = string.Empty;

        [ObservableProperty]
        private string materialName = string.Empty;

        [ObservableProperty]
        private string unitCode = string.Empty;

        [ObservableProperty]
        private decimal totalQty;

        [ObservableProperty]
        private decimal remainingQty;

        [ObservableProperty]
        private Color remainingColor = Colors.Black;

        public ObservableCollection<ReceiveLineViewModel> Lines { get; } = new();

        internal void Recalculate()
        {
            var allocated = Lines.Sum(l => l.Qty);
            RemainingQty = TotalQty - allocated;
            RemainingColor = RemainingQty < 0 ? Colors.Red : Colors.Black;
        }
    }

    public partial class ReceiveLineViewModel : ObservableObject
    {
        private readonly PurchaseOrderItemViewModel _parent;

        public ReceiveLineViewModel(PurchaseOrderItemViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty]
        private WarehouseListItemResponse? warehouse;

        [ObservableProperty]
        private decimal qty;

        partial void OnQtyChanged(decimal value)
        {
            _parent.Recalculate();
        }

        partial void OnWarehouseChanged(WarehouseListItemResponse? value)
        {
            // no-op
        }
    }

    public partial class WarehouseAllocationViewModel : ObservableObject
    {
        private readonly SupplierOrderCompletePageViewModel _parent;

        public WarehouseAllocationViewModel(SupplierOrderCompletePageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty]
        private WarehouseListItemResponse? warehouse;

        [ObservableProperty]
        private decimal shippingCost;

        [ObservableProperty]
        private MaterialAllocationViewModel? selectedMaterial;

        public ObservableCollection<MaterialAllocationViewModel> Lines { get; } = new();

        public bool HasSelectedMaterial => SelectedMaterial is not null;

        partial void OnSelectedMaterialChanged(MaterialAllocationViewModel? value)
        {
            OnPropertyChanged(nameof(HasSelectedMaterial));
        }
    }

    public partial class MaterialAllocationViewModel : ObservableObject
    {
        private readonly SupplierOrderCompletePageViewModel _parent;
        private bool _updatingQty;

        public MaterialAllocationViewModel(SupplierOrderCompletePageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty]
        private PurchaseOrderItemViewModel? item;

        [ObservableProperty]
        private decimal qty;

        public string UnitCode => Item?.UnitCode ?? string.Empty;
        public string MaterialName => Item?.MaterialName ?? string.Empty;

        partial void OnItemChanged(PurchaseOrderItemViewModel? value)
        {
            if (_updatingQty)
                return;

            _updatingQty = true;
            Qty = 0;
            _updatingQty = false;

            OnPropertyChanged(nameof(UnitCode));
            OnPropertyChanged(nameof(MaterialName));
            _parent.RecalculateFromAllocations();
        }

        partial void OnQtyChanged(decimal value)
        {
            if (_updatingQty)
                return;

            if (Item is not null)
            {
                var allocatedWithoutThis = _parent.SelectedWarehouses
                    .SelectMany(w => w.Lines)
                    .Where(l => l.Item == Item && l != this)
                    .Sum(l => l.Qty);

                var allowed = Item.TotalQty - allocatedWithoutThis;
                var adjusted = Math.Max(0, Math.Min(value, allowed));

                if (adjusted != value)
                {
                    _updatingQty = true;
                    Qty = adjusted;
                    _updatingQty = false;
                }
            }

            _parent.RecalculateFromAllocations();
        }
    }
}

