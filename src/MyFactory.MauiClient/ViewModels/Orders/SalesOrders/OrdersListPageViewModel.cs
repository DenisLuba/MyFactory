using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Pages.Orders.SalesOrders;
using MyFactory.MauiClient.Services.SalesOrders;
using MyFactory.MauiClient.Services.Customers;

namespace MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

public partial class OrdersListPageViewModel : PagedListViewModel<SalesOrderListItemResponse>
{
    #region SERVICES
    private readonly ISalesOrdersService _salesOrdersService;
    private readonly ICustomersService _customersService;
    private readonly IPopupService _popupService;
    #endregion

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private string? searchName;
    [ObservableProperty] private DateTime? fromDate;
    [ObservableProperty] private DateTime? toDate;
    [ObservableProperty] private string? selectedStatus;
    #endregion

    #region COLLECTIONS
    [ObservableProperty]
    private List<string> statusOptions;
    #endregion

    #region ON CHANGED
    partial void OnSearchNameChanged(string? value) => ScheduleSearchReload();
    partial void OnSelectedStatusChanged(string? value) => _ = LoadAsync();
    partial void OnFromDateChanged(DateTime? value) => _ = LoadAsync();
    partial void OnToDateChanged(DateTime? value) => _ = LoadAsync();
    #endregion

    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        var status = SelectedStatus?.SalesOrderStatusFromRus();
        _response = await _salesOrdersService.GetListByDateAsync(
            searchName: SearchName,
            sortBy: SortBy,
            sortDesc: SortDesc,
            skip: Skip,
            take: PageSize,
            fromDate: FromDate,
            toDate: ToDate,
            status: status);
    }
    #endregion

    #region CONSTRUCTOR
    public OrdersListPageViewModel(ISalesOrdersService salesOrdersService, ICustomersService customersService, IPopupService popupService)
    {
        _salesOrdersService = salesOrdersService;
        _customersService = customersService;
        _popupService = popupService;

        StatusOptions = [.. Enum.GetValues<SalesOrderStatus>().Select(s => s.SalesOrderRusStatus())];
        StatusOptions.Insert(0, "Все");
        SelectedStatus = StatusOptions[0];
    }
    #endregion

    #region CustomerEntryFocusedCommand
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

        SearchName = selectedCustomer.Name;
    });
    #endregion

    #region ADD
    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(OrderCreatePage), true);
    }
    #endregion

    #region EDIT
    [RelayCommand]
    private async Task EditAsync(SalesOrderListItemResponse? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "OrderId", item.Id.ToString() }
        };

        await Shell.Current.GoToAsync(nameof(OrderUpdatePage), parameters);
    }
    #endregion

    #region DELETE
    [RelayCommand]
    private async Task DeleteAsync(SalesOrderListItemResponse? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Внимание!", $"Вы действительно хотите удалить заказ {item.OrderNumber}?", "", "");
        if (!confirm)
            return;

        try
        {
            await _salesOrdersService.DeleteAsync(item.Id);
            FilteredItems.Remove(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
    }
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(SalesOrderListItemResponse? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "OrderId", item.Id.ToString() }
        };

        await Shell.Current.GoToAsync(nameof(OrderDetailsPage), parameters);
    }
    #endregion

    #region CleanSearchNameCommand
    [RelayCommand]
    private void CleanSearchName() => SearchName = null;
    #endregion
}

