using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Positions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeDetailsPageViewModel(IEmployeesService employeesService, IPositionsService positionsService, IDepartmentsService departmentsService) : ObservableObject
{
    #region COLLECTIONS
    private readonly List<LookupItem> allDepartments = new();
    private readonly List<(Guid Id, string Name, Guid DepartmentId)> allPositions = new();
    public IReadOnlyCollection<string> Statuses { get; } = [Active, Inactive];
    #endregion
    #region CONSTANTS
    public const string Active = "В штате";
    public const string Inactive = "Не в штате";
    #endregion

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? employeeId;
    [ObservableProperty] private string? employeeIdParameter;
    [ObservableProperty] private bool isEditMode = false;
    [ObservableProperty] private string fullName = string.Empty;
    [ObservableProperty] private string department = string.Empty;
    [ObservableProperty] private string position = string.Empty;
    [ObservableProperty] private DateOnly hiredAt = DateOnly.FromDateTime(DateTime.Now);
    [ObservableProperty] private DateOnly? firedAt;
    [ObservableProperty] private string status = Active;
    [ObservableProperty] private string grade = string.Empty;
    [ObservableProperty] private string ratePerHour = string.Empty;
    [ObservableProperty] private string premiumPercent = string.Empty;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private LookupItem? selectedDepartment;
    [ObservableProperty] private LookupItem? selectedPosition;
    #endregion

    #region OBSERVABLE COLLECTIONS
    public ObservableCollection<LookupItem> Departments { get; } = [];
    public ObservableCollection<LookupItem> Positions { get; } = [];
    #endregion


    #region ON PROPERTIES CHANGED
    partial void OnSelectedPositionChanged(LookupItem? value)
    {
        Position = value?.Name ?? string.Empty;
    }

    partial void OnSelectedDepartmentChanged(LookupItem? value)
    {
        Department = value?.Name ?? string.Empty;

        ApplyPositionsFilter(value?.Id, SelectedPosition?.Id);

    }

    partial void OnEmployeeIdChanged(Guid? value)
    {
        if (!IsBusy)
            _ = LoadAsync();
    }

    partial void OnEmployeeIdParameterChanged(string? value)
    {
        EmployeeId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region LOAD DATA
    private async Task LoadDataAsync()
    {
        Guid? positionId = null;
        Guid? departmentId = null;
        if (EmployeeId is not null)
        {
            IsEditMode = true;

            var details = await employeesService.GetDetailsAsync(EmployeeId.Value);
            if (details is not null)
            {
                RatePerHour = details.RatePerNormHour?.ToString() ?? "";
                FullName = details.FullName;
                Department = details.Department.Name;
                Position = details.Position.Name;
                HiredAt = details.HiredAt;
                FiredAt = details.FiredAt;
                Status = details.IsActive ? Active : Inactive;
                Grade = details.Grade?.ToString() ?? "";
                PremiumPercent = details.PremiumPercent?.ToString() ?? "";
                positionId = details.Position.Id;
                departmentId = details.Department.Id;
            }
        }

        Positions.Clear();

        var positionsFull = (await positionsService.GetListAsync(includeInactive: true))?
            .Select(p => (p.Id, p.Name, p.DepartmentId)).ToList();
        allPositions.Clear();
        if (positionsFull is not null)
        {
            allPositions.AddRange(positionsFull);
        }

        var departmentsFull = (await departmentsService.GetListAsync(includeInactive: true))?
            .Select(d => new LookupItem(d.Id, d.Name)).ToList();
        allDepartments.Clear();
        if (departmentsFull is not null)
        {
            allDepartments.AddRange(departmentsFull);
        }

        ApplyDepartmentFilter(departmentId);
        ApplyPositionsFilter(departmentId, positionId);


        Department = SelectedDepartment?.Name ?? string.Empty;
        Position = SelectedPosition?.Name ?? string.Empty;
    }
    #endregion

    #region LOAD
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(LoadDataAsync);
    #endregion

    #region APPLY DEPARTMENT FITLER
    private void ApplyDepartmentFilter(Guid? departmentId)
    {
        Departments.Clear();

        foreach (var item in allDepartments)
        {
            Departments.Add(item);
        }

        SelectedDepartment = Departments.FirstOrDefault(d => departmentId.HasValue && d.Id == departmentId.Value) ?? Departments.FirstOrDefault();
    }
    #endregion

    #region APPLY POSITIONS FILTER
    private void ApplyPositionsFilter(Guid? departmentId, Guid? positionId)
    {
        Positions.Clear();

        IEnumerable<(Guid Id, string Name, Guid DepartmentId)> source = allPositions;
        if (departmentId.HasValue)
        {
            source = source.Where(p => p.DepartmentId == departmentId.Value);
        }

        foreach (var pos in source)
        {
            Positions.Add(new LookupItem(pos.Id, pos.Name));
        }

        SelectedPosition = Positions.FirstOrDefault(p => positionId.HasValue && p.Id == positionId.Value)
            ?? Positions.FirstOrDefault();
    }
    #endregion


    #region DEACTIVATE
    [RelayCommand]
    private async Task DeactivateAsync() => await RunSafeActionAsync(async () =>
    {
        if (EmployeeId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Подтверждение.", "Вы действительно хотите уволить сотрудника?", "Да", "Нет");
        if (!confirm)
            return;

        await employeesService.DeactivateAsync(EmployeeId.Value, new DeactivateEmployeeRequest(DateTime.UtcNow));
        Status = "Неактивен";

    });
    #endregion

    #region ACCRUALS
    [RelayCommand]
    private async Task AccrualsAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.DisplayAlertAsync("Внимание.", "Действие пока не реализовано.", "OK");
    });
    #endregion

    #region PAYMENTS
    [RelayCommand]
    private async Task PaymentsAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.DisplayAlertAsync("Внимание.", "Действие пока не реализовано.", "OK");
    });
    #endregion

    #region OPEN TIMESHEET
    [RelayCommand]
    private async Task OpenTimesheetAsync() => await OpenAsync(nameof(EmployeeTimesheetPage));
    #endregion

    #region OPEN ASSIGNMENTS
    [RelayCommand]
    private async Task OpenAssignmentsAsync() => await OpenAsync(nameof(EmployeeAssignmentsPage));
    #endregion

    #region OPEN PAGE
    private async Task OpenAsync(string page) => await RunSafeActionAsync(async () =>
    {
        if (EmployeeId is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Сначала сохраните сотрудника.", "OK");
            return;
        }

        var parameters = new Dictionary<string, object>
        {
            { "EmployeeId", EmployeeId.Value.ToString() }
        };
        await Shell.Current.GoToAsync(page, parameters);
    });
    #endregion

    #region SAVE
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (string.IsNullOrWhiteSpace(FullName))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите ФИО сотрудника.", "OK");
            return;
        }

        if (SelectedPosition is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Выберите должность.", "OK");
            return;
        }

        if (SelectedDepartment is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Выберите цех/отдел.", "OK");
            return;
        }

        var positionId = SelectedPosition.Id;
        var departmentId = SelectedDepartment.Id;

        int? gradeValue = null;
        if (int.TryParse(Grade, NumberStyles.Integer, CultureInfo.CurrentCulture, out var parsedGrade))
        {
            gradeValue = parsedGrade;
        }

        decimal? ratePerNormHour = string.IsNullOrEmpty(RatePerHour) ? (decimal?)null : RatePerHour.StringToDecimal();
        decimal? premiumValue = string.IsNullOrWhiteSpace(PremiumPercent) ? null : PremiumPercent.StringToDecimal();

        var isActive = string.IsNullOrWhiteSpace(Status) || string.Equals(Status, Active, StringComparison.OrdinalIgnoreCase);
        var hiredAtDate = HiredAt.ToDateTime(TimeOnly.MinValue);

        if (EmployeeId.HasValue)
        {
            var request = new UpdateEmployeeRequest(
                FullName: FullName,
                PositionId: positionId,
                DepartmentId: departmentId,
                Grade: gradeValue,
                RatePerNormHour: ratePerNormHour,
                PremiumPercent: premiumValue,
                HiredAt: hiredAtDate,
                IsActive: isActive);

            await employeesService.UpdateAsync(EmployeeId.Value, request);
        }
        else
        {
            var request = new CreateEmployeeRequest(
                FullName,
                positionId,
                departmentId,
                gradeValue,
                ratePerNormHour,
                premiumValue,
                hiredAtDate,
                isActive);

            var response = await employeesService.CreateAsync(request);
            EmployeeId = response?.Id;
        }

        await Shell.Current.DisplayAlertAsync("Готово", "Данные сохранены.", "OK");
        await Shell.Current.GoToAsync("..");

    });
    #endregion

    #region BACK
    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(EmployeesListPage));
    }
    #endregion

    #region RunSafeAction
    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region LookupItem
    public record LookupItem(Guid Id, string Name);
    #endregion
}

