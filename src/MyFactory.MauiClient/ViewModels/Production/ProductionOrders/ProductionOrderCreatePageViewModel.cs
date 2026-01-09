using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Pages.Production;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.SalesOrders;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

[QueryProperty(nameof(ProductionOrderIdParameter), "ProductionOrderId")]
public partial class ProductionOrderCreatePageViewModel : ObservableObject
{
    private readonly IProductionOrdersService _productionOrdersService;
    private readonly ISalesOrdersService _salesOrdersService;
    private readonly IProductsService _productsService;
    private readonly IDepartmentsService _departmentsService;

    [ObservableProperty]
    private Guid? productionOrderId;

    [ObservableProperty]
    private string? productionOrderIdParameter;

    [ObservableProperty]
    private string productionOrderNumber = string.Empty;

    [ObservableProperty]
    private int quantity;

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private SalesOrderListItemResponse? selectedSalesOrder;

    [ObservableProperty]
    private ProductListItemResponse? selectedProduct;

    [ObservableProperty]
    private DepartmentListItemResponse? selectedDepartment;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<SalesOrderListItemResponse> SalesOrders { get; } = new();
    public ObservableCollection<ProductListItemResponse> Products { get; } = new();
    public ObservableCollection<DepartmentListItemResponse> Departments { get; } = new();
    public ObservableCollection<MaterialItemViewModel> Materials { get; } = new();
    public ObservableCollection<ShipmentItemViewModel> Shipments { get; } = new();

    public ProductionOrderCreatePageViewModel(
        IProductionOrdersService productionOrdersService,
        ISalesOrdersService salesOrdersService,
        IProductsService productsService,
        IDepartmentsService departmentsService)
    {
        _productionOrdersService = productionOrdersService;
        _salesOrdersService = salesOrdersService;
        _productsService = productsService;
        _departmentsService = departmentsService;
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnProductionOrderIdParameterChanged(string? value)
    {
        ProductionOrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            SalesOrders.Clear();
            Products.Clear();
            Departments.Clear();
            Materials.Clear();
            Shipments.Clear();

            var salesOrders = await _salesOrdersService.GetListAsync();
            foreach (var so in salesOrders ?? Array.Empty<SalesOrderListItemResponse>())
                SalesOrders.Add(so);

            var products = await _productsService.GetListAsync();
            foreach (var p in products ?? Array.Empty<ProductListItemResponse>())
                Products.Add(p);

            var departments = await _departmentsService.GetListAsync();
            foreach (var d in departments ?? Array.Empty<DepartmentListItemResponse>())
                Departments.Add(d);

            if (ProductionOrderId is null)
            {
                ProductionOrderNumber = string.Empty;
                Status = "NEW";
                Quantity = 0;
                return;
            }

            var details = await _productionOrdersService.GetDetailsAsync(ProductionOrderId.Value);
            if (details is not null)
            {
                ProductionOrderNumber = details.ProductionOrderNumber;
                Quantity = details.QtyPlanned;
                Status = details.Status.ToString();
                SelectedDepartment = Departments.FirstOrDefault(x => x.Id == details.DepartmentId);
                SelectedSalesOrder = SalesOrders.FirstOrDefault();
                SelectedProduct = Products.FirstOrDefault();
            }

            var materials = await _productionOrdersService.GetMaterialsAsync(ProductionOrderId.Value);
            foreach (var m in materials ?? Array.Empty<ProductionOrderMaterialResponse>())
            {
                Materials.Add(new MaterialItemViewModel(m));
            }

            var shipments = await _productionOrdersService.GetShipmentsAsync(ProductionOrderId.Value);
            foreach (var s in shipments ?? Array.Empty<ProductionOrderShipmentResponse>())
            {
                Shipments.Add(new ShipmentItemViewModel(s));
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
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private async Task StagesAsync()
    {
        if (ProductionOrderId is null)
        {
            await Shell.Current.DisplayAlert("Инфо", "Сначала сохраните производственный заказ", "OK");
            return;
        }

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", ProductionOrderId.Value.ToString() },
            { "ProductionOrderNumber", ProductionOrderNumber },
            { "ProductInfo", SelectedProduct?.Name ?? string.Empty }
        };
        await Shell.Current.GoToAsync(nameof(ProductionStagesPage), parameters);
    }

    [RelayCommand]
    private async Task StartProductionAsync()
    {
        if (ProductionOrderId is null)
        {
            await Shell.Current.DisplayAlert("Инфо", "Сначала сохраните производственный заказ", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            await _productionOrdersService.StartStageAsync(ProductionOrderId.Value, new StartProductionStageRequest(ProductionOrderStatus.Cutting));
            await LoadAsync();
            await Shell.Current.DisplayAlert("Готово", "Производство запущено", "OK");
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
    private async Task CreatePurchaseAsync(MaterialItemViewModel? material)
    {
        if (material is null)
            return;

        await Shell.Current.DisplayAlert("Инфо", "Создание закупки пока не реализовано", "OK");
    }

    [RelayCommand]
    private async Task ConsumeAsync(MaterialItemViewModel? material)
    {
        if (ProductionOrderId is null || material is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", ProductionOrderId.Value.ToString() },
            { "MaterialId", material.MaterialId.ToString() },
            { "MaterialName", material.Name }
        };
        await Shell.Current.GoToAsync(nameof(MaterialConsumptionPage), parameters);
    }

    public sealed class MaterialItemViewModel
    {
        public Guid MaterialId { get; }
        public string Name { get; }
        public string Required { get; }
        public string Available { get; }
        public string Missing { get; }

        public MaterialItemViewModel(ProductionOrderMaterialResponse response)
        {
            MaterialId = response.MaterialId;
            Name = response.MaterialName;
            Required = response.RequiredQty.ToString();
            Available = response.AvailableQty.ToString();
            Missing = response.MissingQty.ToString();
        }
    }

    public sealed class ShipmentItemViewModel
    {
        public Guid WarehouseId { get; }
        public string WarehouseName { get; }
        public string Quantity { get; }
        public string Date { get; }

        public ShipmentItemViewModel(ProductionOrderShipmentResponse response)
        {
            WarehouseId = response.WarehouseId;
            WarehouseName = response.WarehouseName;
            Quantity = response.Qty.ToString();
            Date = response.ShipmentDate.ToShortDateString();
        }
    }
}

