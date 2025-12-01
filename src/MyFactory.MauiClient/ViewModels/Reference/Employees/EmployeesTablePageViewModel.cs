using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Pages.Reference.Employees;
using MyFactory.MauiClient.Services.EmployeesServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Employees;

public partial class EmployeesTablePageViewModel : ObservableObject
{
	private readonly IEmployeesService _employeesService;

	public EmployeesTablePageViewModel(IEmployeesService employeesService)
	{
		_employeesService = employeesService;
	}

	public ObservableCollection<EmployeeListResponse> Employees { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasEmployees;

	public bool HasNoEmployees => !HasEmployees;

	[ObservableProperty]
	private string? roleFilter;

	partial void OnHasEmployeesChanged(bool value) => OnPropertyChanged(nameof(HasNoEmployees));

	partial void OnRoleFilterChanged(string? value)
	{
		if (!IsBusy && LoadCommand.CanExecute(null))
		{
			_ = LoadCommand.ExecuteAsync(null);
		}
	}

	[RelayCommand]
	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Employees.Clear();

			var items = await _employeesService.GetEmployeesAsync(RoleFilter);
			if (items is { Count: > 0 })
			{
				foreach (var employee in items.OrderBy(e => e.FullName))
				{
					Employees.Add(employee);
				}
			}

			HasEmployees = Employees.Count > 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить сотрудников: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task OpenCardAsync(EmployeeListResponse? employee)
	{
		if (employee is null)
		{
			return;
		}

		var viewModel = new EmployeeCardPageViewModel(employee.Id, _employeesService);
		var page = new EmployeeCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}
}
