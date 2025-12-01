using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.UIModels.Finance;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MyFactory.MauiClient.Services.FinanceServices;
using MyFactory.MauiClient.Pages.Finance.Advances;
using Microsoft.Maui.Controls;

namespace MyFactory.MauiClient.ViewModels.Finance.Advances;

public partial class AdvanceCardPageViewModel : ObservableObject
{
    [ObservableProperty]
    private AdvanceItem advance = new AdvanceItem("", "", 0, "", AdvanceStatus.Pending);

    [ObservableProperty]
    private string comment = string.Empty;

    [ObservableProperty]
    private ObservableCollection<AdvanceReportItem> reportItems = new();

    [ObservableProperty]
    private decimal totalSpent;
    [ObservableProperty]
    private decimal balance;
    [ObservableProperty]
    private bool hasReport;

    private readonly IFinanceService _financeService;
    private AdvancesTablePageViewModel? _parentTableViewModel;

    public IRelayCommand AddReportCommand { get; }
    public IRelayCommand EditAdvanceCommand { get; }
    public IAsyncRelayCommand CloseAdvanceCommand { get; }
    public IAsyncRelayCommand DeleteAdvanceCommand { get; }

    public AdvanceCardPageViewModel(IFinanceService financeService)
    {
        _financeService = financeService;
        AddReportCommand = new RelayCommand(AddReport);
        EditAdvanceCommand = new RelayCommand(EditAdvance);
        CloseAdvanceCommand = new AsyncRelayCommand(CloseAdvanceAsync);
        DeleteAdvanceCommand = new AsyncRelayCommand(DeleteAdvanceAsync);
    }

    public void SetParentViewModel(AdvancesTablePageViewModel parentViewModel)
    {
        _parentTableViewModel = parentViewModel;
    }

    partial void OnAdvanceChanged(AdvanceItem value)
    {
        // Расчет баланса при установке аванса
        CalculateBalance();
    }

    partial void OnReportItemsChanged(ObservableCollection<AdvanceReportItem> value)
    {
        // Пересчет при изменении списка расходов
        CalculateBalance();
    }

    private void CalculateBalance()
    {
        if (Advance != null)
        {
            TotalSpent = ReportItems?.Sum(item => item.Amount) ?? 0;
            Balance = Advance.AdvanceAmount - TotalSpent;
            HasReport = ReportItems?.Any() ?? false;
        }
    }

    private async void AddReport()
    {
        // Навигация на страницу отчета по авансу
        var navigationParameters = new Dictionary<string, object>
        {
            { "Advance", Advance }
        };

        if (_parentTableViewModel != null)
        {
            navigationParameters.Add("ParentViewModel", _parentTableViewModel);
        }

        await Shell.Current.GoToAsync(nameof(AdvanceReportCardPage), navigationParameters);
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
        Advance = Advance with { Status = AdvanceStatus.Reported };
        // Уведомляем пользователя об успехе операции
        await Shell.Current.DisplayAlert("Успех", "Выдача успешно закрыта.", "OK");
        // Обновляем таблицу авансов в родительской ViewModel
        _parentTableViewModel?.LoadAdvancesCommand.Execute(null);
    }

    private async Task DeleteAdvanceAsync()
    {
        // Удаляем аванс через сервис
        await _financeService.DeleteAdvanceAsync(Advance.AdvanceNumber);
        // Уведомляем пользователя об успешном удалении
        await Shell.Current.DisplayAlert("Удалено", "Выдача удалена.", "OK");
        // Обновляем таблицу авансов в родительской ViewModel
        _parentTableViewModel?.LoadAdvancesCommand.Execute(null);
        // Навигация назад после удаления
        await Shell.Current.GoToAsync("..", true);
    }
}
