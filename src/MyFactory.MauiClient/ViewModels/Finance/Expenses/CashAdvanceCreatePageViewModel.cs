using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Advances;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Services.Advances;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Finance.Expenses;

public partial class CashAdvanceCreatePageViewModel : ObservableObject
{
    private readonly IAdvancesService _advancesService;
    private readonly IEmployeesService _employeesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<EmployeeListItemResponse> Employees { get; } = new();

    [ObservableProperty]
    private EmployeeListItemResponse? selectedEmployee;

    [ObservableProperty]
    private DateTime issueDate = DateTime.Today;

    [ObservableProperty]
    private string amount = string.Empty;

    [ObservableProperty]
    private string? description;

    public CashAdvanceCreatePageViewModel(IAdvancesService advancesService, IEmployeesService employeesService)
    {
        _advancesService = advancesService;
        _employeesService = employeesService;
        _ = LoadEmployeesAsync();
    }

    private async Task LoadEmployeesAsync()
    {
        try
        {
            Employees.Clear();
            var employees = await _employeesService.GetListAsync();
            if (employees is not null)
            {
                foreach (var employee in employees)
                {
                    Employees.Add(employee);
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedEmployee is null)
        {
            await Shell.Current.DisplayAlertAsync("Сотрудник", "Выберите сотрудника", "ОК");
            return;
        }

        if (!decimal.TryParse(Amount, out var amountValue) || amountValue <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Сумма", "Введите корректную сумму", "ОК");
            return;
        }

        try
        {
            var request = new CreateCashAdvanceRequest(SelectedEmployee.Id, DateOnly.FromDateTime(IssueDate), amountValue, Description);
            await _advancesService.IssueAsync(request);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}

