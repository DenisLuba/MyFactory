using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Shifts;
using MyFactory.MauiClient.Pages.Production.ShiftResults;
using MyFactory.MauiClient.Services.ShiftsServices;
using MyFactory.MauiClient.ViewModels.Production.ShiftResults;

namespace MyFactory.MauiClient.ViewModels.Production.ShiftPlans;

public partial class ShiftPlanCardPageViewModel : ObservableObject
{
	private readonly IShiftPlansService _shiftPlansService;
	private readonly IShiftsService _shiftsService;

	public ShiftPlanCardPageViewModel(Guid shiftPlanId, IShiftPlansService shiftPlansService, IShiftsService shiftsService)
	{
		ShiftPlanId = shiftPlanId;
		_shiftPlansService = shiftPlansService;
		_shiftsService = shiftsService;
	}

	public Guid ShiftPlanId { get; }

	[ObservableProperty]
	private ShiftPlanCardResponse? plan;

	[ObservableProperty]
	private bool isBusy;

	public bool HasPlan => Plan is not null;
	public bool HasNoPlan => !HasPlan;

	partial void OnPlanChanged(ShiftPlanCardResponse? value)
	{
		OnPropertyChanged(nameof(HasPlan));
		OnPropertyChanged(nameof(HasNoPlan));
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
			Plan = await _shiftPlansService.GetPlanByIdAsync(ShiftPlanId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить сменное задание: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task OpenResultAsync()
	{
		if (Plan is null)
		{
			await Shell.Current.DisplayAlertAsync("Сменное задание", "Данные задания ещё не загружены.", "OK");
			return;
		}

		var resultViewModel = new ShiftResultCardPageViewModel(Plan.ShiftPlanId, _shiftsService);
		var page = new ShiftResultCardPage(resultViewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}
}
