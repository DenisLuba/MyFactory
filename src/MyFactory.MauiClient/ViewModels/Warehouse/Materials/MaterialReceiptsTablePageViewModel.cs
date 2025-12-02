using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.WarehouseMaterials;
using MyFactory.MauiClient.Pages.Warehouse.Materials;
using MyFactory.MauiClient.Services.WarehouseMaterialsServices;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Materials;

public partial class MaterialReceiptsTablePageViewModel : ObservableObject
{
	private readonly IWarehouseMaterialsService _warehouseMaterialsService;
	private readonly List<MaterialReceiptListResponse> _receiptsCache = new();

	public MaterialReceiptsTablePageViewModel(IWarehouseMaterialsService warehouseMaterialsService)
	{
		_warehouseMaterialsService = warehouseMaterialsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		CreateReceiptCommand = new AsyncRelayCommand(OpenNewReceiptAsync, () => !IsBusy);
		OpenReceiptCommand = new AsyncRelayCommand<MaterialReceiptJournalItem?>(OpenReceiptAsync);
	}

	public ObservableCollection<MaterialReceiptJournalItem> Receipts { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasReceipts;

	[ObservableProperty]
	private string searchText = string.Empty;

	public bool HasNoReceipts => !HasReceipts;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand CreateReceiptCommand { get; }
	public IAsyncRelayCommand<MaterialReceiptJournalItem?> OpenReceiptCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
		CreateReceiptCommand.NotifyCanExecuteChanged();
	}

	partial void OnHasReceiptsChanged(bool value) => OnPropertyChanged(nameof(HasNoReceipts));

	partial void OnSearchTextChanged(string value)
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
			_receiptsCache.Clear();

			var response = await _warehouseMaterialsService.ListReceiptsAsync();
			if (response is { Count: > 0 })
			{
				_receiptsCache.AddRange(response);
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Поступления", $"Не удалось загрузить поступления: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplyFilters()
	{
		Receipts.Clear();

		IEnumerable<MaterialReceiptListResponse> query = _receiptsCache;

		if (!string.IsNullOrWhiteSpace(SearchText))
		{
			var term = SearchText.Trim();
			query = query.Where(receipt =>
				receipt.DocumentNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
				receipt.SupplierName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
				receipt.WarehouseName.Contains(term, StringComparison.OrdinalIgnoreCase));
		}

		foreach (var receipt in query.OrderByDescending(r => r.DocumentDate))
		{
			Receipts.Add(new MaterialReceiptJournalItem(
				receipt.Id,
				$"{receipt.DocumentNumber} · {receipt.WarehouseName}",
				receipt.DocumentDate.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture),
				receipt.SupplierName,
				receipt.TotalAmount,
				GetStatusDisplay(receipt.Status)));
		}

		HasReceipts = Receipts.Count > 0;
	}

	private async Task OpenNewReceiptAsync()
	{
		var viewModel = new MaterialReceiptCardPageViewModel(_warehouseMaterialsService);
		viewModel.Initialize();
		var page = new MaterialReceiptCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private async Task OpenReceiptAsync(MaterialReceiptJournalItem? journalItem)
	{
		if (journalItem is null)
		{
			return;
		}

		var viewModel = new MaterialReceiptCardPageViewModel(_warehouseMaterialsService);
		viewModel.Initialize(journalItem.Id);
		var page = new MaterialReceiptCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	private static string GetStatusDisplay(MaterialReceiptStatus status) => status switch
	{
		MaterialReceiptStatus.Draft => "Черновик",
		MaterialReceiptStatus.Posted => "Проведено",
		MaterialReceiptStatus.LineAdded or MaterialReceiptStatus.LineUpdated => "Изменены строки",
		MaterialReceiptStatus.LineDeleted => "Удалена строка",
		MaterialReceiptStatus.Updated => "Обновлено",
		_ => status.ToString()
	};
}
