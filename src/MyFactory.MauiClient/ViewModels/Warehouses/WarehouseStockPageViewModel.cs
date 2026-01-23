using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.Warehouses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MyFactory.MauiClient.ViewModels.Warehouses;

[QueryProperty(nameof(WarehouseIdParameter), "WarehouseId")]
[QueryProperty(nameof(WarehouseName), "WarehouseName")]
public partial class WarehouseStockPageViewModel : ObservableObject
{
    private readonly IWarehousesService _warehousesService;
    private readonly IMaterialsService _materialsService;
    private readonly IProductsService _productsService;

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

    [ObservableProperty]
    private int itemQty;

    [ObservableProperty]
    private string? units;

    [ObservableProperty]
    private bool isEditMode = false;

    [ObservableProperty]
    private bool isViewMode = true;

    [ObservableProperty]
    private StockItemViewModel? item;

    public ICollection<StockItemViewModel> ProductItems { get; } = [];

    public ICollection<StockItemViewModel> MaterialItems { get; } = [];


    public ObservableCollection<StockItemViewModel> Items { get; } = [];

    public ObservableCollection<StockItemViewModel> StockItems { get; } = [];

    public WarehouseStockPageViewModel(IWarehousesService warehousesService, IMaterialsService materialsService, IProductsService productsService)
    {
        _warehousesService = warehousesService;
        _materialsService = materialsService;
        _productsService = productsService;
        _ = LoadAsync();
    }

    partial void OnIsEditModeChanged(bool value)
    {
        IsViewMode = !IsEditMode;
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
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
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
        var isAgreed = await Shell.Current.DisplayAlertAsync(
            "Добавить новый элемент на склад?",
            "Вы уверены, что хотите добавить новый элемент на склад?",
            "Да",
            "Нет");
        if (!isAgreed)
            return;

        try
        {
            await EnsureItemsForWarehouseTypeAsync();
            IsEditMode = true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (Item is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите элемент для добавления.", "Ок");
            return;
        }
            
        if (ItemQty <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Количество не может быть меньше нуля или равным нулю.", "Ок");
            return;
        }
        IsBusy = true;
        try
        {
            if (WarehouseId is Guid id)
            {
                IsEditMode = false;
                if (WarehouseType == WarehouseType.FinishedGoods)
                {
                    var productRequest = new AddProductToWarehouseRequest(Item.Id, ItemQty);
                    await _warehousesService.AddProductAsync(id, productRequest);
                }
                else
                {
                    var materialRequest = new AddMaterialToWarehouseRequest(Item.Id, ItemQty);
                    await _warehousesService.AddMaterialAsync(id, materialRequest);
                }
                IsBusy = false;
                await LoadAsync();
            }
            else throw new NullReferenceException("WarehouseId cannot be null.");
        }
        catch(Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "Ok");
        }
        finally
        {
            IsBusy = false;
        }
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

        if (item.Id == Guid.Empty)
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

    private async Task EnsureItemsForWarehouseTypeAsync()
    {
        Items.Clear();

        if (WarehouseType == WarehouseType.FinishedGoods)
        {
            if (ProductItems.Count == 0)
                await LoadProductsAsync();
            else
            {
                foreach (var product in ProductItems)
                    Items.Add(product);
                SetFirstItem();
            }
        }
        else
        {
            if (MaterialItems.Count == 0)
                await LoadMaterialsAsync();
            else
            {
                foreach (var material in MaterialItems)
                    Items.Add(material);
                SetFirstItem();
            }
        }
    }

    private async Task LoadProductsAsync()
    {
        var products = await _productsService.GetListAsync();
        if (products is null || products.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Информация", "Нет доступных товаров.", "OK");
            return;
        }
        Items.Clear();
        foreach (var product in products)
        {
            var stockItem = new StockItemViewModel(
                product.Id,
                product.Name,
                0,
                null,
                isProduct: true);
            ProductItems.Add(stockItem);
            Items.Add(stockItem);
        }
        SetFirstItem();
    }

    private async Task LoadMaterialsAsync()
    {
        var materials = await _materialsService.GetListAsync(isActive: true);
        if (materials is null || materials.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Информация", "Нет доступных материалов.", "OK");
            return;
        }
        Items.Clear();
        foreach (var material in materials)
        {
            var stockItem = new StockItemViewModel(
                material.Id,
                material.Name,
                0,
                material.UnitCode,
                isProduct: false);
            MaterialItems.Add(stockItem);
            Items.Add(stockItem);
        }
        SetFirstItem();
    }

    private void SetFirstItem()
    {
        Item = Items.FirstOrDefault();
        Units = Item?.UnitCode;
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

        public StockItemViewModel(Guid id, string name, decimal qty, string? unitCode, bool isProduct)
        {
            Id = id;
            Name = name;
            Qty = qty;
            UnitCode = unitCode;
            Quantity = unitCode is string unit && !string.IsNullOrWhiteSpace(unit)
                ? $"{qty} {unit}"
                : qty.ToString();
            IsProduct = isProduct;
        }
    }
}

