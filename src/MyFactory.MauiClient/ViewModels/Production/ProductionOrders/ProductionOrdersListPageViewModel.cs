using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Pages.Production;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Customers;
using MyFactory.MauiClient.Services.ProductionOrders;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.SalesOrders;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

[QueryProperty(nameof(SalesOrderParameterId), "SalesOrderId")]
public partial class ProductionOrdersListPageViewModel : PagedListViewModel<ProductionOrderListItemResponse>
{
    #region SERVICES
    private readonly IProductionOrdersService _productionOrdersService;
    private readonly ICustomersService _customersService;
    private readonly ISalesOrdersService _salesOrdersService;
    private readonly IProductsService _productsService;
    private readonly IPopupService _popupService;
    #endregion

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? salesOrderId;
    [ObservableProperty] private string? salesOrderParameterId;

    [ObservableProperty] private DateTime? fromDate;
    [ObservableProperty] private DateTime? toDate;
    [ObservableProperty] private string? selectedStatus;
    [ObservableProperty] private string? searchProductName;
    [ObservableProperty] private string? searchSaleOrderNumber;
    [ObservableProperty] private string? searchCustomerName;
    [ObservableProperty] private string? searchProductionOrderNumber;
    #endregion
    
    #region COLLECTIONS
    [ObservableProperty]
    private List<string> statusOptions;
    #endregion

    #region CONSTRUCTOR
    public ProductionOrdersListPageViewModel(
        IProductionOrdersService productionOrdersService,
        ICustomersService customersService,
        ISalesOrdersService salesOrdersService,
        IProductsService productsService,
        IPopupService popupService)
    {
        _productionOrdersService = productionOrdersService;
        _customersService = customersService;
        _salesOrdersService = salesOrdersService;
        _productsService = productsService;
        _popupService = popupService;

        StatusOptions = [.. Enum.GetValues<ProductionOrderStatus>().Select(s => s.ProductionOrderRusStatus())];
        StatusOptions.Insert(0, "Все");
        SelectedStatus = StatusOptions[0];
    }
    #endregion

    #region ON CHANGED
    partial void OnSalesOrderParameterIdChanged(string? value)
    {
        salesOrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnSelectedStatusChanged(string? value)
    {
        _ = LoadAsync();
    }
    #endregion

    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        var status = SelectedStatus?.ProductionOrderStatusFromRus();
        _response = await _productionOrdersService.GetListByDateAsync(
            searchProductionOrderNumber: SearchProductionOrderNumber,
            searchSaleOrderNumber: SearchSaleOrderNumber,
            searchCustomerName: SearchCustomerName,
            searchProductName: SearchProductName,
            sortBy: SortBy,
            sortDesc: SortDesc,
            skip: Skip,
            take: PageSize,
            fromDate: FromDate,
            toDate: ToDate,
            status: status);
    }
    #endregion

    #region GO TO CREATE PAGE
    private async Task GoToCreatePageAsync(ProductionOrderCreatePageMode mode, Guid? productionOrderId = null)
    {
        var parameters = new Dictionary<string, object?>
        {
            { "Mode", mode }
        };
        if (productionOrderId.HasValue)
            parameters.Add("ProductionOrderId", productionOrderId.Value.ToString());

        await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage), parameters);
    }
    #endregion

    #region ADD
    [RelayCommand]
    private async Task AddAsync() 
        => await GoToCreatePageAsync(ProductionOrderCreatePageMode.Create);
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(ProductionOrderListItemResponse? item)
    {
        if (item is null)
            return;

        await GoToCreatePageAsync(ProductionOrderCreatePageMode.View, item?.Id);    
    }
    #endregion

    #region EDIT
    [RelayCommand]
    private async Task EditAsync(ProductionOrderListItemResponse? item)
    {
        if (item is null)
            return;

        await GoToCreatePageAsync(ProductionOrderCreatePageMode.Edit, item.Id);
    }
    #endregion

    #region OPEN STAGES
    [RelayCommand]
    private async Task OpenStagesAsync(ProductionOrderListItemResponse? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", item.Id.ToString() },
            { "ProductionOrderNumber", item.ProductionOrderNumber },
            //{ "ProductInfo", item.ProductName }
        };
        await Shell.Current.GoToAsync(nameof(ProductionStagesPage), parameters);
    }
    #endregion

    #region DELETE ITEM
    [RelayCommand]
    private async Task DeleteAsync(ProductionOrderListItemResponse? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Внимание!", $"Удалить производственный заказ {item.ProductionOrderNumber}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _productionOrdersService.DeleteAsync(item.Id);
            FilteredItems.Remove(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
    }
    #endregion

    #region CUSTOMER ENTRY FOCUSED
    [RelayCommand]
    public async Task CustomerEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = await _popupService.EntryFocusedAsync(_customersService);
        if (id == Guid.Empty) return;

        var selectedCustomer = await _customersService.GetDetailsAsync(id);

        if (selectedCustomer is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Заказчик не найден", "OK");
            return;
        }

        SearchCustomerName = selectedCustomer.Name;

        await ReloadAsync();
    });
    #endregion

    #region SALES ORDER ENTRY FOCUSED
    [RelayCommand]
    public async Task SalesOrderEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = await _popupService.EntryFocusedAsync(_salesOrdersService);
        if (id == Guid.Empty) return;

        var selectedSalesOrder = await _salesOrdersService.GetDetailsAsync(id);

        if (selectedSalesOrder is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Заказ не найден", "OK");
            return;
        }

        SearchSaleOrderNumber = selectedSalesOrder.OrderNumber.ToString();

        await ReloadAsync();
    });
    #endregion

    #region PRODUCT NAME ENTRY FOCUSED
    [RelayCommand]
    public async Task ProductEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = await _popupService.EntryFocusedAsync(_productsService);
        if (id == Guid.Empty) return;

        var selectedProduct = await _productsService.GetDetailsAsync(id);
        if (selectedProduct is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Товар не найден", "OK");
            return;
        }

        SearchProductName = selectedProduct.Name;

        await ReloadAsync();
    });
    #endregion

    #region PRODUCTION ORDER NUMBER ENTRY FOCUSED
    [RelayCommand]
    public async Task ProductionOrderEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = await _popupService.EntryFocusedAsync(_productionOrdersService);
        if (id == Guid.Empty) return;

        var selectedProductionOrder = await _productionOrdersService.GetDetailsAsync(id);
        if (selectedProductionOrder is null)   
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Производственный заказ не найден", "OK");
            return;
        }

        SearchProductionOrderNumber = selectedProductionOrder.ProductionOrderNumber.ToString();

        await ReloadAsync();
    });
    #endregion

    #region CLEAR FILTERS
    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        SearchProductName = null;
        SearchSaleOrderNumber = null;
        SearchCustomerName = null;
        SearchProductionOrderNumber = null;
        SelectedStatus = StatusOptions[0];  

        await LoadAsync();
    }
    #endregion
}