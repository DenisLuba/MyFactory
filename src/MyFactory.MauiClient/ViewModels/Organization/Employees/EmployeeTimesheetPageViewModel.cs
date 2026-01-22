using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeTimesheetPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;

    private Guid? employeeId;
    private string? employeeIdParameter;
    private DateTime timesheetPeriod = DateTime.Today;
    private bool isBusy;
    private string? errorMessage;

    public ObservableCollection<TimesheetEntryItemViewModel> TimesheetEntries { get; } = new();

    public EmployeeTimesheetPageViewModel(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    public Guid? EmployeeId
    {
        get => employeeId;
        set
        {
            if (SetProperty(ref employeeId, value))
            {
                _ = LoadAsync();
            }
        }
    }

    public string? EmployeeIdParameter
    {
        get => employeeIdParameter;
        set
        {
            if (SetProperty(ref employeeIdParameter, value))
            {
                EmployeeId = Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }

    public DateTime TimesheetPeriod
    {
        get => timesheetPeriod;
        set
        {
            if (SetProperty(ref timesheetPeriod, value))
            {
                _ = LoadAsync();
            }
        }
    }

    public bool IsBusy
    {
        get => isBusy;
        set => SetProperty(ref isBusy, value);
    }

    public string? ErrorMessage
    {
        get => errorMessage;
        set => SetProperty(ref errorMessage, value);
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

            TimesheetEntries.Clear();
            var entries = await _employeesService.GetEmployeeTimesheetAsync(EmployeeId.Value, TimesheetPeriod.Year, TimesheetPeriod.Month);
            foreach (var e in entries ?? [])
            {
                TimesheetEntries.Add(new TimesheetEntryItemViewModel(e));
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
    private async Task AddTimesheetEntryAsync()
    {
        if (EmployeeId is null)
            return;

        var hoursInput = await Shell.Current.DisplayPromptAsync("Часы", "Введите часы", "Сохранить", "Отмена", keyboard: Keyboard.Numeric);
        if (string.IsNullOrWhiteSpace(hoursInput) || !decimal.TryParse(hoursInput, out var hours) || hours <= 0)
            return;

        var comment = await Shell.Current.DisplayPromptAsync("Комментарий", "Введите комментарий (необязательно)", "Сохранить", "Отмена", "", maxLength: 200);

        try
        {
            IsBusy = true;
            var date = new DateOnly(TimesheetPeriod.Year, TimesheetPeriod.Month, 1);
            var request = new AddTimesheetEntryRequest(date, hours, string.IsNullOrWhiteSpace(comment) ? null : comment);
            await _employeesService.AddTimesheetEntryAsync(EmployeeId.Value, request);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
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
