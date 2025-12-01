using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Shipments;
using MyFactory.MauiClient.Pages.FinishedGoods.Shipment;
using MyFactory.MauiClient.Services.ShipmentsServices;
using MyFactory.MauiClient.UIModels.FinishedGoods;

namespace MyFactory.MauiClient.ViewModels.FinishedGoods.Shipment;

public partial class ShipmentsTablePageViewModel : ObservableObject
{
	private readonly IShipmentsService _shipmentsService;
	private readonly List<ShipmentListItem> _allShipments = new();

	public ShipmentsTablePageViewModel(IShipmentsService shipmentsService)
	{
		_shipmentsService = shipmentsService;

		LoadShipmentsCommand = new AsyncRelayCommand(LoadShipmentsAsync);
		RefreshCommand = new AsyncRelayCommand(LoadShipmentsAsync);
		OpenShipmentCommand = new AsyncRelayCommand<ShipmentListItem?>(OpenShipmentAsync);
		CreateShipmentCommand = new AsyncRelayCommand(CreateShipmentAsync);
		ClearFiltersCommand = new RelayCommand(ClearFilters);

		DateFilter = DateTime.Today;
	}

	public ObservableCollection<ShipmentListItem> Shipments { get; } = new();

	public IReadOnlyList<ShipmentStatus> AvailableStatuses { get; } =
		Enum.GetValues(typeof(ShipmentStatus)).Cast<ShipmentStatus>().ToArray();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasShipments;

	[ObservableProperty]
	private string? customerFilter;

	[ObservableProperty]
	private ShipmentStatus? statusFilter;

	[ObservableProperty]
	private DateTime dateFilter;

	[ObservableProperty]
	private bool isDateFilterActive;

	public IAsyncRelayCommand LoadShipmentsCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<ShipmentListItem?> OpenShipmentCommand { get; }
	public IAsyncRelayCommand CreateShipmentCommand { get; }
	public IRelayCommand ClearFiltersCommand { get; }

	private async Task LoadShipmentsAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Shipments.Clear();
			_allShipments.Clear();

			var response = await _shipmentsService.GetShipmentsAsync() ?? new List<ShipmentsListResponse>();

			foreach (var shipment in response.OrderByDescending(s => s.Date))
			{
				var item = new ShipmentListItem(
					shipment.ShipmentId,
					shipment.Customer,
					shipment.ProductName,
					shipment.Quantity,
					shipment.Date,
					shipment.TotalAmount,
					shipment.Status);

				_allShipments.Add(item);
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить отгрузки: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplyFilters()
	{
		Shipments.Clear();

		IEnumerable<ShipmentListItem> query = _allShipments;

		if (!string.IsNullOrWhiteSpace(CustomerFilter))
		{
			var customerTerm = CustomerFilter.Trim();
			query = query.Where(item => item.Customer.Contains(customerTerm, StringComparison.OrdinalIgnoreCase));
		}

		if (StatusFilter.HasValue)
		{
			query = query.Where(item => item.Status == StatusFilter.Value);
		}

		if (IsDateFilterActive)
		{
			query = query.Where(item => item.Date.Date == DateFilter.Date);
		}

		foreach (var shipment in query)
		{
			Shipments.Add(shipment);
		}

		HasShipments = Shipments.Any();
	}

	private async Task OpenShipmentAsync(ShipmentListItem? shipment)
	{
		if (shipment == null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(ShipmentCardPage), true, new Dictionary<string, object>
		{
			{ "ShipmentId", shipment.ShipmentId }
		});
	}

	private async Task CreateShipmentAsync()
	{
		await Shell.Current.DisplayAlert("Создание отгрузки", "Форма создания будет добавлена позже.", "OK");
	}

	private void ClearFilters()
	{
		if (string.IsNullOrWhiteSpace(CustomerFilter) &&
			StatusFilter is null &&
			!IsDateFilterActive)
		{
			return;
		}

		CustomerFilter = null;
		StatusFilter = null;
		IsDateFilterActive = false;
		DateFilter = DateTime.Today;
		ApplyFilters();
	}

	partial void OnCustomerFilterChanged(string? value) => ApplyFilters();
	partial void OnStatusFilterChanged(ShipmentStatus? value) => ApplyFilters();
	partial void OnDateFilterChanged(DateTime value)
	{
		if (IsDateFilterActive)
		{
			ApplyFilters();
		}
	}

	partial void OnIsDateFilterActiveChanged(bool value) => ApplyFilters();
}
