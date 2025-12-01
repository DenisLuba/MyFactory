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

public partial class ProductOperationsTablePageViewModel : ObservableObject
{
	private readonly IProductsService _productsService;

	public ProductOperationsTablePageViewModel(IProductsService productsService)
	{
		_productsService = productsService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		AddOperationCommand = new AsyncRelayCommand(OpenAddModalAsync, CanAdd);
		DeleteOperationCommand = new AsyncRelayCommand<ProductOperationItemResponse?>(DeleteOperationAsync, _ => CanModify());
	}

	public Guid ProductId { get; private set; }

	public ObservableCollection<ProductOperationItemResponse> Operations { get; } = new();

	[ObservableProperty]
	private string productName = string.Empty;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasOperations;

	public string Title => string.IsNullOrWhiteSpace(ProductName)
		? "Операции изделия"
		: $"{ProductName}: Операции";

	public bool HasNoOperations => !HasOperations;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand AddOperationCommand { get; }
	public IAsyncRelayCommand<ProductOperationItemResponse?> DeleteOperationCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		AddOperationCommand.NotifyCanExecuteChanged();
		DeleteOperationCommand.NotifyCanExecuteChanged();
	}

	partial void OnProductNameChanged(string value) => OnPropertyChanged(nameof(Title));

	partial void OnHasOperationsChanged(bool value) => OnPropertyChanged(nameof(HasNoOperations));

	public void Initialize(Guid productId, string productName)
	{
		ProductId = productId;
		ProductName = productName;
		LoadCommand.NotifyCanExecuteChanged();
		AddOperationCommand.NotifyCanExecuteChanged();
		DeleteOperationCommand.NotifyCanExecuteChanged();
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
			Operations.Clear();

			var response = await _productsService.GetOperationsAsync(ProductId) ?? Array.Empty<ProductOperationItemResponse>();
			foreach (var operation in response)
			{
				Operations.Add(operation);
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить операции изделия: {ex.Message}", "OK");
		}
		finally
		{
			HasOperations = Operations.Count > 0;
			IsBusy = false;
		}
	}

	private async Task OpenAddModalAsync()
	{
		if (!CanAdd())
		{
			return;
		}

		var modalViewModel = new ProductOperationModalViewModel(_productsService);
		modalViewModel.Initialize(ProductId, ProductName);
		var modalPage = new ProductOperationModal(modalViewModel);
		var completion = modalViewModel.WaitForResultAsync();
		await Shell.Current.Navigation.PushModalAsync(modalPage);
		var created = await completion;
		if (created is not null)
		{
			Operations.Add(created);
			HasOperations = Operations.Count > 0;
		}
	}

	private async Task DeleteOperationAsync(ProductOperationItemResponse? operation)
	{
		if (operation is null || !CanModify())
		{
			return;
		}

		var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить '{operation.Operation}'?", "Удалить", "Отмена");
		if (!confirm)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _productsService.DeleteOperationAsync(ProductId, operation.Id);
			if (response?.Status?.Equals("Deleted", StringComparison.OrdinalIgnoreCase) == true)
			{
				Operations.Remove(operation);
				HasOperations = Operations.Count > 0;
			}
			else
			{
				await Shell.Current.DisplayAlertAsync("Внимание", "Строка не найдена", "OK");
			}
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось удалить операцию: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

}
