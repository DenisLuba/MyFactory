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
using MyFactory.MauiClient.UIModels.Reference;

namespace MyFactory.MauiClient.ViewModels.Reference.Materials;

public partial class MaterialsTablePageViewModel : ObservableObject
{
	private readonly IMaterialsService _materialsService;
	private readonly ISuppliersService _suppliersService;
	private readonly List<MaterialListResponse> _materialsCache = new();

	public MaterialsTablePageViewModel(IMaterialsService materialsService, ISuppliersService suppliersService)
	{
		_materialsService = materialsService;
		_suppliersService = suppliersService;
		LoadCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		RefreshCommand = new AsyncRelayCommand(LoadAsync, () => !IsBusy);
		OpenCardCommand = new AsyncRelayCommand<MaterialItem?>(OpenCardAsync);
	}

	public ObservableCollection<MaterialItem> Materials { get; } = new();

	[ObservableProperty]
	private bool isBusy;

	[ObservableProperty]
	private bool hasMaterials;

	[ObservableProperty]
	private string searchText = string.Empty;

	[ObservableProperty]
	private string? typeFilter;

	public bool HasNoMaterials => !HasMaterials;

	public IAsyncRelayCommand LoadCommand { get; }
	public IAsyncRelayCommand RefreshCommand { get; }
	public IAsyncRelayCommand<MaterialItem?> OpenCardCommand { get; }

	partial void OnHasMaterialsChanged(bool value) => OnPropertyChanged(nameof(HasNoMaterials));

	partial void OnIsBusyChanged(bool value)
	{
		LoadCommand.NotifyCanExecuteChanged();
		RefreshCommand.NotifyCanExecuteChanged();
	}

	partial void OnSearchTextChanged(string value)
	{
		if (!IsBusy)
		{
			ApplyFilters();
		}
	}

	partial void OnTypeFilterChanged(string? value)
	{
		if (!IsBusy)
		{
			_ = LoadAsync();
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
			_materialsCache.Clear();

			var type = string.IsNullOrWhiteSpace(TypeFilter) ? null : TypeFilter!.Trim();
			var response = await _materialsService.ListAsync(type);
			if (response is { Count: > 0 })
			{
				_materialsCache.AddRange(response);
			}

			ApplyFilters();
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlertAsync("Ошибка", $"Не удалось загрузить материалы: {ex.Message}", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void ApplyFilters()
	{
		Materials.Clear();

		IEnumerable<MaterialListResponse> query = _materialsCache;
		if (!string.IsNullOrWhiteSpace(SearchText))
		{
			var term = SearchText.Trim();
			query = query.Where(material =>
				material.Code.Contains(term, StringComparison.OrdinalIgnoreCase) ||
				material.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
		}
		if (!string.IsNullOrWhiteSpace(TypeFilter))
		{
			var term = TypeFilter!.Trim();
			query = query.Where(material =>
				material.MaterialType.Contains(term, StringComparison.OrdinalIgnoreCase) ||
				material.MaterialType.Equals(term, StringComparison.OrdinalIgnoreCase));
		}

		foreach (var material in query.OrderBy(m => m.Code))
		{
			Materials.Add(new MaterialItem(
				material.Id,
				material.Code,
				material.Name,
				material.MaterialType,
				material.Unit,
				material.IsActive,
				material.LastPrice));
		}

		HasMaterials = Materials.Count > 0;
	}

	private async Task OpenCardAsync(MaterialItem? material)
	{
		if (material is null)
		{
			return;
		}

		var viewModel = new MaterialCardPageViewModel(_materialsService, _suppliersService);
		viewModel.Initialize(material.Id, material.Code, material.Name);
		var page = new MaterialCardPage(viewModel);

		await Shell.Current.Navigation.PushAsync(page);
	}
}
