using System.Collections.ObjectModel;
using MyFactory.MauiClient.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

public partial class SuppliersListPageViewModel(ISuppliersService suppliersService) : PagedListViewModel<SupplierListItemResponse>
{
    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private string? searchName;
    #endregion

    #region ON CHANGED
    partial void OnSearchNameChanged(string? value) => ScheduleSearchReload();
    #endregion
    
    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        _response = await suppliersService.GetListAsync(
            searchName: SearchName,
            sortBy: SortBy,
            sortDesc: SortDesc,
            skip: Skip,
            take: PageSize,
            isActive: null,
            fromId: null);
    }
    #endregion

    #region ADD
    [RelayCommand]
    private async Task AddAsync() => await RunSafeActionAsync(async () =>
    {
        var name = await Shell.Current.DisplayPromptAsync("Название поставщика", "Введите название поставщика.", placeholder: "Название");
        if (string.IsNullOrWhiteSpace(name))
            return;

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Описание поставщика (необязательно)");

        await suppliersService.CreateAsync(new CreateSupplierRequest(name.Trim(), string.IsNullOrWhiteSpace(description) ? null : description.Trim()));

        await ReloadAsync();
    });
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(SupplierListItemResponse? supplier) => await RunSafeActionAsync(async () =>
    {
        if (supplier is null)
            return;

        await Shell.Current.GoToAsync(nameof(SupplierDetailsPage), new Dictionary<string, object>
        {
            { "SupplierId", supplier.Id.ToString() }
        });
    });
    #endregion

    #region EDIT
    [RelayCommand]
    private async Task EditAsync(SupplierListItemResponse? supplier) => await RunSafeActionAsync(async () =>
    {
        if (supplier is null)
            return;

        var newName = await Shell.Current.DisplayPromptAsync("Редактирование", "Введите новое название поставщика.", initialValue: supplier.Name);
        if (string.IsNullOrWhiteSpace(newName) || string.Equals(newName, supplier.Name, StringComparison.Ordinal))
            return;

        await suppliersService.UpdateAsync(supplier.Id, new UpdateSupplierRequest(newName.Trim(), null));

        await ReloadAsync();
    });
    #endregion

    #region DELETE
    [RelayCommand]
    private async Task DeleteAsync(SupplierListItemResponse? supplier) => await RunSafeActionAsync(async () =>
    {
        if (supplier is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить поставщика {supplier.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        await suppliersService.DeleteAsync(supplier.Id);

        await ReloadAsync();        
    });
    #endregion
}

