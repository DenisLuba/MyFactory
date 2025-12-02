using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Purchases;
using MyFactory.MauiClient.Pages.Warehouse.Purchases;
using MyFactory.MauiClient.Services.PurchasesServices;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

public partial class PurchaseRequestsTablePageViewModel : ObservableObject
{
	private readonly IPurchasesService _purchasesService;
	private readonly IServiceProvider _serviceProvider;
	private readonly List<PurchasesResponse> _requestsCache = new();

	public PurchaseRequestsTablePageViewModel(IPurchasesService purchasesService, IServiceProvider serviceProvider)
	{
		_purchasesService = purchasesService;
		_serviceProvider = serviceProvider;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		CreateRequestCommand = new AsyncRelayCommand(OpenNewRequestAsync, () => !IsBusy);
		OpenRequestCommand = new AsyncRelayCommand<PurchaseRequestListItem?>(OpenRequestAsync);
	}

	public ObservableCollection<PurchaseRequestListItem> Requests { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasRequests;

	[ObservableProperty]
	private string searchText = string.Empty;

	public bool HasNoRequests => !HasRequests;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand CreateRequestCommand { get; }
	public IAsyncRelayCommand<PurchaseRequestListItem?> OpenRequestCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
		CreateRequestCommand.NotifyCanExecuteChanged();
	}

	partial void OnHasRequestsChanged(bool value) => OnPropertyChanged(nameof(HasNoRequests));

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
			_requestsCache.Clear();

			var response = await _purchasesService.PurchasesListAsync();
			if (response is { Count: > 0 })
			{
				_requestsCache.AddRange(response);
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ведомости", $"Не удалось загрузить ведомости: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplyFilters()
	{
		Requests.Clear();

		IEnumerable<PurchasesResponse> query = _requestsCache;

		if (!string.IsNullOrWhiteSpace(SearchText))
		{
			var term = SearchText.Trim();
			query = query.Where(request =>
			{
				var summaryItems = request.ItemsSummary ?? Array.Empty<string>();
				return request.DocumentNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
				       summaryItems.Any(item => item.Contains(term, StringComparison.OrdinalIgnoreCase)) ||
				       GetStatusDisplay(request.Status).Contains(term, StringComparison.OrdinalIgnoreCase);
			});
		}

		foreach (var request in query.OrderByDescending(r => r.CreatedAt))
		{
			var summaryItems = request.ItemsSummary ?? Array.Empty<string>();
			var summary = summaryItems.Length > 0 ? string.Join(", ", summaryItems.Take(3)) : "Нет позиций";
			Requests.Add(new PurchaseRequestListItem(
				request.PurchaseId,
				request.DocumentNumber,
				request.CreatedAt,
				summary,
				request.TotalAmount,
				GetStatusDisplay(request.Status)));
		}

		HasRequests = Requests.Count > 0;
	}

	private async Task OpenNewRequestAsync()
	{
		var page = _serviceProvider.GetRequiredService<PurchaseRequestCardPage>();
		if (page.BindingContext is PurchaseRequestCardPageViewModel viewModel)
		{
			viewModel.Initialize();
		}

		await Shell.Current.Navigation.PushAsync(page);
	}

	private async Task OpenRequestAsync(PurchaseRequestListItem? requestItem)
	{
		if (requestItem is null)
		{
			return;
		}

		var page = _serviceProvider.GetRequiredService<PurchaseRequestCardPage>();
		if (page.BindingContext is PurchaseRequestCardPageViewModel viewModel)
		{
			viewModel.Initialize(requestItem.PurchaseId);
		}

		await Shell.Current.Navigation.PushAsync(page);
	}

	private static string GetStatusDisplay(PurchasesStatus status) => status switch
	{
		PurchasesStatus.Draft => "Черновик",
		PurchasesStatus.Created => "Создано",
		PurchasesStatus.Converted => "Преобразовано",
		PurchasesStatus.ConvertedToOrder => "Заказ создан",
		_ => status.ToString()
	};
}
