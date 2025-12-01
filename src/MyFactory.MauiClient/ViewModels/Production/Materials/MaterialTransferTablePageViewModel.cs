using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.Pages.Production.Materials;
using MyFactory.MauiClient.Services.ProductionServices;

namespace MyFactory.MauiClient.ViewModels.Production.Materials;

public partial class MaterialTransferTablePageViewModel : ObservableObject
{
	private readonly IMaterialTransfersService _materialTransfersService;
	private readonly List<MaterialTransferListResponse> _allTransfers = new();

	public MaterialTransferTablePageViewModel(IMaterialTransfersService materialTransfersService)
	{
		_materialTransfersService = materialTransfersService;
		DateFilter = DateTime.Today;
	}

	public ObservableCollection<MaterialTransferListResponse> Transfers { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasTransfers;

	public bool HasNoTransfers => !HasTransfers;

	[ObservableProperty]
	private string? warehouseFilter;

	[ObservableProperty]
	private string? productionOrderFilter;

	[ObservableProperty]
	private DateTime dateFilter;

	[ObservableProperty]
	private bool isDateFilterActive;

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
			Transfers.Clear();
			_allTransfers.Clear();

			var response = await _materialTransfersService.GetTransfersAsync();

			if (response is { Count: > 0 })
			{
				foreach (var transfer in response.OrderByDescending(t => t.Date))
				{
					_allTransfers.Add(transfer);
				}
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить передачи материалов: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private void ClearFilters()
	{
		WarehouseFilter = null;
		ProductionOrderFilter = null;
		IsDateFilterActive = false;
		DateFilter = DateTime.Today;
		ApplyFilters();
	}

	[RelayCommand]
	private async Task OpenCardAsync(MaterialTransferListResponse? transfer)
	{
		if (transfer is null)
		{
			return;
		}

		var cardViewModel = new MaterialTransferCardPageViewModel(transfer.TransferId, _materialTransfersService);
		var page = new MaterialTransferCardPage(cardViewModel);

		await Shell.Current.Navigation.PushAsync(page);
	}

	[RelayCommand]
	private async Task CreateAsync()
	{
		await Shell.Current.DisplayAlert("Передачи", "Создание документа будет добавлено позже.", "OK");
	}

	private void ApplyFilters()
	{
		Transfers.Clear();

		IEnumerable<MaterialTransferListResponse> query = _allTransfers;

		if (!string.IsNullOrWhiteSpace(WarehouseFilter))
		{
			var term = WarehouseFilter.Trim();
			query = query.Where(x => x.Warehouse.Contains(term, StringComparison.OrdinalIgnoreCase));
		}

		if (!string.IsNullOrWhiteSpace(ProductionOrderFilter))
		{
			var term = ProductionOrderFilter.Trim();
			query = query.Where(x => x.ProductionOrder.Contains(term, StringComparison.OrdinalIgnoreCase));
		}

		if (IsDateFilterActive)
		{
			query = query.Where(x => x.Date.Date == DateFilter.Date);
		}

		foreach (var transfer in query)
		{
			Transfers.Add(transfer);
		}

		HasTransfers = Transfers.Count > 0;
	}

	partial void OnWarehouseFilterChanged(string? value) => ApplyFilters();
	partial void OnProductionOrderFilterChanged(string? value) => ApplyFilters();

	partial void OnDateFilterChanged(DateTime value)
	{
		if (IsDateFilterActive)
		{
			ApplyFilters();
		}
	}

	partial void OnIsDateFilterActiveChanged(bool value) => ApplyFilters();

	partial void OnHasTransfersChanged(bool value) => OnPropertyChanged(nameof(HasNoTransfers));
}
