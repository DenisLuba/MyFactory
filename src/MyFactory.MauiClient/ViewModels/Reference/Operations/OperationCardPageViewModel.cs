using System;
using System.Globalization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Operations;
using MyFactory.MauiClient.Services.OperationsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Operations;

public partial class OperationCardPageViewModel : ObservableObject
{
	private readonly IOperationsService _operationsService;

	public OperationCardPageViewModel(IOperationsService operationsService)
	{
		_operationsService = operationsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		StartEditingCommand = new RelayCommand(StartEditing, () => !IsBusy && !IsEditing);
		SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
	}

	public Guid OperationId { get; private set; }

	[ObservableProperty]
	private string code = string.Empty;

	[ObservableProperty]
	private string name = string.Empty;

	[ObservableProperty]
	private string operationType = string.Empty;

	[ObservableProperty]
	private string minutesText = string.Empty;

	[ObservableProperty]
	private string costText = string.Empty;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isEditing;

	public string Title => string.IsNullOrWhiteSpace(Name) ? "Операция" : Name;

	public IAsyncRelayCommand LoadCommand { get; }
	public IRelayCommand StartEditingCommand { get; }
	public IAsyncRelayCommand SaveCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		SaveCommand.NotifyCanExecuteChanged();
		StartEditingCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsEditingChanged(bool value)
	{
		SaveCommand.NotifyCanExecuteChanged();
		StartEditingCommand.NotifyCanExecuteChanged();
	}

	partial void OnNameChanged(string value) => OnPropertyChanged(nameof(Title));

	public void Initialize(Guid operationId)
	{
		OperationId = operationId;
		LoadCommand.NotifyCanExecuteChanged();
	}

	private bool CanLoad() => !IsBusy && OperationId != Guid.Empty;

	private bool CanSave() => !IsBusy && IsEditing;

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

			var operation = await _operationsService.GetOperationAsync(OperationId);
			if (operation is null)
			{
				await ShowAlertAsync("Ошибка", "Карточка операции не найдена");
				return;
			}

			ApplyOperation(operation);
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить операцию: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplyOperation(OperationCardResponse operation)
	{
		Code = operation.Code;
		Name = operation.Name;
		OperationType = operation.OperationType;
		MinutesText = operation.Minutes.ToString("0.##", CultureInfo.InvariantCulture);
		CostText = operation.Cost.ToString("0.00", CultureInfo.InvariantCulture);
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
		if (!IsEditing || IsBusy)
		{
			return;
		}

		if (OperationId == Guid.Empty)
		{
			await ShowAlertAsync("Ошибка", "Операция не выбрана");
			return;
		}

		if (string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(OperationType))
		{
			await ShowAlertAsync("Ошибка", "Код, наименование и тип операции обязательны");
			return;
		}

		if (!double.TryParse(MinutesText, NumberStyles.Float, CultureInfo.CurrentCulture, out var minutes) &&
			!double.TryParse(MinutesText, NumberStyles.Float, CultureInfo.InvariantCulture, out minutes))
		{
			await ShowAlertAsync("Ошибка", "Время операции должно быть числом");
			return;
		}

		if (minutes <= 0)
		{
			await ShowAlertAsync("Ошибка", "Время должно быть положительным");
			return;
		}

		if (!decimal.TryParse(CostText, NumberStyles.Float, CultureInfo.CurrentCulture, out var cost) &&
			!decimal.TryParse(CostText, NumberStyles.Float, CultureInfo.InvariantCulture, out cost))
		{
			await ShowAlertAsync("Ошибка", "Стоимость должна быть числом");
			return;
		}

		if (cost < 0)
		{
			await ShowAlertAsync("Ошибка", "Стоимость не может быть отрицательной");
			return;
		}

		try
		{
			IsBusy = true;
			var request = new OperationUpdateRequest(
				Code.Trim(),
				Name.Trim(),
				OperationType.Trim(),
				minutes,
				cost);

			var response = await _operationsService.UpdateOperationAsync(OperationId, request);
			var status = response?.Status ?? "Обновлено";
			await ShowAlertAsync("Готово", status);
			IsEditing = false;
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось сохранить операцию: {ex.Message}");
			return;
		}
		finally
		{
			IsBusy = false;
		}

		await LoadAsync();
	}

	private static Task ShowAlertAsync(string title, string message)
		=> Shell.Current.DisplayAlertAsync(title, message, "OK");
}
