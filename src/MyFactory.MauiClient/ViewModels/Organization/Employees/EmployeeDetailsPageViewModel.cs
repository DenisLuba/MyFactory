using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using MyFactory.MauiClient.Common;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Positions;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeDetailsPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;
    private readonly IPositionsService _positionsService;
    private Guid? positionId;
    private decimal ratePerNormHour;

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
    private DateOnly hiredAt = DateOnly.FromDateTime(DateTime.Now);

    [ObservableProperty]
    private DateOnly? firedAt;

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

    public bool IsEditMode => EmployeeId.HasValue;

    public ICollection<string> Positions { get; } = new List<string>();

    public IReadOnlyCollection<string> Statuses { get; } = [ "Активен", "Неактивен" ];

    public ObservableCollection<AssignmentItemViewModel> CurrentTasks { get; } = new();
    public ObservableCollection<TimesheetEntryItemViewModel> TimesheetEntries { get; } = new();

    public EmployeeDetailsPageViewModel(IEmployeesService employeesService, IPositionsService positionsService)
    {
        _employeesService = employeesService;
        _positionsService = positionsService;
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

            if (EmployeeId is null)
                return;

            positionId = await GetPositionIdByNameAsync(Position);

            var details = await _employeesService.GetDetailsAsync(EmployeeId.Value);
            if (details is not null)
            {
                positionId = details.Position.Id;
                ratePerNormHour = details.RatePerNormHour;
                FullName = details.FullName;
                Department = details.Department.Name;
                Position = details.Position.Name;
                HiredAt = details.HiredAt;
                FiredAt = details.FiredAt;
                Status = details.IsActive ? "Активен" : "Неактивен";
                Grade = details.Grade.ToString();
                PremiumPercent = details.PremiumPercent?.ToString() ?? "0";
            }

            Positions.Clear();

            var positionsList = (await _positionsService.GetListAsync())?.Select(p => p.Name).ToList();
            if (positionsList is not null)
            {
                foreach (var pos in positionsList)
                {
                    Positions.Add(pos);
                }
            }

            await LoadAssignmentsAsync();
            await LoadTimesheetAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
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
    private async Task DeactivateAsync()
    {
        if (EmployeeId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Подтверждение.", "Вы действительно хотите уволить сотрудника?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _employeesService.DeactivateAsync(EmployeeId.Value, new DeactivateEmployeeRequest(DateTime.UtcNow));
            Status = "Неактивен";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task AccrualsAsync()
    {
        await Shell.Current.DisplayAlertAsync("����", "������� � ����������� ���� �� ����������", "OK");
    }

    [RelayCommand]
    private async Task PaymentsAsync()
    {
        await Shell.Current.DisplayAlertAsync("����", "������� � �������� ���� �� ����������", "OK");
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

        var hoursInput = await Shell.Current.DisplayPromptAsync("Часы", "Введите часы", "Сохранить", "Отмена", keyboard: Keyboard.Numeric);
        if (string.IsNullOrWhiteSpace(hoursInput) || !decimal.TryParse(hoursInput, out var hours) || hours <= 0)
            return;

        var comment = await Shell.Current.DisplayPromptAsync("������", "����������� (�������������)", "���������", "����������", "", maxLength: 200);

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
            await Shell.Current.DisplayAlertAsync("������", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(FullName))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите ФИО сотрудника.", "OK");
            return;
        }

        if (!int.TryParse(Grade, NumberStyles.Integer, CultureInfo.CurrentCulture, out var gradeValue))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Введите корректный разряд.", "OK");
            return;
        }

        var premiumValue = PremiumPercent?.StringToDecimal() ?? 0m;

        if (ratePerNormHour <= 0)
        {
            var rateInput = await Shell.Current.DisplayPromptAsync("Ставка", "Введите ставку за нормо-час", "Сохранить", "Отмена", keyboard: Keyboard.Numeric);
            if (string.IsNullOrWhiteSpace(rateInput) || !decimal.TryParse(rateInput, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsedRate) || parsedRate <= 0)
            {
                await Shell.Current.DisplayAlertAsync("Ошибка", "Некорректная ставка за нормо-час.", "OK");
                return;
            }

            ratePerNormHour = parsedRate;
        }

        var isActive = string.IsNullOrWhiteSpace(Status) || string.Equals(Status, "Активен", StringComparison.OrdinalIgnoreCase);
        var hiredAtDate = HiredAt.ToDateTime(TimeOnly.MinValue);

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var positionId = await GetPositionIdByNameAsync(Position);

            if (EmployeeId.HasValue)
            {
                var request = new UpdateEmployeeRequest(
                    FullName,
                    positionId,
                    gradeValue,
                    ratePerNormHour,
                    premiumValue,
                    hiredAtDate,
                    isActive);

                await _employeesService.UpdateAsync(EmployeeId.Value, request);
            }
            else
            {
                var request = new CreateEmployeeRequest(
                    FullName,
                    positionId,
                    gradeValue,
                    ratePerNormHour,
                    premiumValue,
                    hiredAtDate,
                    isActive);

                var response = await _employeesService.CreateAsync(request);
                EmployeeId = response?.Id;
            }

            await Shell.Current.DisplayAlertAsync("Готово", "Данные сохранены.", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task<Guid> GetPositionIdByNameAsync(string positionName)
    {
        var positions = await _positionsService.GetListAsync();
        var position = positions?.FirstOrDefault(p => string.Equals(p.Name, positionName, StringComparison.OrdinalIgnoreCase));
        if (position is null)
            throw new InvalidOperationException("Должность не найдена.");

        return position.Id;
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

