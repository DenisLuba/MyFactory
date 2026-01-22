using System;
using System.Collections.Generic;
using System.Globalization;
using MyFactory.MauiClient.Common;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Services.Employees;
using MyFactory.MauiClient.Services.Positions;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeDetailsPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;
    private readonly IPositionsService _positionsService;
    private decimal ratePerNormHour;

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

    [ObservableProperty]
    private DateOnly hiredAt = DateOnly.FromDateTime(DateTime.Now);

    [ObservableProperty]
    private DateOnly? firedAt;

    [ObservableProperty]
    private string status = Active;

    [ObservableProperty]
    private string grade = string.Empty;

    [ObservableProperty]
    private string premiumPercent = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public bool IsEditMode => EmployeeId.HasValue;

    public ICollection<string> Departments { get; } = new List<string>();
    public ICollection<string> Positions { get; } = new List<string>();

    public IReadOnlyCollection<string> Statuses { get; } = [ Active, Inactive ];

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

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            if (EmployeeId is not null)
            {
                var details = await _employeesService.GetDetailsAsync(EmployeeId.Value);
                if (details is not null)
                {
                    ratePerNormHour = details.RatePerNormHour;
                    FullName = details.FullName;
                    Department = details.Department.Name;
                    Position = details.Position.Name;
                    HiredAt = details.HiredAt;
                    FiredAt = details.FiredAt;
                    Status = details.IsActive ? Active : Inactive;
                    Grade = details.Grade.ToString();
                    PremiumPercent = details.PremiumPercent?.ToString() ?? "0";
                }
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

        if (!int.TryParse(Grade, NumberStyles.Integer, CultureInfo.CurrentCulture, out var gradeValue))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Введите корректный разряд.", "OK");
            return;
        }
        
        var premiumValue = string.IsNullOrWhiteSpace(PremiumPercent) ? 0m : PremiumPercent.StringToDecimal();

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

            if (string.IsNullOrWhiteSpace(Position))
            {
                await Shell.Current.DisplayAlertAsync("Ошибка", "Выберите должность.", "OK");
                return;
            }

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
}

