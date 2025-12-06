using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Pages.Reference.Materials;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.SuppliersServices;

namespace MyFactory.MauiClient.ViewModels.Reference.Materials;

public partial class MaterialCardPageViewModel : ObservableObject
{
	private readonly IMaterialsService _materialsService;
	private readonly ISuppliersService _suppliersService;

	public MaterialCardPageViewModel(IMaterialsService materialsService, ISuppliersService suppliersService)
	{
		_materialsService = materialsService;
		_suppliersService = suppliersService;
		LoadMaterialCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
		AddPriceCommand = new AsyncRelayCommand(OpenAddPriceModalAsync, CanExecutePriceActions);
	}

	public ObservableCollection<MaterialPriceHistoryItem> PriceHistory { get; } = [];

	[ObservableProperty]
	private Guid materialId;

	[ObservableProperty]
	private string code = string.Empty;

	[ObservableProperty]
	private string name = string.Empty;

	[ObservableProperty]
	private string materialType = string.Empty;

	[ObservableProperty]
	private string unit = string.Empty;

	[ObservableProperty]
	private bool isActive;

	[ObservableProperty]
	private string supplierName = string.Empty;

	[ObservableProperty]
	private decimal lastPrice;

	[ObservableProperty]
	private bool isBusy;

	public IAsyncRelayCommand LoadMaterialCommand { get; }
	public IAsyncRelayCommand AddPriceCommand { get; }

	public string StatusDisplay => IsActive ? "Активен" : "Неактивен";
	public string SupplierDisplay => string.IsNullOrWhiteSpace(SupplierName) ? "-" : SupplierName;
	public string LastPriceDisplay => LastPrice <= 0 ? "-" : $"{LastPrice:N2} ₽";
	public bool HasPriceHistory => PriceHistory.Count > 0;

	public void Initialize(Guid materialId, string materialCode, string materialName)
	{
		MaterialId = materialId;
		Code = materialCode;
		Name = materialName;
	}

	partial void OnMaterialIdChanged(Guid oldValue, Guid newValue)
	{
		LoadMaterialCommand.NotifyCanExecuteChanged();
		AddPriceCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsBusyChanged(bool value)
	{
		LoadMaterialCommand.NotifyCanExecuteChanged();
		AddPriceCommand.NotifyCanExecuteChanged();
	}

	partial void OnIsActiveChanged(bool value) => OnPropertyChanged(nameof(StatusDisplay));
	partial void OnSupplierNameChanged(string value) => OnPropertyChanged(nameof(SupplierDisplay));
	partial void OnLastPriceChanged(decimal value) => OnPropertyChanged(nameof(LastPriceDisplay));

	private bool CanLoad() => !IsBusy && MaterialId != Guid.Empty;
	private bool CanExecutePriceActions() => !IsBusy && MaterialId != Guid.Empty;

	private async Task LoadAsync()
	{
		if (!CanLoad())
		{
			return;
		}

		try
		{
			IsBusy = true;
			var material = await _materialsService.GetAsync(MaterialId.ToString());
			if (material is null)
			{
				await Shell.Current.DisplayAlertAsync("Материал", "Карточка материала не найдена", "OK");
				return;
			}

			UpdatePriceHistory(material.PriceHistory);

			Code = material.Code;
			Name = material.Name;
			MaterialType = material.MaterialType;
			Unit = material.Unit;
			IsActive = material.IsActive;
			SupplierName = PriceHistory.OrderByDescending(h => h.EffectiveFrom).FirstOrDefault()?.SupplierName ?? string.Empty;
			LastPrice = PriceHistory.OrderByDescending(h => h.EffectiveFrom).FirstOrDefault()?.Price ?? 0;
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить материал: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void UpdatePriceHistory(IEnumerable<MaterialPriceHistoryItem>? history)
	{
		PriceHistory.Clear();
		if (history is not null)
		{
			foreach (var item in history.OrderByDescending(h => h.EffectiveFrom))
			{
				PriceHistory.Add(item);
			}
		}

		OnPropertyChanged(nameof(HasPriceHistory));
	}

	private async Task OpenAddPriceModalAsync()
	{
		if (!CanExecutePriceActions())
		{
			return;
		}

		var modalViewModel = new MaterialPriceAddModalViewModel(_materialsService, _suppliersService);
		modalViewModel.Initialize(MaterialId, Code, Name);
		var modalPage = new MaterialPriceAddModal(modalViewModel);

		var completion = modalViewModel.WaitForCompletionAsync();
		await Shell.Current.Navigation.PushModalAsync(modalPage);
		var result = await completion;

		if (result)
		{
			await LoadAsync();
		}
	}
}
