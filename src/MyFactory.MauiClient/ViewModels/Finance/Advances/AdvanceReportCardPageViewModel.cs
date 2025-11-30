using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Finance;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.ViewModels.Finance.Advances;

public partial class AdvanceReportCardPageViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<AdvanceReportItem> reportItems = new();

    [ObservableProperty]
    private decimal totalSpent;
    [ObservableProperty]
    private decimal advanceAmount;
    [ObservableProperty]
    private decimal balance;
    [ObservableProperty]
    private bool isOverSpent;

    public IRelayCommand AddReportItemCommand { get; }
    public IRelayCommand<AdvanceReportItem> DeleteReportItemCommand { get; }
    public IRelayCommand SaveReportCommand { get; }
    public IRelayCommand SubmitReportCommand { get; }

    public AdvanceReportCardPageViewModel()
    {
        AddReportItemCommand = new RelayCommand(AddReportItem);
        DeleteReportItemCommand = new RelayCommand<AdvanceReportItem>(DeleteReportItem);
        SaveReportCommand = new RelayCommand(SaveReport);
        SubmitReportCommand = new RelayCommand(SubmitReport);
    }

    private void AddReportItem()
    {
        // Добавить новую строку расхода
    }

    private void DeleteReportItem(AdvanceReportItem item)
    {
        // Удалить строку расхода
    }

    private void SaveReport()
    {
        // Сохранить отчет
    }

    private void SubmitReport()
    {
        // Провести отчет (изменить статус выдачи)
    }
}
