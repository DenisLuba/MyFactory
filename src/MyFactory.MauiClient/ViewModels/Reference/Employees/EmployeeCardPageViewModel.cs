using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Services.EmployeesServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Employees;

public partial class EmployeeCardPageViewModel : ObservableObject
{
	private readonly IEmployeesService _employeesService;

	public Guid EmployeeId { get; }

	public EmployeeCardPageViewModel(Guid employeeId, IEmployeesService employeesService)
	{
		EmployeeId = employeeId;
		_employeesService = employeesService;
	}

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isSaving;

	[ObservableProperty]
	private string fullName = string.Empty;

	[ObservableProperty]
	private string position = string.Empty;

	[ObservableProperty]
	private string gradeText = string.Empty;

	[ObservableProperty]
	private bool isActive;

	[ObservableProperty]
	private string employeeCode = string.Empty;

	[ObservableProperty]
	private DateOnly hireDate;

	public string HireDateDisplay => HireDate == default ? "-" : HireDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

	partial void OnHireDateChanged(DateOnly value) => OnPropertyChanged(nameof(HireDateDisplay));

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
			var employee = await _employeesService.GetEmployeeAsync(EmployeeId);
			if (employee is null)
			{
				await Shell.Current.DisplayAlertAsync("Ошибка", "Карточка сотрудника не найдена", "OK");
				return;
			}

			MapEmployee(employee);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить данные сотрудника: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		if (IsSaving)
		{
			return;
		}

		if (!int.TryParse(GradeText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var grade) || grade <= 0)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", "Введите корректный разряд (целое число)", "OK");
			return;
		}

		if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Position))
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", "ФИО и должность обязательны", "OK");
			return;
		}

		try
		{
			IsSaving = true;
			var payload = new EmployeeUpdateRequest(
				FullName.Trim(),
				Position.Trim(),
				grade,
				IsActive
			);

			await _employeesService.UpdateEmployeeAsync(EmployeeId, payload);
			await Shell.Current.DisplayAlertAsync("Готово", "Изменения сохранены", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось сохранить сотрудника: {ex.Message}", "OK");
		}
		finally
		{
			IsSaving = false;
		}
	}

	private void MapEmployee(EmployeeCardResponse employee)
	{
		FullName = employee.FullName;
		Position = employee.Position;
		GradeText = employee.Grade.ToString(CultureInfo.InvariantCulture);
		IsActive = employee.IsActive;
		EmployeeCode = employee.EmployeeCode;
		HireDate = employee.HireDate;
	}
}
