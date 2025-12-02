using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Inventory;
using MyFactory.MauiClient.Services.InventoryServices;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Materials;

public partial class MaterialStockTablePageViewModel : ObservableObject
{
	private readonly IInventoryService _inventoryService;
	private readonly List<InventoryItemResponse> _inventoryCache = new();

	public MaterialStockTablePageViewModel(IInventoryService inventoryService)
	{
		_inventoryService = inventoryService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
	}

	public ObservableCollection<MaterialStockItem> Stock { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasItems;

	[ObservableProperty]
	private string searchText = string.Empty;

	[ObservableProperty]
	private string warehouseFilter = string.Empty;

	[ObservableProperty]
	private decimal totalAmount;

	public bool HasNoItems => !HasItems;
	public string TotalAmountDisplay => TotalAmount.ToString("N2", CultureInfo.CurrentCulture) + " ₽";

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
	}

	partial void OnHasItemsChanged(bool value) => OnPropertyChanged(nameof(HasNoItems));
	partial void OnTotalAmountChanged(decimal value) => OnPropertyChanged(nameof(TotalAmountDisplay));

	partial void OnSearchTextChanged(string value)
	{
		if (!IsBusy)
		{
			ApplyFilters();
		}
	}

	partial void OnWarehouseFilterChanged(string value)
	{
		if (!IsBusy)
		{
			ApplyFilters();
		}
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
			_inventoryCache.Clear();

			var response = await _inventoryService.GetAllAsync();
			if (response is { Count: > 0 })
			{
				_inventoryCache.AddRange(response);
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Склад", $"Не удалось получить остатки: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplyFilters()
	{
		Stock.Clear();

		IEnumerable<InventoryItemResponse> query = _inventoryCache;

		if (!string.IsNullOrWhiteSpace(SearchText))
		{
			var term = SearchText.Trim();
			query = query.Where(item =>
				item.MaterialName.Contains(term, StringComparison.OrdinalIgnoreCase));
		}

		if (!string.IsNullOrWhiteSpace(WarehouseFilter))
		{
			var warehouseTerm = WarehouseFilter.Trim();
			query = query.Where(item =>
				item.WarehouseName.Contains(warehouseTerm, StringComparison.OrdinalIgnoreCase));
		}

		foreach (var item in query.OrderBy(i => i.MaterialName))
		{
			Stock.Add(new MaterialStockItem(
				item.MaterialName,
				item.WarehouseName,
				Convert.ToDecimal(item.Quantity, CultureInfo.InvariantCulture),
				item.Unit.ToString(),
				item.AvgPrice,
				item.TotalAmount));
		}

		TotalAmount = Stock.Sum(s => s.TotalAmount);
		HasItems = Stock.Count > 0;
	}
}
