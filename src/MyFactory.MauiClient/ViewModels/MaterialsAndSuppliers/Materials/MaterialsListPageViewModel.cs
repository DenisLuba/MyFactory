using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

public partial class MaterialsListPageViewModel(IMaterialsService materialsService) : PagedListViewModel<MaterialListItemResponse>
{    
    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private string? searchType;
    [ObservableProperty] private string? searchText;
    #endregion

    #region ON CHANGED
    partial void OnSearchTextChanged(string? value) => ScheduleSearchReload();
    partial void OnSearchTypeChanged(string? value) => ScheduleSearchReload();
    #endregion

    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        _response = await materialsService.GetListAsync(
            searchName: SearchText, 
            searchType: SearchType,
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
        await Shell.Current.GoToAsync(nameof(MaterialDetailsEditPage));
    });
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(MaterialListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(MaterialDetailsViewPage), new Dictionary<string, object>
        {
            { "MaterialId", item.Id.ToString() }
        });
    });
    #endregion

    #region EDIT
    [RelayCommand]
    private async Task EditAsync(MaterialListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(MaterialDetailsEditPage), new Dictionary<string, object>
        {
            { "MaterialId", item.Id.ToString() }
        });
    });
    #endregion

    #region DELETE
    [RelayCommand]
    private async Task DeleteAsync(MaterialListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Подтверждение", "Вы уверены, что хотите удалить материал?", "Да", "Нет");
        if (!confirm)
            return;

        await materialsService.DeleteAsync(item.Id);

        await ReloadAsync();
    });
    #endregion
}

