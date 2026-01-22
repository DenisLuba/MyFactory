using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

[QueryProperty(nameof(EmployeeIdParameter), "EmployeeId")]
public partial class EmployeeAssignmentsPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;

    private Guid? employeeId;
    private string? employeeIdParameter;
    private bool isBusy;
    private string? errorMessage;

    public ObservableCollection<AssignmentItemViewModel> CurrentTasks { get; } = new();

    public EmployeeAssignmentsPageViewModel(IEmployeesService employeesService)
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

            CurrentTasks.Clear();
            var assignments = await _employeesService.GetAssignmentsAsync(EmployeeId.Value);
            foreach (var a in assignments ?? [])
            {
                CurrentTasks.Add(new AssignmentItemViewModel(a));
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
}
