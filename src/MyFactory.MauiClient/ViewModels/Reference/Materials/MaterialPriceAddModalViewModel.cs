using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.SuppliersServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Materials;

public partial class MaterialPriceAddModalViewModel : ObservableObject
{
	private readonly IMaterialsService _materialsService;
	private readonly ISuppliersService _suppliersService;
	private readonly TaskCompletionSource<bool> _closingCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);
	private bool _suppliersLoaded;
	private bool _isClosing;

	public MaterialPriceAddModalViewModel(IMaterialsService materialsService, ISuppliersService suppliersService)
	{
		_materialsService = materialsService;
		_suppliersService = suppliersService;
		SubmitCommand = new AsyncRelayCommand(SaveAsync, () => !IsSubmitting && !IsSuppliersLoading);
		CancelCommand = new AsyncRelayCommand(CancelAsync);
		EffectiveFrom = DateTime.Today;
	}

	public ObservableCollection<SupplierResponse> Suppliers { get; } = [];

	[ObservableProperty]
	private Guid materialId;

	[ObservableProperty]
	private string materialCode = string.Empty;

	[ObservableProperty]
	private string materialName = string.Empty;

	[ObservableProperty]
	private string priceText = string.Empty;

	[ObservableProperty]
	private DateTime effectiveFrom;

	[ObservableProperty]
	private bool isSubmitting;

	[ObservableProperty]
	private SupplierResponse? selectedSupplier;

	[ObservableProperty]
	private bool isSuppliersLoading;

	public IAsyncRelayCommand SubmitCommand { get; }
	public IAsyncRelayCommand CancelCommand { get; }

	public void Initialize(Guid materialId, string materialCode, string materialName)
	{
		MaterialId = materialId;
		MaterialCode = materialCode;
		MaterialName = materialName;
		PriceText = string.Empty;
		EffectiveFrom = DateTime.Today;
		SelectedSupplier = null;
		Suppliers.Clear();
		_suppliersLoaded = false;
	}

	public async Task EnsureSuppliersLoadedAsync()
	{
		if (_suppliersLoaded || IsSuppliersLoading)
		{
			return;
		}

		await LoadSuppliersAsync();
	}

	public Task<bool> WaitForCompletionAsync() => _closingCompletion.Task;

	public void NotifyClosedExternally()
	{
		if (_closingCompletion.Task.IsCompleted)
		{
			return;
		}

		_closingCompletion.TrySetResult(false);
	}

	partial void OnIsSubmittingChanged(bool value) => SubmitCommand.NotifyCanExecuteChanged();
	partial void OnIsSuppliersLoadingChanged(bool value) => SubmitCommand.NotifyCanExecuteChanged();

	private async Task SaveAsync()
	{
		if (IsSubmitting)
		{
			return;
		}

		if (MaterialId == Guid.Empty)
		{
			await ShowAlertAsync("Ошибка", "Материал не выбран");
			return;
		}

		if (SelectedSupplier is null)
		{
			await ShowAlertAsync("Ошибка", "Выберите поставщика");
			return;
		}

		var priceInput = PriceText?.Trim() ?? string.Empty;
		if ((!decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.CurrentCulture, out var price) &&
			 !decimal.TryParse(priceInput, NumberStyles.Number, CultureInfo.InvariantCulture, out price)) ||
			price <= 0)
		{
			await ShowAlertAsync("Ошибка", "Цена должна быть положительным числом");
			return;
		}

		var effectiveDate = EffectiveFrom == default ? DateTime.Today : EffectiveFrom;

		try
		{
			IsSubmitting = true;
			var request = new AddMaterialPriceRequest(SelectedSupplier.Id, price, effectiveDate);
			var response = await _materialsService.AddPriceAsync(MaterialId.ToString(), request);
			var statusText = response?.Status switch
			{
				MaterialPriceStatus.PriceAdded => "Цена добавлена",
				MaterialPriceStatus.PriceRemoved => "Цена удалена",
				MaterialPriceStatus.PriceUpdated => "Цена обновлена",
				_ => "Цена сохранена"
			};

			await ShowAlertAsync("Готово", statusText);
			await CloseAsync(true);
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось сохранить цену: {ex.Message}");
		}
		finally
		{
			IsSubmitting = false;
		}
	}

	private async Task CancelAsync() => await CloseAsync(false);

	private async Task CloseAsync(bool result)
	{
		if (_isClosing)
		{
			return;
		}

		_isClosing = true;
		_closingCompletion.TrySetResult(result);

		if (Shell.Current?.Navigation is not null)
		{
			await Shell.Current.Navigation.PopModalAsync();
		}
	}

	private static Task ShowAlertAsync(string title, string message)
		=> Shell.Current.DisplayAlertAsync(title, message, "OK");

	private async Task LoadSuppliersAsync()
	{
		try
		{
			IsSuppliersLoading = true;
			var suppliers = await _suppliersService.ListAsync();

			Suppliers.Clear();
			if (suppliers is { Count: > 0 })
			{
				foreach (var supplier in suppliers.OrderBy(s => s.Name, StringComparer.CurrentCultureIgnoreCase))
				{
					Suppliers.Add(supplier);
				}
			}

			SelectedSupplier ??= Suppliers.FirstOrDefault();
			_suppliersLoaded = true;
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить поставщиков: {ex.Message}");
		}
		finally
		{
			IsSuppliersLoading = false;
		}
	}
}
