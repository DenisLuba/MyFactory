using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeAssignmentsPageViewModel(IEmployeesService employeesService) : ObservableObject
{
    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? employeeId;
    [ObservableProperty] private string? employeeName;
    [ObservableProperty] private string? employeeIdParameter;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    #endregion

    #region OBSERVABLE COLLECTION
    public ObservableCollection<AssignmentItemViewModel> CurrentTasks { get; } = new();
    #endregion

    #region ON PROPERTY CHANGED
    partial void OnEmployeeIdParameterChanged(string? value)
    {
        EmployeeId = Guid.TryParse(value, out var id) ? id : null;
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

        CurrentTasks.Clear();
        
        var details = await employeesService.GetDetailsAsync(EmployeeId.Value);
        EmployeeName = details?.FullName;

        var assignments = await employeesService.GetAssignmentsAsync(EmployeeId.Value);
        foreach (var a in assignments ?? [])
        {
            CurrentTasks.Add(new AssignmentItemViewModel(a));
        }
    }
    #endregion

    #region OPEN PRODUCTION ORDER
    [RelayCommand]
    private async Task OpenProductionOrderAsync(AssignmentItemViewModel? assignment) => await RunSafeActionAsync(async () =>
    {
        if (assignment is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", assignment.ProductionOrderId.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage), parameters);
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

    #region AssignmentItemViewModel
    public sealed class AssignmentItemViewModel(EmployeeProductionAssignmentResponse response)
    {
        public Guid ProductionOrderId { get; } = response.ProductionOrderId;
        public string ProductionOrder { get; } = response.ProductionOrderNumber;
        public string Stage { get; } = response.Stage.ToString();
        public decimal Assigned { get; } = response.QtyAssigned;
        public decimal Completed { get; } = response.QtyCompleted;
    }
    #endregion
}
