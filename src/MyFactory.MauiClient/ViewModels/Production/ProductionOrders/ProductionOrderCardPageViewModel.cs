using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Production.ProductionOrders;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.ProductionServices;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

public partial class ProductionOrderCardPageViewModel : ObservableObject
{
	private readonly IProductionOrdersService _productionOrdersService;

	public ProductionOrderCardPageViewModel(Guid orderId, IProductionOrdersService productionOrdersService)
	{
		OrderId = orderId;
		_productionOrdersService = productionOrdersService;
	}

	public Guid OrderId { get; }

	[ObservableProperty]
	private ProductionOrderCardResponse? order;

	[ObservableProperty]
	private bool isBusy;

	public bool HasOrder => Order is not null;
	public bool HasNoOrder => !HasOrder;

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
			Order = await _productionOrdersService.GetByIdAsync(OrderId);
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Ошибка", $"Не удалось загрузить заказ: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task OpenTransfersAsync()
	{
		var viewModel = new MaterialTransfersForOrderPageViewModel(OrderId, _productionOrdersService);
		var page = new MaterialTransfersForOrderPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	[RelayCommand]
	private async Task OpenStageDistributionAsync()
	{
		var viewModel = new StageDistributionPageViewModel(OrderId, _productionOrdersService);
		var page = new StageDistributionPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}

	partial void OnOrderChanged(ProductionOrderCardResponse? value)
	{
		OnPropertyChanged(nameof(HasOrder));
		OnPropertyChanged(nameof(HasNoOrder));
	}
}
