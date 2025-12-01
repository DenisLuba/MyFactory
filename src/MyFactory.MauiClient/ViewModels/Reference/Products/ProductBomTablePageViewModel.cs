using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Pages.Reference.Products;
using MyFactory.MauiClient.Services.ProductsServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Products;

public partial class ProductBomTablePageViewModel : ObservableObject
{
	private readonly IProductsService _productsService;

	public ProductBomTablePageViewModel(IProductsService productsService)
	{
		_productsService = productsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		AddItemCommand = new AsyncRelayCommand(OpenAddModalAsync, CanAdd);
		DeleteItemCommand = new AsyncRelayCommand<ProductBomItemResponse?>(DeleteItemAsync, _ => CanModify());
	}

	public Guid ProductId { get; private set; }

	public ObservableCollection<ProductBomItemResponse> Items { get; } = new();

	[ObservableProperty]
	private string productName = string.Empty;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasItems;

	public string Title => string.IsNullOrWhiteSpace(ProductName)
		? "Материалы изделия"
		: $"{ProductName}: Материалы";

	public bool HasNoItems => !HasItems;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand AddItemCommand { get; }
	public IAsyncRelayCommand<ProductBomItemResponse?> DeleteItemCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		AddItemCommand.NotifyCanExecuteChanged();
		DeleteItemCommand.NotifyCanExecuteChanged();
	}

	partial void OnProductNameChanged(string value) => OnPropertyChanged(nameof(Title));

	partial void OnHasItemsChanged(bool value) => OnPropertyChanged(nameof(HasNoItems));

	public void Initialize(Guid productId, string productName)
	{
		ProductId = productId;
		ProductName = productName;
		LoadCommand.NotifyCanExecuteChanged();
		AddItemCommand.NotifyCanExecuteChanged();
		DeleteItemCommand.NotifyCanExecuteChanged();
	}

	private bool CanLoad() => !IsBusy && ProductId != Guid.Empty;

	private bool CanAdd() => !IsBusy && ProductId != Guid.Empty;

	private bool CanModify() => !IsBusy && ProductId != Guid.Empty;

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			Items.Clear();

			var response = await _productsService.GetBomAsync(ProductId) ?? Array.Empty<ProductBomItemResponse>();
			foreach (var item in response)
			{
				Items.Add(item);
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить материалы изделия: {ex.Message}", "OK");
		}
		finally
		{
			HasItems = Items.Count > 0;
			IsBusy = false;
		}
	}

	private async Task OpenAddModalAsync()
	{
		if (!CanAdd())
		{
			return;
		}

		var modalViewModel = new ProductBomItemModalViewModel(_productsService);
		modalViewModel.Initialize(ProductId, ProductName);
		var modalPage = new ProductBomItemModal(modalViewModel);
		var completion = modalViewModel.WaitForResultAsync();
		await Shell.Current.Navigation.PushModalAsync(modalPage);
		var created = await completion;
		if (created is not null)
		{
			Items.Add(created);
			HasItems = Items.Count > 0;
		}
	}

	private async Task DeleteItemAsync(ProductBomItemResponse? item)
	{
		if (item is null || !CanModify())
		{
			return;
		}

		var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить {item.Material}?", "Удалить", "Отмена");
		if (!confirm)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _productsService.DeleteBomItemAsync(ProductId, item.Id);
			if (response?.Status?.Equals("Deleted", StringComparison.OrdinalIgnoreCase) == true)
			{
				Items.Remove(item);
				HasItems = Items.Count > 0;
			}
			else
			{
				await Shell.Current.DisplayAlertAsync("Внимание", "Строка не найдена", "OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось удалить материал: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

}
