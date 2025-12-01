using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Shifts;
using MyFactory.MauiClient.Pages.Production.ShiftPlans;
using MyFactory.MauiClient.Services.ShiftsServices;

namespace MyFactory.MauiClient.ViewModels.Production.ShiftPlans;

public partial class ShiftPlansTablePageViewModel : ObservableObject
{
	private readonly IShiftPlansService _shiftPlansService;
	private readonly IShiftsService _shiftsService;

	public ShiftPlansTablePageViewModel(IShiftPlansService shiftPlansService, IShiftsService shiftsService)
	{
		_shiftPlansService = shiftPlansService;
		_shiftsService = shiftsService;
		selectedDate = DateTime.Today;
	}

	public ObservableCollection<ShiftPlanListResponse> Plans { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasPlans;

	public bool HasNoPlans => !HasPlans;

	[ObservableProperty]
	private DateTime selectedDate;

	partial void OnHasPlansChanged(bool value) => OnPropertyChanged(nameof(HasNoPlans));

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
			Plans.Clear();

			var response = await _shiftPlansService.GetPlansAsync(SelectedDate);

			if (response is { Count: > 0 })
			{
				foreach (var plan in response
					.OrderBy(p => p.Date)
					.ThenBy(p => p.EmployeeName))
				{
					Plans.Add(plan);
				}
			}

			HasPlans = Plans.Count > 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить сменные задания: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task OpenCardAsync(ShiftPlanListResponse? plan)
	{
		if (plan is null)
		{
			return;
		}

		var viewModel = new ShiftPlanCardPageViewModel(plan.ShiftPlanId, _shiftPlansService, _shiftsService);
		var page = new ShiftPlanCardPage(viewModel);

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
