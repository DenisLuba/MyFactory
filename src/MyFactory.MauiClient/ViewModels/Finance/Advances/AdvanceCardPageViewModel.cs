using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.UIModels.Finance;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyFactory.MauiClient.Services.FinanceServices;
using MyFactory.MauiClient.Pages.Finance.Advances;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.ViewModels.Finance.Advances;

public partial class AdvanceCardPageViewModel : ObservableObject
{
    [ObservableProperty]
    private AdvanceItem advance;

    [ObservableProperty]
    private string comment;

    [ObservableProperty]
    private ObservableCollection<AdvanceReportItem> reportItems = new();

    [ObservableProperty]
    private decimal totalSpent;
    [ObservableProperty]
    private decimal balance;
    [ObservableProperty]
    private bool hasReport;

    private readonly IFinanceService _financeService;
    private readonly AdvancesTablePageViewModel _parentTableViewModel;

    public IRelayCommand AddReportCommand { get; }
    public IRelayCommand EditAdvanceCommand { get; }
    public IAsyncRelayCommand CloseAdvanceCommand { get; }
    public IAsyncRelayCommand DeleteAdvanceCommand { get; }

    public AdvanceCardPageViewModel(IFinanceService financeService, AdvancesTablePageViewModel parentTableViewModel)
    {
        _financeService = financeService;
        _parentTableViewModel = parentTableViewModel;
        AddReportCommand = new RelayCommand(AddReport);
        EditAdvanceCommand = new RelayCommand(EditAdvance);
        CloseAdvanceCommand = new AsyncRelayCommand(CloseAdvanceAsync);
        DeleteAdvanceCommand = new AsyncRelayCommand(DeleteAdvanceAsync);
    }

    private async void AddReport()
    {
        // Навигация на страницу отчета по авансу
        await Shell.Current.GoToAsync(nameof(AdvanceReportCardPage), new Dictionary<string, object>
        {
            { "Advance", Advance }
        });
    }

    private async void EditAdvance()
    {
        // Навигация на страницу редактирования аванса (можно использовать ту же страницу с режимом редактирования)
        await Shell.Current.GoToAsync(nameof(AdvanceCardPage), new Dictionary<string, object>
        {
            { "Advance", Advance },
            { "EditMode", true }
        });
    }

    private async Task CloseAdvanceAsync()
    {
        // Закрываем аванс через сервис
        await _financeService.CloseAdvanceAsync(Advance.AdvanceNumber);
        Advance = Advance with { Status = AdvanceStatus.Reported.ToString() };
        // Уведомляем пользователя об успехе операции
        await Shell.Current.DisplayAlertAsync("Успех", "Выдача успешно закрыта.", "OK");
        // Обновляем таблицу авансов в родительской ViewModel
        _parentTableViewModel?.LoadAdvancesCommand.Execute(null);
    }

    private async Task DeleteAdvanceAsync()
    {
        // Удаляем аванс через сервис
        await _financeService.DeleteAdvanceAsync(Advance.AdvanceNumber);
        // Уведомляем пользователя об успешном удалении
        await Shell.Current.DisplayAlertAsync("Удалено", "Выдача удалена.", "OK");
        // Обновляем таблицу авансов в родительской ViewModel
        _parentTableViewModel?.LoadAdvancesCommand.Execute(null);
        // Навигация назад после удаления
        await Shell.Current.GoToAsync("..", true);
    }
}
