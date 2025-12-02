using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Operations;
using MyFactory.MauiClient.Models.Specifications;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;

namespace MyFactory.MauiClient.ViewModels.Specifications;

public partial class SpecificationOperationsPageViewModel : ObservableObject
{
	private readonly ISpecificationsService _specificationsService;
	private readonly IOperationsService _operationsService;
	private IReadOnlyList<OperationListResponse>? _operationsCache;

	public SpecificationOperationsPageViewModel(
		ISpecificationsService specificationsService,
		IOperationsService operationsService)
	{
		_specificationsService = specificationsService;
		_operationsService = operationsService;

		LoadCommand = new AsyncRelayCommand(LoadAsync, CanInteract);
		AddOperationCommand = new AsyncRelayCommand(AddOperationAsync, CanInteract);
	}

	public Guid SpecificationId { get; private set; }

	public ObservableCollection<SpecificationOperationItemResponse> Operations { get; } = new();

	[ObservableProperty]
	private string specificationName = string.Empty;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasOperations;

	public string Title => string.IsNullOrWhiteSpace(SpecificationName)
		? "Операции"
		: $"{SpecificationName}: Операции";

	public bool HasNoOperations => !HasOperations;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand AddOperationCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		AddOperationCommand.NotifyCanExecuteChanged();
	}

	partial void OnSpecificationNameChanged(string value) => OnPropertyChanged(nameof(Title));

	partial void OnHasOperationsChanged(bool value) => OnPropertyChanged(nameof(HasNoOperations));

	public void Initialize(Guid specificationId, string specificationName)
	{
		SpecificationId = specificationId;
		SpecificationName = specificationName;
		LoadCommand.NotifyCanExecuteChanged();
		AddOperationCommand.NotifyCanExecuteChanged();
	}

	private bool CanInteract() => !IsBusy && SpecificationId != Guid.Empty;

	private async Task LoadAsync()
	{
		if (!CanInteract())
		{
			return;
		}

		try
		{
			IsBusy = true;
			Operations.Clear();
			var response = await _specificationsService.GetOperationsAsync(SpecificationId) ?? Array.Empty<SpecificationOperationItemResponse>();
			foreach (var operation in response)
			{
				Operations.Add(operation);
			}
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить операции: {ex.Message}");
		}
		finally
		{
			HasOperations = Operations.Count > 0;
			IsBusy = false;
		}
	}

	private async Task AddOperationAsync()
	{
		if (!CanInteract())
		{
			return;
		}

		try
		{
			IsBusy = true;
			_operationsCache = await _operationsService.GetOperationsAsync();
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить операции: {ex.Message}");
			IsBusy = false;
			return;
		}

		IsBusy = false;
		var operations = _operationsCache ?? Array.Empty<OperationListResponse>();
		if (operations.Count == 0)
		{
			await ShowAlertAsync("Внимание", "Справочник операций пуст", "Закрыть");
			return;
		}

		var operationNames = operations.Select(o => o.Name).ToArray();
		var selectedName = await Shell.Current.DisplayActionSheetAsync("Операция", "Отмена", null, operationNames);
		if (string.IsNullOrWhiteSpace(selectedName))
		{
			return;
		}

		var selected = operations.FirstOrDefault(o => o.Name == selectedName);
		if (selected is null)
		{
			await ShowAlertAsync("Ошибка", "Операция не найдена", "OK");
			return;
		}

		var minutesText = await Shell.Current.DisplayPromptAsync(
			"Длительность, мин",
			"Введите длительность",
			initialValue: selected.Minutes.ToString("0.##", CultureInfo.InvariantCulture),
			keyboard: Keyboard.Numeric);

		if (!double.TryParse(minutesText, NumberStyles.Float, CultureInfo.CurrentCulture, out var minutes) &&
			!double.TryParse(minutesText, NumberStyles.Float, CultureInfo.InvariantCulture, out minutes))
		{
			await ShowAlertAsync("Ошибка", "Длительность должна быть числом");
			return;
		}

		if (minutes <= 0)
		{
			await ShowAlertAsync("Ошибка", "Длительность должна быть больше нуля");
			return;
		}

		var costText = await Shell.Current.DisplayPromptAsync(
			"Стоимость",
			"Введите стоимость",
			initialValue: selected.Cost.ToString("0.##", CultureInfo.InvariantCulture),
			keyboard: Keyboard.Numeric);

		if (!decimal.TryParse(costText, NumberStyles.Float, CultureInfo.CurrentCulture, out var cost) &&
			!decimal.TryParse(costText, NumberStyles.Float, CultureInfo.InvariantCulture, out cost))
		{
			await ShowAlertAsync("Ошибка", "Стоимость должна быть числом");
			return;
		}

		try
		{
			IsBusy = true;
			var request = new SpecificationsAddOperationRequest(selected.Id, minutes, cost);
			var response = await _specificationsService.AddOperationAsync(SpecificationId, request);
			if (response?.Item is not null)
			{
				Operations.Add(response.Item);
				HasOperations = Operations.Count > 0;
			}
			else
			{
				await ShowAlertAsync("Ошибка", "Не удалось добавить операцию");
			}
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось сохранить операцию: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private static Task ShowAlertAsync(string title, string message, string accept = "OK")
		=> Shell.Current.DisplayAlertAsync(title, message, accept);
}
