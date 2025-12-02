using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Pages.Reference.Warehouses;
using MyFactory.MauiClient.Services.WarehousesServices;
using MyFactory.MauiClient.UIModels.Reference;

namespace MyFactory.MauiClient.ViewModels.Reference.Warehouses;

public partial class WarehousesTablePageViewModel : ObservableObject
{
	private readonly IWarehousesService _warehousesService;

	public WarehousesTablePageViewModel(IWarehousesService warehousesService)
	{
		_warehousesService = warehousesService;

		LoadWarehousesCommand = new AsyncRelayCommand(LoadWarehousesAsync);
		RefreshCommand = new AsyncRelayCommand(LoadWarehousesAsync);
		OpenWarehouseCommand = new AsyncRelayCommand<WarehouseItem?>(OpenWarehouseAsync);
	}

	public ObservableCollection<WarehouseItem> Warehouses { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasWarehouses;

	public IAsyncRelayCommand LoadWarehousesCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<WarehouseItem?> OpenWarehouseCommand { get; }

	private async Task LoadWarehousesAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Warehouses.Clear();

			var response = await _warehousesService.ListAsync();

			if (response is null)
			{
				HasWarehouses = false;
				return;
			}

			foreach (var warehouse in response.OrderBy(w => w.Code))
			{
				Warehouses.Add(new WarehouseItem(
					warehouse.Id,
					warehouse.Code,
					warehouse.Name,
					warehouse.Type,
					warehouse.Status));
			}

			HasWarehouses = Warehouses.Any();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить склады: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task OpenWarehouseAsync(WarehouseItem? warehouse)
	{
		if (warehouse is null)
		{
			return;
		}

		await Shell.Current.GoToAsync(nameof(WarehouseCardPage), true, new Dictionary<string, object>
		{
			{ "WarehouseId", warehouse.Id }
		});
	}
}
