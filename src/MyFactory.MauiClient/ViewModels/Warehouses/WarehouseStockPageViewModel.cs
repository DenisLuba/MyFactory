using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Services.Warehouses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyFactory.MauiClient.ViewModels.Warehouses;

[QueryProperty(nameof(WarehouseIdParameter), "WarehouseId")]
[QueryProperty(nameof(WarehouseName), "WarehouseName")]
public partial class WarehouseStockPageViewModel : ObservableObject
{
    private readonly IWarehousesService _warehousesService;

    [ObservableProperty]
    private WarehouseType warehouseType;

    [ObservableProperty]
    private Guid? warehouseId;

    [ObservableProperty]
    private string? warehouseIdParameter;

    [ObservableProperty]
    private string? warehouseName;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<StockItemViewModel> StockItems { get; } = new();

    public WarehouseStockPageViewModel(IWarehousesService warehousesService)
    {
        _warehousesService = warehousesService;
        _ = LoadAsync();
    }

    partial void OnWarehouseIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnWarehouseIdParameterChanged(string? value)
    {
        WarehouseId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (WarehouseId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            StockItems.Clear();

            var info = await _warehousesService.GetInfoAsync(WarehouseId.Value);
            if (info is not null)
            {
                WarehouseName = info.Name;
                WarehouseType = info.Type;
            }

            var items = await _warehousesService.GetStockAsync(WarehouseId.Value);
            foreach (var item in items ?? Enumerable.Empty<WarehouseStockItemResponse>())
            {
                StockItems.Add(new StockItemViewModel(item, WarehouseType));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("������", ex.Message, "OK");
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

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await Shell.Current.DisplayAlertAsync("��������", "���������� ������� �� �����������", "OK");
    }

    [RelayCommand]
        private async Task TransferAsync(StockItemViewModel? item)
        {
            if (item is null || WarehouseId is null)
                return;

            var parameters = new Dictionary<string, object?>
            {
                { "WarehouseId", WarehouseId.Value.ToString() },
                { "WarehouseName", WarehouseName },
                { "ItemName", item.Name },
                { "UnitCode", item.UnitCode ?? string.Empty },
                { "AvailableQty", item.Qty.ToString() },
                { item.IsProduct ? "ProductId" : "MaterialId", item.Id.ToString() }
            };

            await Shell.Current.GoToAsync(nameof(Pages.Warehouses.TransferFromWarehousePage), parameters);
        }

    [RelayCommand]
    private async Task OpenItemAsync(StockItemViewModel? item)
    {
        if (item is null)
            return;
        
        if(item.Id == Guid.Empty)
            return;

        (string pageName, string idParameterName) = item.IsProduct
            ? (nameof(ProductDetailsPage), "ProductId")
            : (nameof(MaterialDetailsViewPage), "MaterialId");

        var parameters = new Dictionary<string, object?>
        {
            { idParameterName, item.Id.ToString() }
        };

        await Shell.Current.GoToAsync(pageName, parameters);
    }

    public sealed class StockItemViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Quantity { get; }
        public decimal Qty { get; }
        public string? UnitCode { get; }
        public bool IsProduct { get; }

        public StockItemViewModel(WarehouseStockItemResponse response, WarehouseType warehouseType)
        {
            Id = response.ItemId;
            Name = response.Name;
            Qty = response.Qty;
            UnitCode = response.UnitCode;
            Quantity = response.UnitCode is string unit && !string.IsNullOrWhiteSpace(unit)
                ? $"{response.Qty} {unit}"
                : response.Qty.ToString();
            IsProduct = warehouseType == WarehouseType.FinishedGoods;
        }
    }
}

