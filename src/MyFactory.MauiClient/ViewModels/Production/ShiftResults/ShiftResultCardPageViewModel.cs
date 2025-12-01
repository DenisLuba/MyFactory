using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Shifts;
using MyFactory.MauiClient.Services.ShiftsServices;

namespace MyFactory.MauiClient.ViewModels.Production.ShiftResults;

public partial class ShiftResultCardPageViewModel : ObservableObject
{
	private readonly IShiftsService _shiftsService;

	public ShiftResultCardPageViewModel(Guid shiftPlanId, IShiftsService shiftsService)
	{
		ShiftPlanId = shiftPlanId;
		_shiftsService = shiftsService;
	}

	public Guid ShiftPlanId { get; }

	[ObservableProperty]
	private ShiftResultCardResponse? result;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private int actualQty;

	[ObservableProperty]
	private double hoursWorked;

	[ObservableProperty]
	private bool bonus;

	public bool HasResult => Result is not null;
	public bool HasNoResult => !HasResult;

	partial void OnResultChanged(ShiftResultCardResponse? value)
	{
		OnPropertyChanged(nameof(HasResult));
		OnPropertyChanged(nameof(HasNoResult));

		if (value is not null)
		{
			ActualQty = value.ActualQty;
			HoursWorked = value.HoursWorked;
			Bonus = value.Bonus;
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
			Result = await _shiftsService.GetResultByIdAsync(ShiftPlanId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить результат сдачи: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var request = new ShiftsRecordResultRequest(ShiftPlanId, ActualQty, HoursWorked, Bonus);
			await _shiftsService.SaveResultAsync(request);
			await LoadAsync();
			await Shell.Current.DisplayAlertAsync("Сохранено", "Результат сдачи обновлён.", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось сохранить результат: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}
}
