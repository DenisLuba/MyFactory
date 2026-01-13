using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Organization.Employees;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.ViewModels.Organization.Employees;

public partial class EmployeesListPageViewModel : ObservableObject
{
    private readonly IEmployeesService _employeesService;

    [ObservableProperty]
    private string? searchText;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<EmployeeItemViewModel> Employees { get; } = new();

    public EmployeesListPageViewModel(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
        _ = LoadAsync();
    }

    partial void OnSearchTextChanged(string? value)
    {
        _ = LoadAsync();
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
            Employees.Clear();

            var items = await _employeesService.GetListAsync(SearchText);
            foreach (var emp in items ?? Array.Empty<EmployeeListItemResponse>())
            {
                Employees.Add(new EmployeeItemViewModel(emp));
            }
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

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(EmployeeDetailsPage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(EmployeeItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "EmployeeId", item.Id.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(EmployeeDetailsPage), parameters);
    }

    [RelayCommand]
    private Task EditAsync(EmployeeItemViewModel? item) => OpenDetailsAsync(item);

    [RelayCommand]
    private async Task DeactivateAsync(EmployeeItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Деактивировать", $"Деактивировать сотрудника {item.FullName}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _employeesService.DeactivateAsync(item.Id, new DeactivateEmployeeRequest(DateTime.UtcNow));
            item.IsActive = false;
            item.RaisePropertyChanges();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
    }

    public sealed class EmployeeItemViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string FullName { get; }
        public string Department { get; }
        public string Position { get; }

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }

        public EmployeeItemViewModel(EmployeeListItemResponse response)
        {
            Id = response.Id;
            FullName = response.FullName;
            Department = response.DepartmentName;
            Position = response.PositionName;
            isActive = response.IsActive;
        }

        public void RaisePropertyChanges() => OnPropertyChanged(nameof(IsActive));
    }
}

