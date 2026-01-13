using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeDetailsPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;

    [ObservableProperty]
    private Guid? employeeId;

    [ObservableProperty]
    private string? employeeIdParameter;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string department = string.Empty;

    [ObservableProperty]
    private string position = string.Empty;

    [ObservableProperty]
    private string hiredAt = string.Empty;

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private string grade = string.Empty;

    [ObservableProperty]
    private string premiumPercent = string.Empty;

    [ObservableProperty]
    private DateTime timesheetPeriod = DateTime.Today;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<AssignmentItemViewModel> CurrentTasks { get; } = new();
    public ObservableCollection<TimesheetEntryItemViewModel> TimesheetEntries { get; } = new();

    public EmployeeDetailsPageViewModel(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
        _ = LoadAsync();
    }

    partial void OnEmployeeIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnEmployeeIdParameterChanged(string? value)
    {
        EmployeeId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnTimesheetPeriodChanged(DateTime value)
    {
        _ = LoadTimesheetAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy || EmployeeId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var details = await _employeesService.GetDetailsAsync(EmployeeId.Value);
            if (details is not null)
            {
                FullName = details.FullName;
                Department = details.Department.Name;
                Position = details.Position.Name;
                HiredAt = details.HiredAt.ToString("dd.MM.yyyy");
                Status = details.IsActive ? "Активен" : "Неактивен";
                Grade = details.Grade.ToString();
                PremiumPercent = details.PremiumPercent?.ToString() ?? "0";
            }

            await LoadAssignmentsAsync();
            await LoadTimesheetAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadAssignmentsAsync()
    {
        if (EmployeeId is null)
            return;

        CurrentTasks.Clear();
        var assignments = await _employeesService.GetAssignmentsAsync(EmployeeId.Value);
        foreach (var a in assignments ?? Array.Empty<EmployeeProductionAssignmentResponse>())
        {
            CurrentTasks.Add(new AssignmentItemViewModel(a));
        }
    }

    private async Task LoadTimesheetAsync()
    {
        if (EmployeeId is null)
            return;

        TimesheetEntries.Clear();
        var entries = await _employeesService.GetEmployeeTimesheetAsync(EmployeeId.Value, TimesheetPeriod.Year, TimesheetPeriod.Month);
        foreach (var e in entries ?? Array.Empty<EmployeeTimesheetEntryResponse>())
        {
            TimesheetEntries.Add(new TimesheetEntryItemViewModel(e));
        }
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        await Shell.Current.DisplayAlertAsync("Инфо", "Редактирование сотрудника недоступно в этой версии", "OK");
    }

    [RelayCommand]
    private async Task DeactivateAsync()
    {
        if (EmployeeId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Деактивировать", "Деактивировать сотрудника?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _employeesService.DeactivateAsync(EmployeeId.Value, new DeactivateEmployeeRequest(DateTime.UtcNow));
            Status = "Неактивен";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task AccrualsAsync()
    {
        await Shell.Current.DisplayAlertAsync("Инфо", "Переход к начислениям пока не реализован", "OK");
    }

    [RelayCommand]
    private async Task PaymentsAsync()
    {
        await Shell.Current.DisplayAlertAsync("Инфо", "Переход к выплатам пока не реализован", "OK");
    }

    [RelayCommand]
    private async Task OpenProductionOrderAsync(AssignmentItemViewModel? assignment)
    {
        if (assignment is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", assignment.ProductionOrderId.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage), parameters);
    }

    [RelayCommand]
    private async Task AddTimesheetEntryAsync()
    {
        if (EmployeeId is null)
            return;

        var hoursInput = await Shell.Current.DisplayPromptAsync("Табель", "Часы за день", "Сохранить", "Отмена", keyboard: Keyboard.Numeric);
        if (string.IsNullOrWhiteSpace(hoursInput) || !decimal.TryParse(hoursInput, out var hours) || hours <= 0)
            return;

        var comment = await Shell.Current.DisplayPromptAsync("Табель", "Комментарий (необязательно)", "Сохранить", "Пропустить", "", maxLength: 200);

        try
        {
            IsBusy = true;
            var date = new DateOnly(TimesheetPeriod.Year, TimesheetPeriod.Month, 1);
            var request = new AddTimesheetEntryRequest(date, hours, string.IsNullOrWhiteSpace(comment) ? null : comment);
            await _employeesService.AddTimesheetEntryAsync(EmployeeId.Value, request);
            await LoadTimesheetAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public sealed class AssignmentItemViewModel
    {
        public Guid ProductionOrderId { get; }
        public string ProductionOrder { get; }
        public string Stage { get; }
        public int Assigned { get; }
        public int Completed { get; }

        public AssignmentItemViewModel(EmployeeProductionAssignmentResponse response)
        {
            ProductionOrderId = response.ProductionOrderId;
            ProductionOrder = response.ProductionOrderNumber;
            Stage = response.Stage.ToString();
            Assigned = response.QtyAssigned;
            Completed = response.QtyCompleted;
        }
    }

    public sealed class TimesheetEntryItemViewModel
    {
        public string Date { get; }
        public string Hours { get; }
        public string Comment { get; }

        public TimesheetEntryItemViewModel(EmployeeTimesheetEntryResponse response)
        {
            Date = response.Date.ToString("dd.MM.yyyy");
            Hours = response.Hours.ToString();
            Comment = response.Comment ?? string.Empty;
        }
    }
}

