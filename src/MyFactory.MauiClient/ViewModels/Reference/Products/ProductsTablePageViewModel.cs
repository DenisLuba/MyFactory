using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Pages.Reference.Products;
using MyFactory.MauiClient.Services.ProductsServices;
using MyFactory.MauiClient.UIModels.Reference;

namespace MyFactory.MauiClient.ViewModels.Reference.Products;

public partial class ProductsTablePageViewModel : ObservableObject
{
	private readonly IProductsService _productsService;

	public ProductsTablePageViewModel(IProductsService productsService)
	{
		_productsService = productsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		OpenCardCommand = new AsyncRelayCommand<ProductItem?>(OpenCardAsync);
	}

	public ObservableCollection<ProductItem> Products { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasProducts;

	public bool HasNoProducts => !HasProducts;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<ProductItem?> OpenCardCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
	}

	partial void OnHasProductsChanged(bool value) => OnPropertyChanged(nameof(HasNoProducts));

	private async Task LoadAsync()
	{
		if (IsBusy)
		{
			return;
		}

		try
		{
			IsBusy = true;
			Products.Clear();

			var response = await _productsService.GetProductsAsync() ?? Array.Empty<ProductsListResponse>();
			foreach (var product in response.OrderBy(p => p.Sku))
			{
				Products.Add(new ProductItem(
					product.Id,
					product.Sku,
					product.Name,
					product.PlanPerHour,
					product.Status,
					product.ImageCount));
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить изделия: {ex.Message}", "OK");
		}
		finally
		{
			HasProducts = Products.Count > 0;
			IsBusy = false;
		}
	}

	private async Task OpenCardAsync(ProductItem? product)
	{
		if (product is null)
		{
			return;
		}

		var viewModel = new ProductCardPageViewModel(_productsService);
		viewModel.Initialize(product.Id);
		var page = new ProductCardPage(viewModel);
		await Shell.Current.Navigation.PushAsync(page);
	}
}
