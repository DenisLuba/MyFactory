using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Operations;
using MyFactory.MauiClient.Pages.Reference.Operations;
using MyFactory.MauiClient.Services.OperationsServices;
using MyFactory.MauiClient.UIModels.Reference;

namespace MyFactory.MauiClient.ViewModels.Reference.Operations;

public partial class OperationsTablePageViewModel : ObservableObject
{
	private readonly IOperationsService _operationsService;
	private const string AllOperationTypesOption = "Все типы";
	private readonly ObservableCollection<OperationItem> _allOperations = new();

	public OperationsTablePageViewModel(IOperationsService operationsService)
	{
		_operationsService = operationsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		OpenCardCommand = new AsyncRelayCommand<OperationItem?>(OpenCardAsync);
		OperationTypes.Add(AllOperationTypesOption);
		SelectedOperationType = AllOperationTypesOption;
	}

	public ObservableCollection<OperationItem> Operations { get; } = new();
	public ObservableCollection<string> OperationTypes { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasOperations;

	[ObservableProperty]
	private string? selectedOperationType;

	public bool HasNoOperations => !HasOperations;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<OperationItem?> OpenCardCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
	}

	partial void OnHasOperationsChanged(bool value) => OnPropertyChanged(nameof(HasNoOperations));

	partial void OnSelectedOperationTypeChanged(string? value) => ApplyFilters();

	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Operations.Clear();
			_allOperations.Clear();
			OperationTypes.Clear();
			OperationTypes.Add(AllOperationTypesOption);

			var response = await _operationsService.GetOperationsAsync() ?? Array.Empty<OperationListResponse>();
			foreach (var operation in response.OrderBy(o => o.Code))
			{
				var item = new OperationItem(
					operation.Id,
					operation.Code,
					operation.Name,
					operation.OperationType,
					operation.Minutes,
					operation.Cost);
				_allOperations.Add(item);
			}

			PopulateOperationTypes();
			RestoreSelectionOrDefault();
			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить операции: {ex.Message}", "OK");
		}
		finally
		{
			HasOperations = Operations.Count > 0;
			IsBusy = false;
		}
	}

	private void PopulateOperationTypes()
	{
		var distinctTypes = _allOperations
			.Select(o => o.OperationType)
			.Where(type => !string.IsNullOrWhiteSpace(type))
			.Distinct()
			.OrderBy(type => type);

		foreach (var type in distinctTypes)
		{
			OperationTypes.Add(type);
		}
	}

	private void RestoreSelectionOrDefault()
	{
		var previousSelection = SelectedOperationType;
		if (!string.IsNullOrWhiteSpace(previousSelection) && OperationTypes.Any(type => string.Equals(type, previousSelection, StringComparison.OrdinalIgnoreCase)))
		{
			SelectedOperationType = previousSelection;
			return;
		}

		SelectedOperationType = AllOperationTypesOption;
	}

	private void ApplyFilters()
	{
		Operations.Clear();

		IEnumerable<OperationItem> filtered = SelectedOperationType switch
		{
			null => _allOperations,
			var type when string.Equals(type, AllOperationTypesOption, StringComparison.OrdinalIgnoreCase) => _allOperations,
			var type => _allOperations.Where(o => string.Equals(o.OperationType, type, StringComparison.OrdinalIgnoreCase))
		};

		foreach (var operation in filtered)
		{
			Operations.Add(operation);
		}

		HasOperations = Operations.Count > 0;
	}

	private async Task OpenCardAsync(OperationItem? operation)
	{
		if (operation is null)
		{
			return;
		}

		var viewModel = new OperationCardPageViewModel(_operationsService);
		viewModel.Initialize(operation.Id);
		var page = new OperationCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}
}
