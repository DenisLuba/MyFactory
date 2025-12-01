using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Production.MaterialTransfers;
using MyFactory.MauiClient.Services.ProductionServices;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

public partial class MaterialTransfersForOrderPageViewModel : ObservableObject
{
	private readonly IProductionOrdersService _productionOrdersService;

	public MaterialTransfersForOrderPageViewModel(Guid orderId, IProductionOrdersService productionOrdersService)
	{
		OrderId = orderId;
		_productionOrdersService = productionOrdersService;
	}

	public Guid OrderId { get; }

	public ObservableCollection<MaterialTransferItemDto> Items { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasItems;

	public bool HasNoItems => !HasItems;

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
			Items.Clear();

			var response = await _productionOrdersService.GetMaterialTransfersAsync(OrderId);
			if (response is { Count: > 0 })
			{
				foreach (var item in response)
				{
					Items.Add(item);
				}
			}

			HasItems = Items.Count > 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить передачи: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	partial void OnHasItemsChanged(bool value) => OnPropertyChanged(nameof(HasNoItems));
}
