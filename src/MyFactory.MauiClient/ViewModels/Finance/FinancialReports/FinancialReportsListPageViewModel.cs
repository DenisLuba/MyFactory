using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Reports;
using MyFactory.MauiClient.Services.Reports;

namespace MyFactory.MauiClient.ViewModels.Finance.FinancialReports;

public partial class FinancialReportsListPageViewModel : ObservableObject
{
    private readonly IReportsService _reportsService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<MonthlyFinancialReportListItemResponse> Reports { get; } = new();

    public FinancialReportsListPageViewModel(IReportsService reportsService)
    {
        _reportsService = reportsService;
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
            Reports.Clear();
            var items = await _reportsService.GetMonthlyAsync() ?? Array.Empty<MonthlyFinancialReportListItemResponse>();
            foreach (var item in items.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month))
            {
                Reports.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CalculateAsync()
    {
        var now = DateTime.Today;
        try
        {
            await _reportsService.CalculateAsync(new CalculateMonthlyFinancialReportRequest(now.Year, now.Month));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(MonthlyFinancialReportListItemResponse? report)
    {
        if (report is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(Pages.Finance.FinancialReports.MonthlyReportDetailsPage), new Dictionary<string, object>
        {
            { "Year", report.Year },
            { "Month", report.Month }
        });
    }
}

