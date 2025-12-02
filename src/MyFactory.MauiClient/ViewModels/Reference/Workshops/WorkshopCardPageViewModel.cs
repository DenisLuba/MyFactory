using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Workshops;
using MyFactory.MauiClient.Pages.Reference.Workshops;
using MyFactory.MauiClient.Services.WorkshopsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Workshops;

public partial class WorkshopCardPageViewModel : ObservableObject
{
	private readonly IWorkshopsService _workshopsService;

	public WorkshopCardPageViewModel(IWorkshopsService workshopsService)
	{
		_workshopsService = workshopsService;

		WorkshopTypes = Enum.GetValues(typeof(WorkshopType)).Cast<WorkshopType>().ToArray();
		Statuses = Enum.GetValues(typeof(WorkshopStatus)).Cast<WorkshopStatus>().ToArray();

		LoadWorkshopCommand = new AsyncRelayCommand(LoadAsync);
		SaveWorkshopCommand = new AsyncRelayCommand(SaveAsync);
		OpenExpensesCommand = new AsyncRelayCommand(OpenExpensesAsync);
	}

	public Guid WorkshopId { get; private set; }

	public IReadOnlyList<WorkshopType> WorkshopTypes { get; }
	public IReadOnlyList<WorkshopStatus> Statuses { get; }

	[ObservableProperty]
	private string name = string.Empty;

	[ObservableProperty]
	private WorkshopType selectedType = WorkshopType.Cutting;

	[ObservableProperty]
	private WorkshopStatus selectedStatus = WorkshopStatus.Active;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isSaving;

	public IAsyncRelayCommand LoadWorkshopCommand { get; }
	public IAsyncRelayCommand SaveWorkshopCommand { get; }
	public IAsyncRelayCommand OpenExpensesCommand { get; }

	public void Initialize(Guid? workshopId)
	{
		WorkshopId = workshopId.GetValueOrDefault();
	}

	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		if (WorkshopId == Guid.Empty)
		{
			Name = string.Empty;
			SelectedType = WorkshopType.Cutting;
			SelectedStatus = WorkshopStatus.Active;
			return;
		}

		try
		{
			IsBusy = true;
			var workshop = await _workshopsService.GetAsync(WorkshopId);

			if (workshop is null)
			{
				await Shell.Current.DisplayAlertAsync("Ошибка", "Цех не найден", "OK");
				await Shell.Current.GoToAsync("..");
				return;
			}

			Name = workshop.Name;
			SelectedType = workshop.Type;
			SelectedStatus = workshop.Status;
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

		if (string.IsNullOrWhiteSpace(Name))
		{
			await Shell.Current.DisplayAlertAsync("Внимание", "Введите название цеха", "OK");
			return;
		}

		try
		{
			IsSaving = true;

			if (WorkshopId == Guid.Empty)
			{
				var createResponse = await _workshopsService.CreateAsync(new WorkshopCreateRequest(Name.Trim(), SelectedType, SelectedStatus));
				if (createResponse is not null)
				{
					WorkshopId = createResponse.Id;
				}
			}
			else
			{
				await _workshopsService.UpdateAsync(WorkshopId, new WorkshopUpdateRequest(Name.Trim(), SelectedType, SelectedStatus));
			}

			await Shell.Current.DisplayAlertAsync("Готово", "Изменения сохранены", "OK");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось сохранить цех: {ex.Message}", "OK");
		}
		finally
		{
			IsSaving = false;
		}
	}

	private Task OpenExpensesAsync()
	{
		var parameters = new Dictionary<string, object>();
		if (WorkshopId != Guid.Empty)
		{
			parameters["WorkshopId"] = WorkshopId;
		}

		return parameters.Count > 0
			? Shell.Current.GoToAsync(nameof(WorkshopExpensesTablePage), true, parameters)
			: Shell.Current.GoToAsync(nameof(WorkshopExpensesTablePage), true);
	}
}
