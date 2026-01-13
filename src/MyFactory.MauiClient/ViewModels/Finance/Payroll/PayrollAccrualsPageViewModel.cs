using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.Models.PayrollRules;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Finance;
using MyFactory.MauiClient.Services.PayrollRules;

namespace MyFactory.MauiClient.ViewModels.Finance.Payroll;

public partial class PayrollAccrualsPageViewModel : ObservableObject
{
    private readonly IFinanceService _financeService;
    private readonly IPayrollRulesService _payrollRulesService;
    private readonly IEmployeesService _employeesService;
    private readonly IDepartmentsService _departmentsService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private DateTime fromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

    [ObservableProperty]
    private DateTime toDate = DateTime.Today;

    [ObservableProperty]
    private PayrollRuleResponse? selectedRule;

    [ObservableProperty]
    private EmployeeListItemResponse? selectedEmployee;

    [ObservableProperty]
    private DepartmentListItemResponse? selectedDepartment;

    public ObservableCollection<PayrollAccrualListItemResponse> Accruals { get; } = new();
    public ObservableCollection<PayrollRuleResponse> PayrollRules { get; } = new();
    public ObservableCollection<EmployeeListItemResponse> Employees { get; } = new();
    public ObservableCollection<DepartmentListItemResponse> Departments { get; } = new();

    public PayrollAccrualsPageViewModel(
        IFinanceService financeService,
        IPayrollRulesService payrollRulesService,
        IEmployeesService employeesService,
        IDepartmentsService departmentsService)
    {
        _financeService = financeService;
        _payrollRulesService = payrollRulesService;
        _employeesService = employeesService;
        _departmentsService = departmentsService;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await LoadLookupsAsync();
        await LoadAccrualsAsync();
    }

    private async Task LoadLookupsAsync()
    {
        try
        {
            var rules = await _payrollRulesService.GetListAsync() ?? Array.Empty<PayrollRuleResponse>();
            PayrollRules.Clear();
            foreach (var rule in rules.OrderByDescending(r => r.EffectiveFrom))
            {
                PayrollRules.Add(rule);
            }

            var employees = await _employeesService.GetListAsync();
            Employees.Clear();
            if (employees is not null)
            {
                foreach (var emp in employees)
                {
                    Employees.Add(emp);
                }
            }

            var departments = await _departmentsService.GetListAsync();
            Departments.Clear();
            if (departments is not null)
            {
                foreach (var dep in departments)
                {
                    Departments.Add(dep);
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
    }

    [RelayCommand]
    private async Task LoadAccrualsAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            Accruals.Clear();
            var result = await _financeService.GetPayrollAccrualsAsync(
                DateOnly.FromDateTime(FromDate),
                DateOnly.FromDateTime(ToDate),
                SelectedEmployee?.Id,
                SelectedDepartment?.Id);

            if (result is not null)
            {
                foreach (var item in result)
                {
                    Accruals.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnFromDateChanged(DateTime value)
    {
        _ = LoadAccrualsAsync();
    }

    partial void OnToDateChanged(DateTime value)
    {
        _ = LoadAccrualsAsync();
    }

    partial void OnSelectedEmployeeChanged(EmployeeListItemResponse? value)
    {
        _ = LoadAccrualsAsync();
    }

    partial void OnSelectedDepartmentChanged(DepartmentListItemResponse? value)
    {
        _ = LoadAccrualsAsync();
    }

    [RelayCommand]
    private async Task OpenDailyBreakdownAsync(PayrollAccrualListItemResponse? accrual)
    {
        if (accrual is null)
        {
            return;
        }

        var date = FromDate;
        await Shell.Current.GoToAsync(nameof(Pages.Finance.Payroll.PayrollDailyBreakdownPage), new Dictionary<string, object>
        {
            { "EmployeeId", accrual.EmployeeId.ToString() },
            { "Year", date.Year.ToString() },
            { "Month", date.Month.ToString() },
            { "EmployeeName", accrual.EmployeeName }
        });
    }

    [RelayCommand]
    private async Task EditPayrollRuleAsync(PayrollAccrualListItemResponse? accrual)
    {
        await Shell.Current.GoToAsync(nameof(Pages.Finance.Payroll.PayrollRulesListPage));
    }
}
