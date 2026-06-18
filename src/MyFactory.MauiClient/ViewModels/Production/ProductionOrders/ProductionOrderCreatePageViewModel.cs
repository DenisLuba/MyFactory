using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
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
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.SupplierOrders;
using CommunityToolkit.Maui;
using MyFactory.MauiClient.Services.Materials;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

public partial class ProductionOrderCreatePageViewModel(
        IProductionOrdersService productionOrdersService,
        ISalesOrdersService salesOrdersService,
        IDepartmentsService departmentsService,
        IMaterialsService materialsService,
        IProductsService productsService,
        IPopupService popupService) : ObservableObject, IQueryAttributable
{
    #region APPLY QUERY ATTRIBUTES
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ProductionOrderId", out var productionOrderIdValue)
            && productionOrderIdValue is string productionOrderIdResult)
            ProductionOrderIdParameter = productionOrderIdResult;

        if (query.TryGetValue("Mode", out var modeValue) && modeValue is ProductionOrderCreatePageMode modeResult)
            Mode = modeResult;

        if (query.TryGetValue("SelectedSalesOrder", out var selectedSalesOrderParameterValue)
            && selectedSalesOrderParameterValue is SalesOrderDetailsResponse selectedSalesOrderParameterResult)
            SelectedSalesOrderParameter = selectedSalesOrderParameterResult;

        if (query.TryGetValue("SelectedSalesOrderItem", out var selectedSalesOrderItemParameterValue)
            && selectedSalesOrderItemParameterValue is SalesOrderItemResponse selectedSalesOrderItemParameterResult)
            SelectedProductParameter = selectedSalesOrderItemParameterResult;

        if (query.TryGetValue("Qty", out var qtyValue) && qtyValue is decimal qty)
            QtyParameter = qty;

        if (query.TryGetValue("SelectedDepartment", out var selectedDepartmentParameterValue)
            && selectedDepartmentParameterValue is DepartmentListItemResponse selectedDepartmentParameterResult)
            SelectedDepartmentParameter = selectedDepartmentParameterResult;
    }
    #endregion

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? productionOrderId;
    [ObservableProperty] private string? productionOrderIdParameter;
    [ObservableProperty] private ProductionOrderCreatePageMode mode;
    [ObservableProperty] private ProductionOrderStatus? orderStatus = null;
    [ObservableProperty] private string status = string.Empty;
    [ObservableProperty] private int? productionOrderNumber;
    [ObservableProperty] private decimal quantity = 0;
    [ObservableProperty] private decimal? qtyParameter;
    [ObservableProperty] private SalesOrderDetailsResponse? selectedSalesOrder;
    [ObservableProperty] private SalesOrderDetailsResponse? selectedSalesOrderParameter;
    [ObservableProperty] private SalesOrderItemResponse? selectedProduct = null;
    [ObservableProperty] private SalesOrderItemResponse? selectedProductParameter;
    [ObservableProperty] private DepartmentListItemResponse? selectedDepartment;
    [ObservableProperty] private DepartmentListItemResponse? selectedDepartmentParameter;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    #endregion

    #region PROPERTIES
    public bool IsEditMode => Mode == ProductionOrderCreatePageMode.Edit;
    public bool IsCreateMode => Mode == ProductionOrderCreatePageMode.Create;
    public bool IsViewMode => Mode == ProductionOrderCreatePageMode.View;
    public int MaxQty;
    public decimal _initialQtyPlanned = 0; // для режима EditMode, чтобы это значение можно было впоследствии изменить
    public bool CanIssueMaterials { get; set; }
    #endregion

    #region PRIVATE VARIABLES
    bool _isAdjustingQuantity;
    //decimal _lastQuantity;
    #endregion

    #region OBSERVABLE COLLECTIONS
    public ObservableCollection<SalesOrderItemResponse> Products { get; } = new();
    public ObservableCollection<DepartmentListItemResponse> Departments { get; } = new();
    public ObservableCollection<MaterialItemViewModel> Materials { get; } = new();
    public ObservableCollection<ShipmentItemViewModel> Shipments { get; } = new();
    #endregion

    #region ON CHANGED METHODS
    partial void OnProductionOrderIdChanged(Guid? value)
    {
        if (value is null)
            return;

        if (!IsBusy)
            _ = LoadAsync();
    }

    partial void OnProductionOrderIdParameterChanged(string? value)
    {
        ProductionOrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnModeChanged(ProductionOrderCreatePageMode value)
    {
        OnPropertyChanged(nameof(IsEditMode));
        OnPropertyChanged(nameof(IsCreateMode));
        OnPropertyChanged(nameof(IsViewMode));
    }

    //partial void OnQuantityChanging(decimal oldValue, decimal newValue)
    //{
    //    if (_isAdjustingQuantity)
    //        return;
    //    _lastQuantity = oldValue; // запомним для спецификации
    //}

    partial void OnQuantityChanged(decimal value)
    {
        if (SelectedProduct is null)
            return;

        if (_isAdjustingQuantity)
            return;

        var ordered = SelectedProduct?.QtyOrdered ?? 0;
        var allocated = SelectedProduct?.QtyAllocated ?? 0;
        var currentQty = IsEditMode ? _initialQtyPlanned : 0;

        var max = ordered - allocated + currentQty;

        var clamped = value > max ? max : value;

        if (clamped != value)
        {
            try
            {
                _isAdjustingQuantity = true;
                Quantity = clamped; // обновим Entry
            }
            finally
            {
                _isAdjustingQuantity = false;
            }
        }

        _ = SetRequiredMaterialsQty(clamped);
    }

    partial void OnSelectedProductChanged(SalesOrderItemResponse? value)
    {
        if (IsEditMode && QtyParameter is null)
            Quantity = 0;
        if (value is null)
            return;

        _ = SetSpecification();
    }

    partial void OnSelectedSalesOrderParameterChanged(SalesOrderDetailsResponse? value)
    {
        if (value is not null)
            SelectedSalesOrder = value;
    }

    partial void OnSelectedProductParameterChanged(SalesOrderItemResponse? value)
    {
        if (value is not null)
            SelectedProduct = value;
    }

    partial void OnQtyParameterChanged(decimal? value)
    {
        if (value is not null)
            Quantity = value.Value;
    }

    partial void OnSelectedDepartmentParameterChanged(DepartmentListItemResponse? value)
    {
        if (value is not null)
            SelectedDepartment = value;
    }
    #endregion

    #region LOAD DATA
    private async Task LoadDataAsync()
    {
        Departments.Clear();
        Materials.Clear();
        Shipments.Clear();

        ProductionOrderDetailsResponse? details = null;
        SalesOrderItemResponse? salesOrderItemResponse = null;

        if (ProductionOrderId is not null)
        {
            details = await productionOrdersService.GetDetailsAsync(ProductionOrderId.Value);

            if (details is not null)
            {
                await SetDetailsByProductionOrderId(details);

                // заказ
                SelectedSalesOrder = details.SalesOrderId == Guid.Empty
                    ? null
                    : await salesOrdersService.GetDetailsAsync(details.SalesOrderId);

                // товар из заказа
                salesOrderItemResponse = details.SalesOrderItemId == Guid.Empty || SelectedSalesOrder is null
                    ? null
                    : SelectedSalesOrder.Items.FirstOrDefault(i => i.Id == details.SalesOrderItemId);
            }
        }

        CanIssueMaterials = OrderStatus == ProductionOrderStatus.New;

        await SetProductsBySalesOrderAsync(salesOrderItemResponse);

        await SetDepartments(details);

        // If creating new production order, set default values
        if (ProductionOrderId is null)
        {
            Status = ProductionOrderStatus.New.ProductionOrderRusStatus();
            return;
        }

        // SetSpecification произойдет при присвоении значения SelectedProduct

        // Shipments
        var shipments = await productionOrdersService.GetShipmentsAsync(ProductionOrderId.Value);
        foreach (var s in shipments ?? [])
        {
            Shipments.Add(new ShipmentItemViewModel(s));
        }
    }
    #endregion

    #region SET DETAILS BY PRODUCTION ORDER ID
    private async Task SetDetailsByProductionOrderId(ProductionOrderDetailsResponse details)
    {
        // номер ПЗ
        ProductionOrderNumber = details.ProductionOrderNumber;

        // количество товара в ПЗ
        _initialQtyPlanned = details.QtyPlanned;

        if (QtyParameter is not null)
            Quantity = QtyParameter.Value;
        else
            Quantity = details.QtyPlanned;

        // статус ПЗ
        OrderStatus = details.Status;
        Status = details.Status.ProductionOrderRusStatus();
    }
    #endregion

    #region LOAD
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(LoadDataAsync);
    #endregion

    #region EDIT
    [RelayCommand]
    private async Task EditAsync() => await RunSafeActionAsync(async () =>
    {
        Mode = ProductionOrderCreatePageMode.Edit;
    });
    #endregion

    #region CANCEL
    [RelayCommand]
    private async Task CancelAsync() => await GoBackAsync();
    #endregion

    #region STAGES
    [RelayCommand]
    private async Task StagesAsync() => await RunSafeActionAsync(async () =>
    {
        if (Mode is ProductionOrderCreatePageMode.Create or ProductionOrderCreatePageMode.Edit)
        {
            await SaveDataAsync();
        }

        if (ProductionOrderId is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите заказ на производство", "OK");
            return;
        }

        var parameters = new Dictionary<string, object>
    {
        { "ProductionOrderId", ProductionOrderId.Value.ToString() },
        //{ "ProductInfo", SelectedProduct?.ProductName ?? string.Empty }
    };
        if (ProductionOrderNumber is not null)
            parameters.Add("ProductionOrderNumber", ProductionOrderNumber);

        await Shell.Current.GoToAsync(nameof(ProductionStagesPage), parameters);
    });
    #endregion

    //#region START PRODUCTION
    //[RelayCommand]
    //private async Task StartProductionAsync() => await RunSafeActionAsync(async () =>
    //{
    //    if (Mode is ProductionOrderCreatePageMode.Create)
    //    {
    //        await SaveDataAsync();
    //        await LoadDataAsync();
    //    }

    //    if (ProductionOrderId is null)
    //    {
    //        await Shell.Current.DisplayAlertAsync("Ошибка!", "Производственный заказ не определен.", "OK");
    //        return;
    //    }

    //    if (OrderStatus is null)
    //    {
    //        await Shell.Current.DisplayAlertAsync("Ошибка!", "Статус производственного заказа не определен.", "ОК");
    //        return;
    //    }

    //    await productionOrdersService.StartStageAsync(ProductionOrderId.Value, new StartProductionStageRequest(OrderStatus.Value));
    //    await LoadDataAsync();
    //    await Shell.Current.DisplayAlertAsync("Внимание!", "Стадия производства запущена", "OK");
    //});
    //#endregion

    #region SAVE
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        await SaveDataAsync();
        if (ProductionOrderId is not null)
        {
            await Shell.Current.DisplayAlertAsync("Успех!", "Производственный заказ создан.", "Ок");
            await GoBackAsync();
        }
    });
    #endregion

    #region SAVE DATA
    private async Task SaveDataAsync()
    {
        // Guards
        if (SelectedDepartment is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите цех для выполнения производственного заказа.", "Ок");
            return;
        }

        if (SelectedSalesOrder is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите заказ клиента.", "Ок");
            return;
        }

        if (SelectedProduct is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите товар из заказ клиента.", "Ок");
            return;
        }

        if (Quantity <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Введите количество товара для производства больше 0.", "Ок");
            return;
        }

        // Save Production order

        if (IsCreateMode)
        {
            var order = await productionOrdersService.CreateAsync(new(SelectedProduct.Id, SelectedDepartment.Id, Quantity));
            if (order != null)
            {
                ProductionOrderId = order.Id;
                ProductionOrderNumber = (await productionOrdersService.GetDetailsAsync(order.Id))?.ProductionOrderNumber;
                return;
            }
        }
        if (IsEditMode && ProductionOrderId is not null)
        {
            await productionOrdersService.UpdateAsync(ProductionOrderId.Value, new UpdateProductionOrderRequest(DepartmentId: SelectedDepartment.Id, QtyPlanned: Quantity));
            return;
        }

        await Shell.Current.DisplayAlertAsync("Ошибка!", "Производственный заказ не был создан.", "Ок");
    }
    #endregion

    #region CREATE PURCHASE
    [RelayCommand]
    private async Task CreatePurchaseAsync(MaterialItemViewModel? material)
    {
        if (material is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Внимание!", "Вы уверены, что хотите создать заказ на закупку?", "OK", "Отмена");
        if (!confirm)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "MaterialId", material.MaterialId.ToString() },
            { "QtyMaterial", material.Missing },
            { "Qty", Quantity },
            { "Mode", Mode }
        };

        if (SelectedSalesOrder is not null)
            parameters.Add("SelectedSalesOrder", SelectedSalesOrder);
        if (SelectedProduct is not null)
            parameters.Add("SelectedSalesOrderItem", SelectedProduct);
        if (SelectedDepartment is not null)
            parameters.Add("SelectedDepartment", SelectedDepartment);

        await Shell.Current.GoToAsync(nameof(SupplierOrderCreatePage), parameters);
    }
    #endregion

    #region CONSUME MATERIAL
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
    #endregion

    #region SALES ORDER ENTRY FOCUSED
    [RelayCommand]
    public async Task SalesOrderEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = await popupService.EntryFocusedAsync(salesOrdersService);
        if (id == Guid.Empty) return;

        var salesOrder = await salesOrdersService.GetDetailsAsync(id);

        if (salesOrder is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Заказ не найден", "OK");
            return;
        }

        SelectedSalesOrder = salesOrder;

        await SetProductsBySalesOrderAsync();
    });
    #endregion

    #region SET PRODUCTS BY SALES ORDER
    private async Task SetProductsBySalesOrderAsync(SalesOrderItemResponse? salesOrderItemResponse = null)
    {
        if (SelectedSalesOrder is null)
        {
            var salesOrder = (await salesOrdersService.GetListAsync(take: 1))?.Items[0];
            if (salesOrder is null) return;
            SelectedSalesOrder = await salesOrdersService.GetDetailsAsync(salesOrder.Id);
        }

        Products.Clear();

        foreach (var p in SelectedSalesOrder?.Items ?? [])
            Products.Add(p);

        var targetProduct =
            salesOrderItemResponse
            ?? SelectedProductParameter
            ?? SelectedProduct;

        SelectedProduct = targetProduct is not null
            ? Products.FirstOrDefault(x => x.Id == targetProduct.Id) ?? targetProduct
            : SelectedProduct = Products.FirstOrDefault();
    }
    #endregion

    #region SET DEPARTMENTS
    private async Task SetDepartments(ProductionOrderDetailsResponse? details)
    {
        var departments = await departmentsService.GetListAsync();

        foreach (var d in departments ?? [])
            Departments.Add(d);

        if (SelectedDepartmentParameter is not null)
        {
            SelectedDepartment = Departments.FirstOrDefault(x => x.Id == SelectedDepartmentParameter.Id)
                ?? SelectedDepartmentParameter;
        }
        else if (SelectedDepartment is not null)
        {
            SelectedDepartment = Departments.FirstOrDefault(x => x.Id == SelectedDepartment.Id)
                ?? SelectedDepartment;
        }
        else if (details is not null)
        {
            SelectedDepartment = Departments.FirstOrDefault(x => x.Id == details.DepartmentId);
        }
        else
        {
            SelectedDepartment = Departments.FirstOrDefault();
        }
    }
    #endregion

    #region SET REQUIRED MATERIALS QTY
    private async Task SetRequiredMaterialsQty(decimal qty)
    {
        var productId = SelectedProduct?.ProductId
            ?? Products.FirstOrDefault()?.Id
            ?? Guid.Empty;

        var productDetails = await productsService.GetDetailsAsync(productId);

        foreach (var i in productDetails?.Bom ?? [])
        {
            Materials.FirstOrDefault(m => m.MaterialId == i.MaterialId)?
                .TotalRequired = i.QtyPerUnit * qty;
        }
    }
    #endregion

    #region SET SPECIFICATION
    private async Task SetSpecification()
    {
        Materials.Clear();

        if (ProductionOrderId is null)
        {
            var productId = SelectedProduct?.ProductId
                ?? Products.FirstOrDefault()?.Id
                ?? Guid.Empty;

            var productDetails = await productsService.GetDetailsAsync(productId);

            foreach (var i in productDetails?.Bom ?? [])
            {
                var requiredMaterial = i.QtyPerUnit * Quantity;
                var availableMaterial = (await materialsService.GetDetailsAsync(i.MaterialId))?
                    .Warehouses.Sum(w => w.Qty) ?? 0m;
                var missingMaterial = (availableMaterial - requiredMaterial) >= 0
                    ? 0
                    : requiredMaterial - availableMaterial;

                var materialItem = new MaterialItemViewModel
                    (
                        new(
                            i.MaterialId,
                            i.MaterialName,
                            requiredMaterial,
                            availableMaterial,
                            missingMaterial
                        ),
                        i.Unit
                    );

                Materials.Add(materialItem);
            }
        }
        else
        {
            var materials = await productionOrdersService.GetMaterialsAsync(ProductionOrderId.Value);
            foreach (var m in materials ?? [])
            {
                var unit = (await materialsService.GetDetailsAsync(m.MaterialId))?.UnitCode;
                Materials.Add(new MaterialItemViewModel(m, unit));
            }
        }
    }
    #endregion

    #region BACK 
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync(nameof(ProductionOrdersListPage), true);
    }
    #endregion

    #region RUN SAFE ACTION
    protected async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region MaterialItemViewModel
    public partial class MaterialItemViewModel : ObservableObject
    {
        readonly ProductionOrderMaterialResponse response;

        #region OBSERVABLE PROPERTY
        [ObservableProperty] public Guid materialId;
        [ObservableProperty] public string name;
        [ObservableProperty] public decimal totalRequired;
        [ObservableProperty] public string? requiredString;
        [ObservableProperty] public decimal available;
        [ObservableProperty] public string? availableString;
        [ObservableProperty] public decimal missing;
        [ObservableProperty] public string? missingString;
        #endregion

        #region PROPERTIES
        public string? Unit { get; }
        #endregion

        #region CONSTRUCTOR
        public MaterialItemViewModel(ProductionOrderMaterialResponse _response, string? unit)
        {
            response = _response;
            Unit = unit;
            MaterialId = response.MaterialId;
            Name = response.MaterialName;


            TotalRequired = response.RequiredQty;
            Available = response.AvailableQty;
            Missing = response.MissingQty;

            OnTotalRequiredChanged(TotalRequired);
            OnAvailableChanged(Available);
            OnMissingChanged(Missing);
        }
        #endregion

        #region ON CHANGED
        partial void OnTotalRequiredChanged(decimal value)
        {
            var missingQty = TotalRequired - Available;
            Missing = missingQty > 0 ? missing : 0;
            RequiredString = $"{TotalRequired.ToString("0.##")} {Unit}";
        }

        partial void OnAvailableChanged(decimal value)
        {
            AvailableString = $"{value.ToString("0.##")} {Unit}";
        }

        partial void OnMissingChanged(decimal value)
        {
            MissingString = $"{value.ToString("0.##")} {Unit}";
        }
        #endregion
    }
    #endregion

    #region ShipmentItemViewModel
    public sealed class ShipmentItemViewModel(ProductionOrderShipmentResponse response)
    {
        public Guid WarehouseId { get; } = response.WarehouseId;
        public string WarehouseName { get; } = response.WarehouseName;
        public string Quantity { get; } = response.Qty.ToString();
        public string Date { get; } = response.ShipmentDate.ToShortDateString();
    }
    #endregion
}
