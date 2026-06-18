using System;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeTimesheetPageViewModel(IEmployeesService employeesService) : ObservableObject
{
    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? employeeId;
    [ObservableProperty] private string? employeeName;
    [ObservableProperty] private string? employeeIdParameter;
    [ObservableProperty] private int? yearFilter = DateTime.Today.Year;
    [ObservableProperty] private int? monthFilter = DateTime.Today.Month - 1;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    #endregion

    #region OBSERVABLE COLLECTION
    public ObservableCollection<TimesheetEntryItemViewModel> TimesheetEntries { get; } = new();
    public IReadOnlyList<int> Years { get; } = Enumerable.Range(DateTime.Today.Year - 10, 21).ToList();
    public IReadOnlyList<string> Months { get; } = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();
    #endregion

    #region ON PROPERTY CHANGED
    partial void OnEmployeeIdParameterChanged(string? value)
    {
        EmployeeId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnYearFilterChanged(int? value)
    {
        if (EmployeeId.HasValue && !IsBusy)
            _ = LoadDataAsync();
    }

    partial void OnMonthFilterChanged(int? value)
    {
        if (EmployeeId.HasValue && !IsBusy)
            _ = LoadDataAsync();
    }
    #endregion

    #region LOAD COMMAND
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(LoadDataAsync);
    #endregion

    #region LOAD DATA
    private async Task LoadDataAsync()
    {
        if (EmployeeId is null)
            return;

        TimesheetEntries.Clear();

        if (string.IsNullOrWhiteSpace(EmployeeName))
        {
            var employee = await employeesService.GetDetailsAsync(EmployeeId.Value);
            EmployeeName = employee?.FullName ?? "Сотрудник";
        }

        var selectedYear = YearFilter ?? DateTime.Today.Year;
        var selectedMonth = (MonthFilter ?? (DateTime.Today.Month - 1)) + 1;
        var entries = await employeesService.GetEmployeeTimesheetAsync(EmployeeId.Value, selectedYear, selectedMonth);
        foreach (var e in entries ?? [])
        {
            TimesheetEntries.Add(new TimesheetEntryItemViewModel(e));
        }
    }
    #endregion

    #region ADD TIMESHEET ENTRY
    [RelayCommand]
    private async Task AddTimesheetEntryAsync() => await RunSafeActionAsync(async () =>
    {
        if (EmployeeId is null)
            return;

        var hoursInput = await Shell.Current.DisplayPromptAsync(
            title: "Внимание", 
            message: "Введите количество часов", 
            accept: "ОК", 
            cancel: "Отмена", 
            keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(hoursInput) || !decimal.TryParse(hoursInput, out var hours) || hours <= 0)
            return;

        var comment = await Shell.Current.DisplayPromptAsync(
            title: "Комментарий", 
            message: "Введите комментарий (необязательно)", 
            accept: "ОК", 
            cancel: "Отмена", 
            initialValue: "", 
            maxLength: 200);
        
        var selectedYear = YearFilter ?? DateTime.Today.Year;
        var selectedMonth = (MonthFilter ?? (DateTime.Today.Month - 1)) + 1;
        var date = new DateOnly(selectedYear, selectedMonth, 1);
        var request = new AddTimesheetEntryRequest(date, hours, string.IsNullOrWhiteSpace(comment) ? null : comment);
        await employeesService.AddTimesheetEntryAsync(EmployeeId.Value, request);
        await LoadDataAsync();
    });
    #endregion

    #region BACK
    [RelayCommand]
    private async Task BackAsync()
    {
        var parameters = new Dictionary<string, object>();
        if (EmployeeId.HasValue)
            parameters.Add("EmployeeId", EmployeeId.Value.ToString());

        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(EmployeeDetailsPage), parameters: parameters);
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

    #region TimesheetEntryItemViewModel
    public sealed class TimesheetEntryItemViewModel(EmployeeTimesheetEntryResponse response)
    {
        public string Date { get; } = response.Date.ToString("dd.MM.yyyy");
        public string Hours { get; } = response.Hours.ToString();
        public string Comment { get; } = response.Comment ?? string.Empty;
    }
    #endregion
}
