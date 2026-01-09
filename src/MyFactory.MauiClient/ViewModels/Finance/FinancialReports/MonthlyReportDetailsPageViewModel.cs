using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Reports;
using MyFactory.MauiClient.Services.Reports;

namespace MyFactory.MauiClient.ViewModels.Finance.FinancialReports;

[QueryProperty(nameof(YearParameter), "Year")]
[QueryProperty(nameof(MonthParameter), "Month")]
public partial class MonthlyReportDetailsPageViewModel : ObservableObject
{
    private readonly IReportsService _reportsService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private int year;

    [ObservableProperty]
    private string? yearParameter;

    [ObservableProperty]
    private int month;

    [ObservableProperty]
    private string? monthParameter;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private MonthlyReportStatus status;

    [ObservableProperty]
    private string calculatedAt = string.Empty;

    [ObservableProperty]
    private Guid createdBy;

    [ObservableProperty]
    private decimal totalRevenue;

    [ObservableProperty]
    private decimal payrollExpenses;

    [ObservableProperty]
    private decimal materialExpenses;

    [ObservableProperty]
    private decimal otherExpenses;

    [ObservableProperty]
    private decimal profit;

    public MonthlyReportDetailsPageViewModel(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    partial void OnYearChanged(int value)
    {
        _ = LoadAsync();
    }

    partial void OnYearParameterChanged(string? value)
    {
        Year = int.TryParse(value, out var year) ? year : 0;
    }

    partial void OnMonthChanged(int value)
    {
        _ = LoadAsync();
    }

    partial void OnMonthParameterChanged(string? value)
    {
        Month = int.TryParse(value, out var month) ? month : 0;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (Year == 0 || Month == 0)
        {
            return;
        }

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            var details = await _reportsService.GetMonthlyDetailsAsync(Year, Month);
            if (details is null)
            {
                return;
            }

            Title = $"‘ËÌ‡ÌÒÓ‚˚È ÓÚ˜∏Ú ó {Month:00}.{Year}";
            Status = details.Status;
            CalculatedAt = details.CalculatedAt.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            CreatedBy = details.CreatedBy;
            TotalRevenue = details.TotalRevenue;
            PayrollExpenses = details.PayrollExpenses;
            MaterialExpenses = details.MaterialExpenses;
            OtherExpenses = details.OtherExpenses;
            Profit = details.Profit;
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
    private async Task RecalculateAsync()
    {
        try
        {
            await _reportsService.RecalculateAsync(new RecalculateMonthlyFinancialReportRequest(Year, Month));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
    }

    [RelayCommand]
    private async Task ApproveAsync()
    {
        try
        {
            await _reportsService.ApproveAsync(new ApproveMonthlyFinancialReportRequest(Year, Month));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
    }

    [RelayCommand]
    private async Task CloseAsync()
    {
        try
        {
            await _reportsService.CloseAsync(new CloseMonthlyFinancialReportRequest(Year, Month));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
    }
}

