using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Returns;
using MyFactory.MauiClient.Services.ReturnsServices;

namespace MyFactory.MauiClient.ViewModels.FinishedGoods.Returns;

public partial class ReturnCardPageViewModel : ObservableObject
{
	private readonly IReturnsService _returnsService;
	private readonly IReturnLookupService _lookupService;
	private CancellationTokenSource? _customerLookupCts;
	private CancellationTokenSource? _productLookupCts;
	private bool _suppressCustomerQueryUpdates;
	private bool _suppressProductQueryUpdates;

	public ReturnCardPageViewModel(IReturnsService returnsService, IReturnLookupService lookupService)
	{
		_returnsService = returnsService;
        _lookupService = lookupService;
		Date = DateTime.Today;
		LoadReturnCommand = new AsyncRelayCommand(LoadReturnAsync, () => IsExistingReturn && !IsBusy);
		CreateReturnCommand = new AsyncRelayCommand(CreateReturnAsync, () => IsNewReturn && !IsSaving);
		UseCustomerSuggestionCommand = new RelayCommand<LookupSuggestion?>(UseCustomerSuggestion);
		UseProductSuggestionCommand = new RelayCommand<LookupSuggestion?>(UseProductSuggestion);

        _ = RefreshCustomerSuggestionsAsync(string.Empty);
        _ = RefreshProductSuggestionsAsync(string.Empty);
	}

	[ObservableProperty]
	private Guid returnId;

	public bool IsExistingReturn => ReturnId != Guid.Empty;
	public bool IsNewReturn => !IsExistingReturn;

	public IAsyncRelayCommand LoadReturnCommand { get; }
	public IAsyncRelayCommand CreateReturnCommand { get; }

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool isSaving;

	[ObservableProperty]
	private string customer = string.Empty;

	[ObservableProperty]
	private string productName = string.Empty;

	[ObservableProperty]
	private int quantity;

	[ObservableProperty]
	private DateTime date;

	[ObservableProperty]
	private string reason = string.Empty;

	[ObservableProperty]
	private ReturnStatus status;

	[ObservableProperty]
	private string? comment;

	[ObservableProperty]
	private string customerIdText = string.Empty;

	[ObservableProperty]
	private string specificationIdText = string.Empty;

	[ObservableProperty]
	private string quantityText = "1";

    [ObservableProperty]
    private string customerQuery = string.Empty;

    [ObservableProperty]
    private string specificationQuery = string.Empty;

	[ObservableProperty]
	private string selectedCustomerSummary = string.Empty;

	[ObservableProperty]
	private string selectedProductSummary = string.Empty;

	public ObservableCollection<LookupSuggestion> CustomerSuggestions { get; } = new();
	public ObservableCollection<LookupSuggestion> ProductSuggestions { get; } = new();

    public IRelayCommand<LookupSuggestion?> UseCustomerSuggestionCommand { get; }
    public IRelayCommand<LookupSuggestion?> UseProductSuggestionCommand { get; }

	public string DateDisplay => Date == default
		? "-"
		: Date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

	public bool HasComment => !string.IsNullOrWhiteSpace(Comment);
	public bool HasReason => !string.IsNullOrWhiteSpace(Reason);
    public bool HasSelectedCustomer => !string.IsNullOrWhiteSpace(SelectedCustomerSummary);
    public bool HasSelectedProduct => !string.IsNullOrWhiteSpace(SelectedProductSummary);

	partial void OnDateChanged(DateTime value) => OnPropertyChanged(nameof(DateDisplay));

	partial void OnCommentChanged(string? value) => OnPropertyChanged(nameof(HasComment));
	partial void OnReasonChanged(string value) => OnPropertyChanged(nameof(HasReason));
	partial void OnSelectedCustomerSummaryChanged(string value) => OnPropertyChanged(nameof(HasSelectedCustomer));
	partial void OnSelectedProductSummaryChanged(string value) => OnPropertyChanged(nameof(HasSelectedProduct));
	partial void OnCustomerQueryChanged(string value)
	{
		if (_suppressCustomerQueryUpdates)
		{
			_suppressCustomerQueryUpdates = false;
			return;
		}

		_ = RefreshCustomerSuggestionsAsync(value);
	}

	partial void OnSpecificationQueryChanged(string value)
	{
		if (_suppressProductQueryUpdates)
		{
			_suppressProductQueryUpdates = false;
			return;
		}

		_ = RefreshProductSuggestionsAsync(value);
	}

	partial void OnReturnIdChanged(Guid oldValue, Guid newValue)
	{
		OnPropertyChanged(nameof(IsExistingReturn));
		OnPropertyChanged(nameof(IsNewReturn));
		LoadReturnCommand.NotifyCanExecuteChanged();
		CreateReturnCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsBusyChanged(bool value) => LoadReturnCommand.NotifyCanExecuteChanged();
	partial void OnIsSavingChanged(bool value) => CreateReturnCommand.NotifyCanExecuteChanged();

	public void Initialize(Guid returnId) => ReturnId = returnId;

	private async Task LoadReturnAsync()
	{
		if (IsBusy || IsNewReturn)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _returnsService.GetReturnAsync(ReturnId);
			if (response is null)
			{
				await ShowAlertAsync("Ошибка", "Карточка возврата не найдена");
				return;
			}

			Customer = response.Customer;
			ProductName = response.ProductName;
			Quantity = response.Quantity;
			Date = response.Date;
			Reason = response.Reason;
			Status = response.Status;
			Comment = response.Comment;
            SelectedCustomerSummary = response.Customer;
            SelectedProductSummary = response.ProductName;
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить данные возврата: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task CreateReturnAsync()
	{
		if (!IsNewReturn || IsSaving)
		{
			return;
		}

		if (!Guid.TryParse(CustomerIdText, out var customerId))
		{
			await ShowAlertAsync("Ошибка", "Введите корректный GUID клиента");
			return;
		}

		if (!Guid.TryParse(SpecificationIdText, out var specificationId))
		{
			await ShowAlertAsync("Ошибка", "Введите корректный GUID спецификации");
			return;
		}

		if (!int.TryParse(QuantityText, out var qty) || qty <= 0)
		{
			await ShowAlertAsync("Ошибка", "Количество должно быть положительным целым числом");
			return;
		}

		if (string.IsNullOrWhiteSpace(Reason))
		{
			await ShowAlertAsync("Ошибка", "Укажите причину возврата");
			return;
		}

		try
		{
			IsSaving = true;
			var request = new ReturnsCreateRequest(
				customerId,
				specificationId,
				qty,
				Reason.Trim(),
				Date == default ? DateTime.Today : Date
			);

			var response = await _returnsService.CreateReturnAsync(request);
			if (response is null)
			{
				await ShowAlertAsync("Ошибка", "Сервис не вернул данные по созданному возврату");
				return;
			}

			await ShowAlertAsync("Готово", $"Возврат создан со статусом: {response.Status}");
			ReturnId = response.ReturnId;
			await LoadReturnAsync();
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось создать возврат: {ex.Message}");
		}
		finally
		{
			IsSaving = false;
		}
	}

	private static Task ShowAlertAsync(string title, string message)
		=> Shell.Current.DisplayAlertAsync(title, message, "OK");

	private async Task RefreshCustomerSuggestionsAsync(string? searchTerm)
	{
		_customerLookupCts?.Cancel();
		var cts = new CancellationTokenSource();
		_customerLookupCts = cts;

		try
		{
			var suggestions = await _lookupService.GetCustomerSuggestionsAsync(searchTerm, cts.Token);
			if (cts.IsCancellationRequested)
			{
				return;
			}

			ApplySuggestions(CustomerSuggestions, suggestions);
		}
		catch (OperationCanceledException)
		{
		}
	}

	private async Task RefreshProductSuggestionsAsync(string? searchTerm)
	{
		_productLookupCts?.Cancel();
		var cts = new CancellationTokenSource();
		_productLookupCts = cts;

		try
		{
			var suggestions = await _lookupService.GetProductSuggestionsAsync(searchTerm, cts.Token);
			if (cts.IsCancellationRequested)
			{
				return;
			}

			ApplySuggestions(ProductSuggestions, suggestions);
		}
		catch (OperationCanceledException)
		{
		}
	}

	private static void ApplySuggestions(ObservableCollection<LookupSuggestion> target, IReadOnlyList<LookupSuggestion> suggestions)
	{
		target.Clear();
		if (suggestions is null)
		{
			return;
		}

		foreach (var suggestion in suggestions)
		{
			target.Add(suggestion);
		}
	}

	private void UseCustomerSuggestion(LookupSuggestion? suggestion)
	{
		if (suggestion is null)
		{
			return;
		}

		_suppressCustomerQueryUpdates = true;
		CustomerQuery = suggestion.DisplayName;
		CustomerIdText = suggestion.Id.ToString();
		Customer = suggestion.DisplayName;
		SelectedCustomerSummary = BuildSummaryText(suggestion);
		CustomerSuggestions.Clear();
		_ = ShowToastAsync($"Клиент выбран: {suggestion.DisplayName}");
	}

	private void UseProductSuggestion(LookupSuggestion? suggestion)
	{
		if (suggestion is null)
		{
			return;
		}

		_suppressProductQueryUpdates = true;
		SpecificationQuery = suggestion.DisplayName;
		SpecificationIdText = suggestion.Id.ToString();
		ProductName = suggestion.DisplayName;
		SelectedProductSummary = BuildSummaryText(suggestion);
		ProductSuggestions.Clear();
		_ = ShowToastAsync($"Изделие привязано: {suggestion.DisplayName}");
	}

	private static string BuildSummaryText(LookupSuggestion suggestion)
	{
		var details = string.IsNullOrWhiteSpace(suggestion.Details) ? string.Empty : $" • {suggestion.Details}";
		return $"{suggestion.DisplayName}{details}";
	}

	private Task ShowToastAsync(string message)
	{
		var toast = Toast.Make(message, ToastDuration.Short);
		return MainThread.InvokeOnMainThreadAsync(() => toast.Show());
	}
}
