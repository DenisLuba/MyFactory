using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Specifications;
using MyFactory.MauiClient.Services.SpecificationsServices;
using MyFactory.MauiClient.UIModels.Specifications;

namespace MyFactory.MauiClient.ViewModels.Specifications;

public partial class SpecificationCostCardPageViewModel : ObservableObject
{
	private readonly ISpecificationsService _specificationsService;
	private Guid? _preselectedSpecificationId;
	private string? _preselectedSpecificationName;

	public SpecificationCostCardPageViewModel(ISpecificationsService specificationsService)
	{
		_specificationsService = specificationsService;
		LoadSpecificationsCommand = new AsyncRelayCommand(LoadSpecificationsAsync, () => !IsBusy);
		RefreshCostCommand = new AsyncRelayCommand(LoadCostAsync, CanRefreshCost);
	}

	public ObservableCollection<SpecificationsListItem> Specifications { get; } = new();

	[ObservableProperty]
	private SpecificationsListItem? selectedSpecification;

	[ObservableProperty]
	private DateTime asOfDate = DateTime.Today;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private decimal materialsCost;

	[ObservableProperty]
	private decimal operationsCost;

	[ObservableProperty]
	private decimal workshopExpenses;

	[ObservableProperty]
	private decimal totalCost;

	[ObservableProperty]
	private bool hasCost;

	public string Title => SelectedSpecification?.Name is { Length: > 0 } name
		? $"{name}: Себестоимость"
		: string.IsNullOrWhiteSpace(_preselectedSpecificationName)
			? "Себестоимость спецификации"
			: $"{_preselectedSpecificationName}: Себестоимость";

	public IAsyncRelayCommand LoadSpecificationsCommand { get; }
	public IAsyncRelayCommand RefreshCostCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadSpecificationsCommand.NotifyCanExecuteChanged();
		RefreshCostCommand.NotifyCanExecuteChanged();
	}

	partial void OnSelectedSpecificationChanged(SpecificationsListItem? value)
	{
		OnPropertyChanged(nameof(Title));
		_preselectedSpecificationId = value?.Id;
		if (value?.Name is { Length: > 0 })
		{
			_preselectedSpecificationName = value.Name;
		}
		if (value is null)
		{
			HasCost = false;
		}
		else if (RefreshCostCommand.CanExecute(null))
		{
			_ = RefreshCostCommand.ExecuteAsync(null);
		}
	}

	public void Initialize(Guid specificationId, string? specificationName)
	{
		_preselectedSpecificationId = specificationId;
		_preselectedSpecificationName = specificationName;
		OnPropertyChanged(nameof(Title));
	}

	private bool CanRefreshCost() => !IsBusy && SelectedSpecification is not null;

	private async Task LoadSpecificationsAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Specifications.Clear();
			var response = await _specificationsService.ListAsync() ?? Array.Empty<SpecificationsListResponse>();
			foreach (var spec in response.OrderBy(s => s.Sku))
			{
				Specifications.Add(new SpecificationsListItem(
					spec.Id,
					spec.Sku,
					spec.Name,
					spec.PlanPerHour,
					spec.Status.ToString(),
					spec.ImagesCount));
			}

			if (_preselectedSpecificationId.HasValue)
			{
				var match = Specifications.FirstOrDefault(s => s.Id == _preselectedSpecificationId.Value);
				if (match is not null)
				{
					SelectedSpecification = match;
				}
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить спецификации: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task LoadCostAsync()
	{
		if (!CanRefreshCost())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var cost = await _specificationsService.CostAsync(SelectedSpecification!.Id, AsOfDate);
			if (cost is null)
			{
				HasCost = false;
				await Shell.Current.DisplayAlertAsync("Внимание", "Данные по себестоимости отсутствуют", "OK");
				return;
			}

			MaterialsCost = cost.MaterialsCost;
			OperationsCost = cost.OperationsCost;
			WorkshopExpenses = cost.WorkshopExpenses;
			TotalCost = cost.TotalCost;
			HasCost = true;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось рассчитать себестоимость: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}
}
