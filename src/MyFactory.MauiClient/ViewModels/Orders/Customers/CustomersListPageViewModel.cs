using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Pages.Orders.Customers;
using MyFactory.MauiClient.Services.Customers;

namespace MyFactory.MauiClient.ViewModels.Orders.Customers;

public partial class CustomersListPageViewModel(ICustomersService customersService) : PagedListViewModel<CustomerListItemResponse>
{
    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private string? searchName;
    [ObservableProperty] private bool includeInactive = false;
    #endregion

    #region ON CHANGED
    partial void OnSearchNameChanged(string? value) => ScheduleSearchReload();
    #endregion

    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        bool? status = IncludeInactive ? true : null;
        _response = await customersService.GetListAsync(
            searchName: SearchName,
            sortBy: SortBy,
            sortDesc: SortDesc,
            skip: Skip,
            take: PageSize,
            isActive: status);
    }
    #endregion

    #region ADD
    [RelayCommand]
    private async Task AddAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.GoToAsync(nameof(CustomerDetailsPage), new Dictionary<string, object>
        {
            { "IsEditMode", true }
        });
    });
    #endregion

    #region EDIT
    [RelayCommand]
    private async Task EditAsync(CustomerListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(CustomerDetailsPage), new Dictionary<string, object>
        {
            { "CustomerId", item.Id.ToString() },
            { "IsEditMode", true }
        });
    });
    #endregion

    #region DEACTIVATE
    [RelayCommand]
    private async Task DeactivateAsync(CustomerListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Внимание!", $"Вы действительно хотите деактивировать клиента {item.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        await customersService.DeactivateAsync(item.Id);
        await ReloadAsync();
    });
    #endregion

    #region ACTIVATE
    [RelayCommand]
    private async Task ActivateAsync(CustomerListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Внимание!", $"Вы действительно хотите активировать клиента {item.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        await customersService.ActivateAsync(item.Id);
        await ReloadAsync();
    });
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(CustomerListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "CustomerId", item.Id.ToString() },
            { "IsEditMode", false }
        };

        await Shell.Current.GoToAsync(nameof(CustomerDetailsPage), parameters);
    });
    #endregion

    #region STATUS SWITCHER
    [RelayCommand]
    private async Task StatusSwitcherAsync() => await RunSafeActionAsync(async () =>
    {
        IncludeInactive = !IncludeInactive;
        await ReloadAsync();
    });
    #endregion
}

