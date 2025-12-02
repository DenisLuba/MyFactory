using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Specifications;
using MyFactory.MauiClient.Services.MaterialsServices;
using MyFactory.MauiClient.Services.SpecificationsServices;

namespace MyFactory.MauiClient.ViewModels.Specifications;

public partial class SpecificationBomPageViewModel : ObservableObject
{
	private readonly ISpecificationsService _specificationsService;
	private readonly IMaterialsService _materialsService;
	private IReadOnlyList<MaterialListResponse>? _materialsCache;

	public SpecificationBomPageViewModel(
		ISpecificationsService specificationsService,
		IMaterialsService materialsService)
	{
		_specificationsService = specificationsService;
		_materialsService = materialsService;

		LoadCommand = new AsyncRelayCommand(LoadAsync, CanInteract);
		AddItemCommand = new AsyncRelayCommand(AddItemAsync, CanInteract);
		DeleteItemCommand = new AsyncRelayCommand<SpecificationBomItemResponse?>(DeleteItemAsync);
	}

	public Guid SpecificationId { get; private set; }

	public ObservableCollection<SpecificationBomItemResponse> Items { get; } = new();

	[ObservableProperty]
	private string specificationName = string.Empty;

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasItems;

	public string Title => string.IsNullOrWhiteSpace(SpecificationName)
		? "Спецификация"
		: $"{SpecificationName}: BOM";

	public bool HasNoItems => !HasItems;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand AddItemCommand { get; }
	public IAsyncRelayCommand<SpecificationBomItemResponse?> DeleteItemCommand { get; }

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		AddItemCommand.NotifyCanExecuteChanged();
	}

	partial void OnSpecificationNameChanged(string value) => OnPropertyChanged(nameof(Title));

	partial void OnHasItemsChanged(bool value) => OnPropertyChanged(nameof(HasNoItems));

	public void Initialize(Guid specificationId, string specificationName)
	{
		SpecificationId = specificationId;
		SpecificationName = specificationName;
		LoadCommand.NotifyCanExecuteChanged();
		AddItemCommand.NotifyCanExecuteChanged();
	}

	private bool CanInteract() => !IsBusy && SpecificationId != Guid.Empty;

	private async Task LoadAsync()
	{
		if (!CanInteract())
		{
			return;
		}

		try
		{
			IsBusy = true;
			Items.Clear();
			var bom = await _specificationsService.GetBomAsync(SpecificationId) ?? Array.Empty<SpecificationBomItemResponse>();
			foreach (var line in bom)
			{
				Items.Add(line);
			}
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить BOM: {ex.Message}");
		}
		finally
		{
			HasItems = Items.Count > 0;
			IsBusy = false;
		}
	}

	private async Task AddItemAsync()
	{
		if (!CanInteract())
		{
			return;
		}

		try
		{
			IsBusy = true;
			_materialsCache = await _materialsService.ListAsync();
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось загрузить материалы: {ex.Message}");
			IsBusy = false;
			return;
		}

		IsBusy = false;
		var materials = _materialsCache ?? Array.Empty<MaterialListResponse>();
		if (materials.Count == 0)
		{
			await ShowAlertAsync("Внимание", "Материалы не найдены", "Закрыть");
			return;
		}

		var materialNames = materials.Select(m => m.Name).ToArray();
		var selectedName = await Shell.Current.DisplayActionSheetAsync("Материал", "Отмена", null, materialNames);
		if (string.IsNullOrWhiteSpace(selectedName))
		{
			return;
		}

		var selectedMaterial = materials.FirstOrDefault(m => m.Name == selectedName);
		if (selectedMaterial is null)
		{
			await ShowAlertAsync("Ошибка", "Материал не найден", "OK");
			return;
		}

		var qtyText = await Shell.Current.DisplayPromptAsync("Количество", "Введите количество", initialValue: "1", maxLength: -1, keyboard: Keyboard.Numeric);
		if (!double.TryParse(qtyText, NumberStyles.Float, CultureInfo.CurrentCulture, out var qty) &&
			!double.TryParse(qtyText, NumberStyles.Float, CultureInfo.InvariantCulture, out qty))
		{
			await ShowAlertAsync("Ошибка", "Количество должно быть числом");
			return;
		}

		if (qty <= 0)
		{
			await ShowAlertAsync("Ошибка", "Количество должно быть больше нуля");
			return;
		}

		var unit = await Shell.Current.DisplayPromptAsync("Ед. изм.", "Введите единицу измерения", initialValue: selectedMaterial.Unit);
		if (string.IsNullOrWhiteSpace(unit))
		{
			await ShowAlertAsync("Ошибка", "Ед. измерения обязательна");
			return;
		}

		var priceInitial = selectedMaterial.LastPrice.ToString("0.##", CultureInfo.InvariantCulture);
		var priceText = await Shell.Current.DisplayPromptAsync("Цена", "Введите цену", initialValue: priceInitial, keyboard: Keyboard.Numeric);
		if (!decimal.TryParse(priceText, NumberStyles.Float, CultureInfo.CurrentCulture, out var price) &&
			!decimal.TryParse(priceText, NumberStyles.Float, CultureInfo.InvariantCulture, out price))
		{
			await ShowAlertAsync("Ошибка", "Цена должна быть числом");
			return;
		}

		try
		{
			IsBusy = true;
			var request = new SpecificationsAddBomRequest(selectedMaterial.Id, qty, unit.Trim(), price);
			var response = await _specificationsService.AddBomAsync(SpecificationId, request);
			if (response?.Item is not null)
			{
				Items.Add(response.Item);
				HasItems = Items.Count > 0;
			}
			else
			{
				await ShowAlertAsync("Ошибка", "Не удалось добавить строку");
			}
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось сохранить строку BOM: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private async Task DeleteItemAsync(SpecificationBomItemResponse? item)
	{
		if (item is null || !CanInteract())
		{
			return;
		}

		var confirm = await Shell.Current.DisplayAlertAsync(
			"Удаление",
			$"Удалить '{item.Material}'?",
			"Удалить",
			"Отмена");

		if (!confirm)
		{
			return;
		}

		try
		{
			IsBusy = true;
			var response = await _specificationsService.DeleteBomItemAsync(SpecificationId, item.Id);
			if (response?.Status == SpecificationsStatus.BomDeleted)
			{
				Items.Remove(item);
				HasItems = Items.Count > 0;
			}
			else
			{
				await ShowAlertAsync("Внимание", "Строка не найдена", "OK");
			}
		}
		catch (Exception ex)
		{
			await ShowAlertAsync("Ошибка", $"Не удалось удалить строку: {ex.Message}");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private static Task ShowAlertAsync(string title, string message, string accept = "OK")
		=> Shell.Current.DisplayAlertAsync(title, message, accept);
}
