using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Advances;
using MyFactory.MauiClient.Services.Advances;

namespace MyFactory.MauiClient.ViewModels.Finance.Expenses;

public partial class CashAdvancesListPageViewModel : ObservableObject
{
    private readonly IAdvancesService _advancesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<CashAdvanceListItemResponse> CashAdvances { get; } = new();

    public CashAdvancesListPageViewModel(IAdvancesService advancesService)
    {
        _advancesService = advancesService;
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            CashAdvances.Clear();
            var items = await _advancesService.GetListAsync();
            if (items is not null)
            {
                foreach (var item in items.OrderByDescending(x => x.IssueDate))
                {
                    CashAdvances.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.Finance.Expenses.CashAdvanceCreatePage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(CashAdvanceListItemResponse? item)
    {
        if (item is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(Pages.Finance.Expenses.CashAdvanceDetailsPage), new Dictionary<string, object>
        {
            { "AdvanceItem", item }
        });
    }

    [RelayCommand]
    private async Task EditAsync(CashAdvanceListItemResponse? item)
    {
        await OpenDetailsAsync(item);
    }

    [RelayCommand]
    private async Task DeleteAsync(CashAdvanceListItemResponse? item)
    {
        if (item is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlert("Закрыть аванс", "Закрыть аванс?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            await _advancesService.CloseAsync(item.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }
}

