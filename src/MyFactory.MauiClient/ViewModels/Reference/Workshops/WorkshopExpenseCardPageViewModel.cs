using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.WorkshopExpenses;
using MyFactory.MauiClient.Models.Workshops;
using MyFactory.MauiClient.Services.WorkshopExpensesServices;
using MyFactory.MauiClient.Services.WorkshopsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Workshops;

public partial class WorkshopExpenseCardPageViewModel : ObservableObject
{
	private readonly IWorkshopExpensesService _workshopExpensesService;
	private readonly IWorkshopsService _workshopsService;

	public WorkshopExpenseCardPageViewModel(IWorkshopExpensesService workshopExpensesService, IWorkshopsService workshopsService)
	{
		_workshopExpensesService = workshopExpensesService;
		_workshopsService = workshopsService;

		Workshops = new ObservableCollection<WorkshopsListResponse>();
		LoadExpenseCommand = new AsyncRelayCommand(LoadAsync);
		SaveExpenseCommand = new AsyncRelayCommand(SaveAsync);
	}

	public Guid ExpenseId { get; private set; }
	public Guid? WorkshopIdFilter { get; private set; }

	public ObservableCollection<WorkshopsListResponse> Workshops { get; }

	[ObservableProperty]
	private WorkshopsListResponse? selectedWorkshop;

	[ObservableProperty]
	private decimal amountPerUnit;

	[ObservableProperty]
	private DateTime effectiveFrom = DateTime.Today;

	[ObservableProperty]
	private DateTime effectiveTo = DateTime.Today;

	[ObservableProperty]
	private bool hasEffectiveTo;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isSaving;

	public IAsyncRelayCommand LoadExpenseCommand { get; }
	public IAsyncRelayCommand SaveExpenseCommand { get; }

	public void Initialize(Guid? expenseId, Guid? workshopId)
	{
		ExpenseId = expenseId.GetValueOrDefault();
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
			await LoadWorkshopsAsync();

			if (ExpenseId == Guid.Empty)
			{
				EffectiveFrom = DateTime.Today;
				EffectiveTo = DateTime.Today;
				HasEffectiveTo = false;
				if (WorkshopIdFilter.HasValue)
				{
					SelectedWorkshop = Workshops.FirstOrDefault(x => x.Id == WorkshopIdFilter.Value);
				}
				return;
			}

			var expense = await _workshopExpensesService.GetAsync(ExpenseId);
			if (expense is null)
			{
				await Shell.Current.DisplayAlertAsync("Ошибка", "Расход не найден", "OK");
				await Shell.Current.GoToAsync("..");
				return;
			}

			AmountPerUnit = expense.AmountPerUnit;
			EffectiveFrom = expense.EffectiveFrom;
			EffectiveTo = expense.EffectiveTo ?? expense.EffectiveFrom;
			HasEffectiveTo = expense.EffectiveTo.HasValue;
			SelectedWorkshop = Workshops.FirstOrDefault(x => x.Id == expense.WorkshopId);
			WorkshopIdFilter = expense.WorkshopId;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить карточку: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task SaveAsync()
	{
		if (IsSaving)
		{
			return;
		}

		if (SelectedWorkshop is null)
		{
			await Shell.Current.DisplayAlertAsync("Внимание", "Выберите цех", "OK");
			return;
		}

		try
		{
			IsSaving = true;
			var request = new WorkshopExpenseUpdateRequest(
				SelectedWorkshop.Id,
				AmountPerUnit,
				EffectiveFrom,
				HasEffectiveTo ? EffectiveTo : null
			);

			if (ExpenseId == Guid.Empty)
			{
				var response = await _workshopExpensesService.CreateAsync(new WorkshopExpenseCreateRequest(
					request.WorkshopId,
					request.AmountPerUnit,
					request.EffectiveFrom,
					request.EffectiveTo
				));
				if (response is not null)
				{
					ExpenseId = response.Id;
				}
			}
			else
			{
				await _workshopExpensesService.UpdateAsync(ExpenseId, request);
			}

			await Shell.Current.DisplayAlertAsync("Готово", "Расход сохранён", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось сохранить расход: {ex.Message}", "OK");
		}
		finally
		{
			IsSaving = false;
		}
	}

	private async Task LoadWorkshopsAsync()
	{
		if (Workshops.Count > 0)
		{
			return;
		}

		var workshops = await _workshopsService.ListAsync();
		if (workshops is null)
		{
			return;
		}

		Workshops.Clear();
		foreach (var workshop in workshops)
		{
			Workshops.Add(workshop);
		}
	}
}
