using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.Models.PayrollRules;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Finance;
using MyFactory.MauiClient.Services.PayrollRules;

namespace MyFactory.MauiClient.ViewModels.Finance.Payroll;

public partial class PayrollAccrualsPageViewModel : PagedListViewModel<PayrollAccrualListItemResponse>
{
    private readonly IFinanceService _financeService;
    private readonly IPayrollRulesService _payrollRulesService;
    private readonly IEmployeesService _employeesService;
    private readonly IDepartmentsService _departmentsService;
    private bool _lookupsLoaded;

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
    }

    public async Task InitializeAsync()
    {
        if (!_lookupsLoaded)
        {
            await LoadLookupsAsync();
            _lookupsLoaded = true;
        }

        await LoadAsync();
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
            if (employees is not null && employees.Items.Count > 0)
            {
                foreach (var emp in employees.Items)
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
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
    }

    protected override async Task SetResponse()
    {
        _response = await _financeService.GetPayrollAccrualsAsync(
            from: DateOnly.FromDateTime(FromDate),
            to: DateOnly.FromDateTime(ToDate),
            employeeId: SelectedEmployee?.Id,
            departmentId: SelectedDepartment?.Id,
            sortBy: SortBy,
            sortDesc: SortDesc,
            skip: Skip,
            take: PageSize);
    }

    partial void OnFromDateChanged(DateTime value)
    {
        if (!_lookupsLoaded)
            return;

        _ = LoadAsync();
    }

    partial void OnToDateChanged(DateTime value)
    {
        if (!_lookupsLoaded)
            return;

        _ = LoadAsync();
    }

    partial void OnSelectedEmployeeChanged(EmployeeListItemResponse? value)
    {
        if (!_lookupsLoaded)
            return;

        _ = LoadAsync();
    }

    partial void OnSelectedDepartmentChanged(DepartmentListItemResponse? value)
    {
        if (!_lookupsLoaded)
            return;

        _ = LoadAsync();
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
    private async Task EditPayrollRuleAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.Finance.Payroll.PayrollRulesListPage));
    }
}
