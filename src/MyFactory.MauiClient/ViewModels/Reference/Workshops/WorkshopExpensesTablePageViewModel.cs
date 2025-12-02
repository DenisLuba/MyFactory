using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.WorkshopExpenses;
using MyFactory.MauiClient.Pages.Reference.Workshops;
using MyFactory.MauiClient.Services.WorkshopExpensesServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Workshops;

public partial class WorkshopExpensesTablePageViewModel : ObservableObject
{
	private readonly IWorkshopExpensesService _workshopExpensesService;

	public WorkshopExpensesTablePageViewModel(IWorkshopExpensesService workshopExpensesService)
	{
		_workshopExpensesService = workshopExpensesService;
		Expenses = new ObservableCollection<WorkshopExpenseListResponse>();
		LoadExpensesCommand = new AsyncRelayCommand(LoadAsync);
		RefreshCommand = new AsyncRelayCommand(LoadAsync);
		OpenCardCommand = new AsyncRelayCommand<WorkshopExpenseListResponse?>(OpenCardAsync);
		CreateNewCommand = new AsyncRelayCommand(CreateNewAsync);
	}

	public Guid? WorkshopIdFilter { get; private set; }

	public ObservableCollection<WorkshopExpenseListResponse> Expenses { get; }

	[ObservableProperty]
	private WorkshopExpenseListResponse? selectedExpense;

	[ObservableProperty]
	private bool isBusy;

	public IAsyncRelayCommand LoadExpensesCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<WorkshopExpenseListResponse?> OpenCardCommand { get; }
	public IAsyncRelayCommand CreateNewCommand { get; }

	public void Initialize(Guid? workshopId)
	{
		WorkshopIdFilter = workshopId;
	}

	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Expenses.Clear();
			var response = await _workshopExpensesService.ListAsync(WorkshopIdFilter);
			if (response is null)
			{
				return;
			}

			foreach (var expense in response)
			{
				Expenses.Add(expense);
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить расходы: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task OpenCardAsync(WorkshopExpenseListResponse? expense)
	{
		SelectedExpense = null;
		if (expense is null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(WorkshopExpenseCardPage), true, new Dictionary<string, object>
		{
			{ "ExpenseId", expense.Id },
			{ "WorkshopId", expense.WorkshopId }
		});
	}

	private Task CreateNewAsync()
	{
		var parameters = new Dictionary<string, object>();
		if (WorkshopIdFilter.HasValue)
		{
			parameters["WorkshopId"] = WorkshopIdFilter.Value;
		}

		return parameters.Count > 0
			? Shell.Current.GoToAsync(nameof(WorkshopExpenseCardPage), true, parameters)
			: Shell.Current.GoToAsync(nameof(WorkshopExpenseCardPage), true);
	}
}
