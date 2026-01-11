using System.Collections.ObjectModel;
using System.Linq;
using System;
using Microsoft.Maui.Graphics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Warehouses;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Auth;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

[QueryProperty(nameof(PurchaseOrderIdParameter), "PurchaseOrderId")]
public partial class SupplierOrderCompletePageViewModel : ObservableObject
{
    private readonly IMaterialPurchaseOrdersService _ordersService;
    private readonly IWarehousesService _warehousesService;
    private readonly IMaterialsService _materialsService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private Guid? purchaseOrderId;

    [ObservableProperty]
    private Guid? supplierId;

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
        _ = LoadAsync();
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

        if (PurchaseOrderId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Warehouses.Clear();
            Items.Clear();

            var whs = await _warehousesService.GetListAsync();
            foreach (var w in whs ?? Array.Empty<WarehouseListItemResponse>())
                Warehouses.Add(w);

            var details = await _ordersService.GetDetailsAsync(PurchaseOrderId.Value);
            if (details?.Items.Any() != true)
                return;

            SupplierId = details.SupplierId;

            var materials = await _materialsService.GetListAsync();

            foreach (var item in details.Items)
            {
                var materialInfo = materials?.FirstOrDefault(m => m.Id == item.MaterialId);
                var vm = new PurchaseOrderItemViewModel(this)
                {
                    ItemId = item.Id,
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
    private void AddLine(PurchaseOrderItemViewModel? item)
    {
        if (item is null)
            return;

        item.Lines.Add(new ReceiveLineViewModel(item)
        {
            Warehouse = Warehouses.FirstOrDefault(),
            Qty = 0
        });
    }

    [RelayCommand]
    private async Task CompleteAsync()
    {
        if (PurchaseOrderId is null)
            return;

        if (Items.Count == 0)
            return;

        var itemsWithAllocations = new List<ReceiveMaterialPurchaseOrderItemRequest>();

        foreach (var item in Items)
        {
            if (item.RemainingQty != 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Распределите все количества по складам", "OK");
                return;
            }

            var allocations = item.Lines
                .Where(l => l.Warehouse is not null && l.Qty > 0)
                .Select(l => new ReceiveMaterialPurchaseOrderAllocationRequest(l.Warehouse!.Id, l.Qty))
                .ToList();

            if (allocations.Count == 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Укажите распределение по складам", "OK");
                return;
            }

            itemsWithAllocations.Add(new ReceiveMaterialPurchaseOrderItemRequest(item.ItemId, allocations));
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var receiverId = CurrentUserId ?? _authService.CurrentUserId ?? Guid.NewGuid();

            var request = new ReceiveMaterialPurchaseOrderRequest(
                ReceivedByUserId: receiverId,
                Items: itemsWithAllocations);

            await _ordersService.ReceiveAsync(PurchaseOrderId.Value, request);
            await Shell.Current.DisplayAlert("Готово", "Заказ завершен", "OK");
            //if (SupplierId is not null)
            //{
            //    var parameters = new Dictionary<string, object>
            //    {
            //        { "SupplierId", SupplierId.Value.ToString() }
            //    };
            //    await Shell.Current.GoToAsync(nameof(SupplierDetailsPage), parameters);
            //}
            //else 
            //{
                await Shell.Current.GoToAsync("../..", true);
            //}
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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
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
}

