using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;

namespace MyFactory.MauiClient.Controllers;

public partial class SearchViewModel(IPopupService popupService) : PagedListViewModel<SearchItemDto>, IQueryAttributable
{
    #region CONSTANTS
    private const string SearchItemsQueryKey = "Search";
    private const string SearchByNameQueryKey = "Name";
    private const string SearchByTypeQueryKey = "Type";
    #endregion

    #region SERVICES
    private readonly IPopupService _popupService = popupService;
    private readonly INavigation navigation =
        Application.Current?.Windows[0].Page?.Navigation ??
        throw new InvalidOperationException("Unable to locate INavigation.");
    #endregion

    #region PRIVATE VARIABLES
    private ISearchPageSouce? _source;
    private bool IsInitialized = false;
    #endregion

    #region OBSERVABLE PROPERTIES    
    [ObservableProperty] private string searchName = string.Empty;
    [ObservableProperty] private string searchType = string.Empty;
    [ObservableProperty] private bool isVisiblePage = false;
    #endregion

    #region ON CHANGED
    partial void OnSearchNameChanged(string value) 
    {
        if (IsInitialized)
            ScheduleSearchReload();
    }
    partial void OnSearchTypeChanged(string value) 
    {
        if (IsInitialized)
            ScheduleSearchReload();
    }
    #endregion

    #region OVERRIDE SET IS_BUSY
    protected override void SetIsBusy(bool value)
    {
        IsVisiblePage = !value;
    }    
    #endregion

    #region SELECT
    [RelayCommand]
    private async Task SelectAsync(SearchItemDto selectedItem, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(selectedItem.Id) || string.IsNullOrWhiteSpace(selectedItem.Name)) return;

        await _popupService.ClosePopupAsync(navigation, selectedItem.Id, token);
    }
    #endregion

    #region APPLY QUERY ATTRIBUTES
    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue(SearchItemsQueryKey, out var value) || value is not ISearchPageSouce source)
            return;

        _source = source;

        if (query.TryGetValue(SearchByNameQueryKey, out value) && value is string name && !string.IsNullOrWhiteSpace(name))
        {
            SearchName = name;
        }

        if (query.TryGetValue(SearchByTypeQueryKey, out value) && value is string type && !string.IsNullOrWhiteSpace(type))
        {
            SearchType = type;
        }

        IsInitialized = true;

        await LoadAsync();
    }
    #endregion

    #region PROTECTED SET RESPONSE
    protected override async Task SetResponse()
    {
        if (_source is not null)
        {
            _response = await _source.GetPageAsync(
                skip: Skip,
                take: PageSize,
                searchName: SearchName,
                searchType: SearchType);
        }
    }
    #endregion
}
