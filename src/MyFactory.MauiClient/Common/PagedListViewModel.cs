using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.Common;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.Common;

public abstract partial class PagedListViewModel<TItem> : ObservableObject
{
    #region PRIVATE CONSTANTS AND VARIABLES
    private CancellationTokenSource? _searchDebounceCts;
    private const int SearchDebounceMs = 350;
    private const int _pageSize = 30;
    private int _skip;
    private string? _sortBy;
    private bool _sortDesc;
    #endregion

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool isDescending;
    [ObservableProperty] private bool isLoadingMore;
    [ObservableProperty] private bool hasMore = true;
    #endregion

    #region ON CHANGED
    partial void OnIsBusyChanged(bool value) => SetIsBusy(value);
    #endregion

    #region OBSERVABLE COLLECTIONS
    public ObservableCollection<TItem> FilteredItems { get; } = [];
    #endregion

    #region PROTECTED PROPERTIES
    protected int Skip => _skip;
    protected string? SortBy => _sortBy;
    protected bool SortDesc => _sortDesc;
    protected int PageSize = _pageSize;

    protected ListResponse<TItem>? _response;
    #endregion

    #region LOAD
    public async Task LoadAsync() => await RunSafeActionAsync(ReloadAsync);
    #endregion

    #region RELOAD
    protected virtual async Task ReloadAsync()
    {
        _skip = 0;
        HasMore = true;
        FilteredItems.Clear();
        await LoadNextPageAsync();
    }
    #endregion

    #region ABSTRACT METHOD SET RESPONSE
    protected abstract Task SetResponse();
    #endregion

    #region VIRTUAL METHOD SET IS_BUSY
    protected virtual void SetIsBusy(bool value) { }
    #endregion

    #region LOAD NEXT PAGE
    [RelayCommand]
    public async Task LoadNextPageAsync()
    {
        if (IsLoadingMore || !HasMore)
            return;

        try
        {
            IsLoadingMore = true;

            await SetResponse();

            var items = _response?.Items ?? [];
            foreach (var item in items)
                FilteredItems.Add(item);

            _skip += items.Count;
            HasMore = _response?.HasMore ?? false;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsLoadingMore = false;
        }
    }
    #endregion

    #region SORT
    [RelayCommand]
    private async Task SortAsync(object? parameter) => await RunSafeActionAsync(async () =>
    {
        if (parameter is not SortLabel.SortLabelCommandParameter sortParam)
            return;

        _sortBy = sortParam.SortKey;
        _sortDesc = sortParam.IsDescending;
        IsDescending = sortParam.IsDescending;

        await ReloadAsync();
    });
    #endregion

    #region SCHEDULE SEARCH
    protected void ScheduleSearchReload()
    {
        CancelSearchDebounce();
        _searchDebounceCts = new CancellationTokenSource();
        _ = DebouncedReloadAsync(_searchDebounceCts.Token);
    }
    #endregion

    #region DEBOUNCE
    private async Task DebouncedReloadAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(SearchDebounceMs, token);
            if (token.IsCancellationRequested)
                return;

            await RunSafeActionAsync(ReloadAsync);
        }
        catch (OperationCanceledException) { }
    }
    #endregion

    #region DISPOSE
    public void Dispose()
    {
        CancelSearchDebounce();
    }
    #endregion

    #region CANCEL SEARCH DEBOUNCE
    private void CancelSearchDebounce()
    {
        var cts = Interlocked.Exchange(ref _searchDebounceCts, null);
        if (cts is null)
            return;

        try
        {
            cts.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // already disposed
        }

        cts.Dispose();
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
}
