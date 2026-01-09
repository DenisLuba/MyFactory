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

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

[QueryProperty(nameof(PurchaseOrderIdParameter), "PurchaseOrderId")]
public partial class SupplierOrderCompletePageViewModel : ObservableObject
{
    private readonly IMaterialPurchaseOrdersService _ordersService;
    private readonly IWarehousesService _warehousesService;
    private readonly IMaterialsService _materialsService;

    [ObservableProperty]
    private Guid? purchaseOrderId;

    [ObservableProperty]
    private string? purchaseOrderIdParameter;

    [ObservableProperty]
    private string? materialType;

    [ObservableProperty]
    private string? materialName;

    [ObservableProperty]
    private decimal totalQty;

    [ObservableProperty]
    private string? unitCode;

    [ObservableProperty]
    private decimal remainingQty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private Color remainingColor = Colors.Black;

    public ObservableCollection<WarehouseListItemResponse> Warehouses { get; } = new();
    public ObservableCollection<ReceiveLineViewModel> Lines { get; } = new();

    public SupplierOrderCompletePageViewModel(
        IMaterialPurchaseOrdersService ordersService,
        IWarehousesService warehousesService,
        IMaterialsService materialsService)
    {
        _ordersService = ordersService;
        _warehousesService = warehousesService;
        _materialsService = materialsService;
        _ = LoadAsync();
    }

    partial void OnPurchaseOrderIdParameterChanged(string? value)
    {
        PurchaseOrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    private async Task LoadAsync()
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
            Lines.Clear();

            var whs = await _warehousesService.GetListAsync();
            foreach (var w in whs ?? Array.Empty<WarehouseListItemResponse>())
                Warehouses.Add(w);

            var details = await _ordersService.GetDetailsAsync(PurchaseOrderId.Value);
            if (details?.Items.Any() != true)
                return;

            var first = details.Items.First();
            var materials = await _materialsService.GetListAsync();
            var materialInfo = materials?.FirstOrDefault(m => m.Id == first.MaterialId);

            MaterialType = materialInfo?.MaterialType ?? string.Empty;
            MaterialName = first.MaterialName;
            UnitCode = first.UnitCode;
            TotalQty = first.Qty;
            RemainingQty = TotalQty;
            UpdateRemainingColor();

            Lines.Add(new ReceiveLineViewModel(this)
            {
                Warehouse = Warehouses.FirstOrDefault(),
                Qty = 0
            });
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

    internal void Recalculate()
    {
        var allocated = Lines.Sum(l => l.Qty);
        RemainingQty = TotalQty - allocated;
        UpdateRemainingColor();
    }

    private void UpdateRemainingColor()
    {
        RemainingColor = RemainingQty < 0 ? Colors.Red : Colors.Black;
    }

    [RelayCommand]
    private void AddLine()
    {
        Lines.Add(new ReceiveLineViewModel(this)
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

        var distinctWh = Lines.Where(l => l.Warehouse is not null && l.Qty > 0).Select(l => l.Warehouse!.Id).Distinct().ToList();
        if (distinctWh.Count != 1)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Разнесение по нескольким складам пока не поддерживается. Выберите один склад.", "OK");
            return;
        }

        var targetWh = distinctWh.First();

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await _ordersService.ReceiveAsync(PurchaseOrderId.Value, new ReceiveMaterialPurchaseOrderRequest(targetWh, Guid.Empty));
            await Shell.Current.DisplayAlert("Готово", "Заказ завершен", "OK");
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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public partial class ReceiveLineViewModel : ObservableObject
    {
        private readonly SupplierOrderCompletePageViewModel _parent;

        public ReceiveLineViewModel(SupplierOrderCompletePageViewModel parent)
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

