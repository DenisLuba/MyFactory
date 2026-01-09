using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.Services.Finance;

namespace MyFactory.MauiClient.ViewModels.Finance.Payroll;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
[QueryProperty(nameof(YearParameter), "Year")]
[QueryProperty(nameof(MonthParameter), "Month")]
[QueryProperty(nameof(EmployeeName), "EmployeeName")]
public partial class PayrollDailyBreakdownPageViewModel : ObservableObject
{
    private readonly IFinanceService _financeService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private Guid employeeId;

    [ObservableProperty]
    private string? employeeIdParameter;

    [ObservableProperty]
    private int year;

    [ObservableProperty]
    private string? yearParameter;

    [ObservableProperty]
    private int month;

    [ObservableProperty]
    private string? monthParameter;

    [ObservableProperty]
    private string employeeName = string.Empty;

    [ObservableProperty]
    private string position = string.Empty;

    [ObservableProperty]
    private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

    [ObservableProperty]
    private DateTime toDate = DateTime.Today;

    [ObservableProperty]
    private string payoutAmount = string.Empty;

    [ObservableProperty]
    private decimal totalHours;

    [ObservableProperty]
    private decimal totalQtyPlanned;

    [ObservableProperty]
    private decimal totalQtyProduced;

    [ObservableProperty]
    private decimal totalQtyExtra;

    [ObservableProperty]
    private decimal totalBaseAmount;

    [ObservableProperty]
    private decimal totalPremiumAmount;

    [ObservableProperty]
    private decimal totalAmount;

    [ObservableProperty]
    private decimal paidAmount;

    [ObservableProperty]
    private decimal remainingAmount;

    public ObservableCollection<EmployeePayrollAccrualDailyResponse> DailyItems { get; } = new();

    public PayrollDailyBreakdownPageViewModel(IFinanceService financeService)
    {
        _financeService = financeService;
    }

    partial void OnEmployeeIdChanged(Guid value)
    {
        _ = LoadAsync();
    }

    partial void OnEmployeeIdParameterChanged(string? value)
    {
        EmployeeId = Guid.TryParse(value, out var id) ? id : Guid.Empty;
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

    partial void OnFromDateChanged(DateTime value)
    {
        Year = value.Year;
        Month = value.Month;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (EmployeeId == Guid.Empty || Year == 0 || Month == 0)
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
            DailyItems.Clear();
            var details = await _financeService.GetEmployeePayrollAccrualsAsync(EmployeeId, Year, Month);
            if (details is null)
            {
                return;
            }

            EmployeeName = details.EmployeeName;
            Position = details.PositionName;
            fromDate = new DateTime(Year, Month, 1);
            toDate = fromDate.AddMonths(1).AddDays(-1);
            TotalBaseAmount = details.TotalBaseAmount;
            TotalPremiumAmount = details.TotalPremiumAmount;
            TotalAmount = details.TotalAmount;
            PaidAmount = details.PaidAmount;
            RemainingAmount = details.RemainingAmount;

            foreach (var day in details.Days)
            {
                DailyItems.Add(day);
            }

            TotalHours = DailyItems.Sum(x => x.HoursWorked);
            TotalQtyPlanned = DailyItems.Sum(x => x.QtyPlanned);
            TotalQtyProduced = DailyItems.Sum(x => x.QtyProduced);
            TotalQtyExtra = DailyItems.Sum(x => x.QtyExtra);
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
    private async Task PayAsync()
    {
        if (EmployeeId == Guid.Empty)
        {
            return;
        }

        if (!decimal.TryParse(PayoutAmount, out var amount) || amount <= 0)
        {
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", "¬‚Â‰ËÚÂ ÒÛÏÏÛ", "Œ ");
            return;
        }

        try
        {
            var request = new CreatePayrollPaymentRequest(EmployeeId, DateOnly.FromDateTime(DateTime.Today), amount);
            await _financeService.CreatePayrollPaymentAsync(request);
            PayoutAmount = string.Empty;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
    }
}

