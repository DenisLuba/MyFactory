using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Shifts;
using MyFactory.MauiClient.Pages.Production.ShiftResults;
using MyFactory.MauiClient.Services.ShiftsServices;

namespace MyFactory.MauiClient.ViewModels.Production.ShiftResults;

public partial class ShiftResultsTablePageViewModel : ObservableObject
{
	private readonly IShiftsService _shiftsService;

	public ShiftResultsTablePageViewModel(IShiftsService shiftsService)
	{
		_shiftsService = shiftsService;
		selectedDate = DateTime.Today;
	}

	public ObservableCollection<ShiftResultListResponse> Results { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasResults;

	public bool HasNoResults => !HasResults;

	[ObservableProperty]
	private string? employeeId;

	[ObservableProperty]
	private DateTime selectedDate;

	partial void OnHasResultsChanged(bool value) => OnPropertyChanged(nameof(HasNoResults));

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
			Results.Clear();

			Guid? employeeGuid = null;
			if (!string.IsNullOrWhiteSpace(EmployeeId) && Guid.TryParse(EmployeeId, out var parsed))
			{
				employeeGuid = parsed;
			}

			var data = await _shiftsService.GetResultsAsync(employeeGuid, SelectedDate);

			if (data is { Count: > 0 })
			{
				foreach (var result in data)
				{
					Results.Add(result);
				}
			}

			HasResults = Results.Count > 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить фактическую сдачу: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task OpenCardAsync(ShiftResultListResponse? item)
	{
		if (item is null)
		{
			return;
		}

		var viewModel = new ShiftResultCardPageViewModel(item.ShiftPlanId, _shiftsService);
		var page = new ShiftResultCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	partial void OnSelectedDateChanged(DateTime value)
	{
		if (!IsBusy && LoadCommand.CanExecute(null))
		{
			_ = LoadCommand.ExecuteAsync(null);
		}
	}
}
