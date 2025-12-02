using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Specifications;
using MyFactory.MauiClient.Pages.Specifications;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;

namespace MyFactory.MauiClient.ViewModels.Specifications;

public partial class SpecificationCardPageViewModel : ObservableObject
{
	private readonly ISpecificationsService _specificationsService;
	private readonly IMaterialsService _materialsService;
	private readonly IOperationsService _operationsService;

	public SpecificationCardPageViewModel(
		ISpecificationsService specificationsService,
		IMaterialsService materialsService,
		IOperationsService operationsService)
	{
		_specificationsService = specificationsService;
		_materialsService = materialsService;
		_operationsService = operationsService;

		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		StartEditingCommand = new RelayCommand(StartEditing, () => !IsBusy && !IsEditing);
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
		OpenBomCommand = new AsyncRelayCommand(OpenBomAsync, CanOpenDetails);
		OpenOperationsCommand = new AsyncRelayCommand(OpenOperationsAsync, CanOpenDetails);
		OpenCostCommand = new AsyncRelayCommand(OpenCostAsync, CanOpenDetails);
	}

	public Guid SpecificationId { get; private set; }

	[ObservableProperty]
	private string sku = string.Empty;

	[ObservableProperty]
	private string name = string.Empty;

	[ObservableProperty]
	private string planPerHourText = string.Empty;

	[ObservableProperty]
	private string description = string.Empty;

	[ObservableProperty]
	private string status = string.Empty;

	[ObservableProperty]
	private int imagesCount;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isEditing;

	public string Title => string.IsNullOrWhiteSpace(Name)
		? "Спецификация"
		: Name;

	public bool IsNew => SpecificationId == Guid.Empty;

	public IAsyncRelayCommand LoadCommand { get; }
	public IRelayCommand StartEditingCommand { get; }
	public IAsyncRelayCommand SaveCommand { get; }
	public IAsyncRelayCommand OpenBomCommand { get; }
	public IAsyncRelayCommand OpenOperationsCommand { get; }
	public IAsyncRelayCommand OpenCostCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		StartEditingCommand.NotifyCanExecuteChanged();
		SaveCommand.NotifyCanExecuteChanged();
		OpenBomCommand.NotifyCanExecuteChanged();
		OpenOperationsCommand.NotifyCanExecuteChanged();
		OpenCostCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsEditingChanged(bool value)
	{
		StartEditingCommand.NotifyCanExecuteChanged();
		SaveCommand.NotifyCanExecuteChanged();
	}

	partial void OnNameChanged(string value) => OnPropertyChanged(nameof(Title));

	public void Initialize(Guid specificationId)
	{
		SpecificationId = specificationId;
		LoadCommand.NotifyCanExecuteChanged();
		OpenBomCommand.NotifyCanExecuteChanged();
		OpenOperationsCommand.NotifyCanExecuteChanged();
		OpenCostCommand.NotifyCanExecuteChanged();
	}

	public void InitializeForCreate()
	{
		SpecificationId = Guid.Empty;
		IsEditing = true;
		Status = "Создана";
	}

	private bool CanLoad() => !IsBusy && SpecificationId != Guid.Empty;

	private bool CanSave() => !IsBusy && IsEditing;

	private bool CanOpenDetails() => !IsBusy && SpecificationId != Guid.Empty;

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			IsEditing = false;

			var specification = await _specificationsService.GetAsync(SpecificationId);
			if (specification is null)
			{
				await ShowAlertAsync("Ошибка", "Спецификация не найдена");
				return;
			}

			ApplySpecification(specification);
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить спецификацию: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplySpecification(SpecificationsGetResponse specification)
	{
		Sku = specification.Sku;
		Name = specification.Name;
		PlanPerHourText = specification.PlanPerHour.ToString("0.##", CultureInfo.InvariantCulture);
		Description = specification.Description ?? string.Empty;
		ImagesCount = specification.ImagesCount;
		Status = specification.Status.ToString();
	}

	private void StartEditing()
	{
		if (IsBusy)
		{
			return;
		}

		IsEditing = true;
	}

	private async Task SaveAsync()
	{
		if (!CanSave())
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(Sku) || string.IsNullOrWhiteSpace(Name))
		{
			await ShowAlertAsync("Ошибка", "SKU и наименование обязательны");
			return;
		}

		if (!double.TryParse(PlanPerHourText, NumberStyles.Float, CultureInfo.CurrentCulture, out var planPerHour) &&
			!double.TryParse(PlanPerHourText, NumberStyles.Float, CultureInfo.InvariantCulture, out planPerHour))
		{
			await ShowAlertAsync("Ошибка", "План/час должен быть числом");
			return;
		}

		if (planPerHour <= 0)
		{
			await ShowAlertAsync("Ошибка", "План/час должен быть больше нуля");
			return;
		}

		var shouldReload = false;
		try
		{
			IsBusy = true;
			if (SpecificationId == Guid.Empty)
			{
				var createRequest = new SpecificationsCreateRequest(
					Sku.Trim(),
					Name.Trim(),
					planPerHour,
					Description?.Trim());

				var createResponse = await _specificationsService.CreateAsync(createRequest);
				if (createResponse is null || createResponse.Id == Guid.Empty)
				{
					await ShowAlertAsync("Ошибка", "Не удалось создать спецификацию");
					return;
				}

				SpecificationId = createResponse.Id;
				await ShowAlertAsync("Готово", createResponse.Status.ToString());
				shouldReload = true;
			}
			else
			{
				var updateRequest = new SpecificationsUpdateRequest(
					Sku.Trim(),
					Name.Trim(),
					planPerHour,
					Description?.Trim());

				var updateResponse = await _specificationsService.UpdateAsync(SpecificationId, updateRequest);
				await ShowAlertAsync("Готово", updateResponse?.Status.ToString() ?? "Обновлено");
				IsEditing = false;
				shouldReload = true;
			}
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось сохранить спецификацию: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}

		if (shouldReload && SpecificationId != Guid.Empty)
		{
			await LoadAsync();
		}
	}

	private async Task OpenBomAsync()
	{
		if (!CanOpenDetails())
		{
			return;
		}

		var viewModel = new SpecificationBomPageViewModel(_specificationsService, _materialsService);
		viewModel.Initialize(SpecificationId, Name);
		var page = new SpecificationBomPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private async Task OpenOperationsAsync()
	{
		if (!CanOpenDetails())
		{
			return;
		}

		var viewModel = new SpecificationOperationsPageViewModel(_specificationsService, _operationsService);
		viewModel.Initialize(SpecificationId, Name);
		var page = new SpecificationOperationsPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private async Task OpenCostAsync()
	{
		if (!CanOpenDetails())
		{
			return;
		}

		var costViewModel = new SpecificationCostCardPageViewModel(_specificationsService);
		costViewModel.Initialize(SpecificationId, Name);
		var page = new SpecificationCostCardPage(costViewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private static Task ShowAlertAsync(string title, string message)
		=> Shell.Current.DisplayAlertAsync(title, message, "OK");
}
