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
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeDetailsPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;
    private readonly IPositionsService _positionsService;
    private readonly IDepartmentsService _departmentsService;

    public const string Active = "В штате";
    public const string Inactive = "Не в штате";

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

    partial void OnSelectedPositionChanged(LookupItem? value)
    {
        Position = value?.Name ?? string.Empty;
    }

    [ObservableProperty]
    private DateOnly hiredAt = DateOnly.FromDateTime(DateTime.Now);

    [ObservableProperty]
    private DateOnly? firedAt;

    [ObservableProperty]
    private string status = Active;

    [ObservableProperty]
    private string grade = string.Empty;

    [ObservableProperty]
    private string ratePerHour = string.Empty;

    [ObservableProperty]
    private string premiumPercent = string.Empty;

    partial void OnSelectedDepartmentChanged(LookupItem? value)
    {
        Department = value?.Name ?? string.Empty;
    }

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public bool IsEditMode => EmployeeId.HasValue;

    public record LookupItem(Guid Id, string Name);

    public ObservableCollection<LookupItem> Departments { get; } = [];

    public ObservableCollection<LookupItem> Positions { get; } = [];

    [ObservableProperty]
    private LookupItem? selectedDepartment;

    [ObservableProperty]
    private LookupItem? selectedPosition;

    public IReadOnlyCollection<string> Statuses { get; } = [ Active, Inactive ];

    public EmployeeDetailsPageViewModel(IEmployeesService employeesService, IPositionsService positionsService, IDepartmentsService departmentService)
    {
        _employeesService = employeesService;
        _positionsService = positionsService;
        _departmentsService = departmentService;
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

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Guid? positionId = null;
            Guid? departmentId = null;
            if (EmployeeId is not null)
            {
                var details = await _employeesService.GetDetailsAsync(EmployeeId.Value);
                if (details is not null)
                {
                    RatePerHour = details.RatePerNormHour.ToString();
                    FullName = details.FullName;
                    Department = details.Department.Name;
                    Position = details.Position.Name;
                    HiredAt = details.HiredAt;
                    FiredAt = details.FiredAt;
                    Status = details.IsActive ? Active : Inactive;
                    Grade = details.Grade.ToString();
                    PremiumPercent = details.PremiumPercent?.ToString() ?? "0";
                    positionId = details.Position.Id;
                    departmentId = details.Department.Id;
                }
            }

            Positions.Clear();

            var positionsList = (await _positionsService.GetListAsync())?.Select(p => new LookupItem(p.Id, p.Name)).ToList();
            if (positionsList is not null)
            {
                foreach (var pos in positionsList)
                {
                    Positions.Add(pos);
                }

                SelectedPosition = Positions.FirstOrDefault(p => positionId.HasValue && p.Id == positionId.Value) ?? Positions.FirstOrDefault();
            }

            Departments.Clear();

            var departmentsList = (await _departmentsService.GetListAsync())?.Select(d => new LookupItem(d.Id, d.Name)).ToList();
            if (departmentsList is not null)
            {
                foreach (var dept in departmentsList)
                {
                    Departments.Add(dept);
                }

                SelectedDepartment = Departments.FirstOrDefault(d => departmentId.HasValue && d.Id == departmentId.Value) ?? Departments.FirstOrDefault();
            }
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
    private async Task OpenTimesheetAsync()
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
        await Shell.Current.GoToAsync(nameof(EmployeeTimesheetPage), parameters);
    }

    [RelayCommand]
    private async Task OpenAssignmentsAsync()
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
        await Shell.Current.GoToAsync(nameof(EmployeeAssignmentsPage), parameters);
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

        if (SelectedPosition is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Выберите должность.", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var positionId = SelectedPosition.Id;

            if (!int.TryParse(Grade, NumberStyles.Integer, CultureInfo.CurrentCulture, out var gradeValue))
            {
                gradeValue = 0;
            }

            var ratePerNormHour = string.IsNullOrEmpty(RatePerHour) ? 0m : RatePerHour.StringToDecimal();
            var premiumValue = string.IsNullOrWhiteSpace(PremiumPercent) ? 0m : PremiumPercent.StringToDecimal();

            var isActive = string.IsNullOrWhiteSpace(Status) || string.Equals(Status, Active, StringComparison.OrdinalIgnoreCase);
            var hiredAtDate = HiredAt.ToDateTime(TimeOnly.MinValue);

            if (EmployeeId.HasValue)
            {
                var request = new UpdateEmployeeRequest(
                    FullName: FullName,
                    PositionId: positionId,
                    Grade: gradeValue,
                    RatePerNormHour: ratePerNormHour,
                    PremiumPercent: premiumValue,
                    HiredAt: hiredAtDate,
                    IsActive: isActive);

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
}

