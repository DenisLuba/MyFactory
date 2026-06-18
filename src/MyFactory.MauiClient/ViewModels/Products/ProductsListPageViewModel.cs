using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Common;

namespace MyFactory.MauiClient.ViewModels.Products;

public partial class ProductsListPageViewModel(IProductsService productsService) : PagedListViewModel<ProductListItemResponse>
{

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private string? searchText;
    [ObservableProperty] private string? searchType;
    #endregion

    #region ON CHANGED
    partial void OnSearchTextChanged(string? value) => ScheduleSearchReload();
    partial void OnSearchTypeChanged(string? value) => ScheduleSearchReload();
    #endregion

    #region OVERRIDE METHOD SET RESPONSE
    protected override async Task SetResponse()
    {
        _response = await productsService.GetListAsync(
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
        await Shell.Current.GoToAsync(nameof(ProductEditPage));
    });
    #endregion

    #region OPEN DETAILS
    [RelayCommand]
    private async Task OpenDetailsAsync(ProductListItemResponse? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(ProductDetailsPage), new Dictionary<string, object>
        {
            { "ProductId", item.Id.ToString() }
        });
    });
    #endregion
}

