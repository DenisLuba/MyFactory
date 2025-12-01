using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Production.ProductionOrders;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.ProductionServices;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

public partial class ProductionOrdersTablePageViewModel : ObservableObject
{
	private readonly IProductionOrdersService _productionOrdersService;

	public ProductionOrdersTablePageViewModel(IProductionOrdersService productionOrdersService)
	{
		_productionOrdersService = productionOrdersService;
	}

	public ObservableCollection<ProductionOrderListResponse> Orders { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasOrders;

	public bool HasNoOrders => !HasOrders;

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
			Orders.Clear();

			var response = await _productionOrdersService.GetListAsync();
			if (response is { Count: > 0 })
			{
				foreach (var order in response.OrderByDescending(o => o.StartDate))
				{
					Orders.Add(order);
				}
			}

			HasOrders = Orders.Count > 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить заказы: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task OpenCardAsync(ProductionOrderListResponse? selected)
	{
		if (selected is null)
		{
			return;
		}

		var cardViewModel = new ProductionOrderCardPageViewModel(selected.OrderId, _productionOrdersService);
		var page = new ProductionOrderCardPage(cardViewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	partial void OnHasOrdersChanged(bool value) => OnPropertyChanged(nameof(HasNoOrders));
}
