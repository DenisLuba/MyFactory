using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.Materials;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

public partial class MaterialsListPageViewModel : ObservableObject
{
    private readonly IMaterialsService _materialsService;
    private List<MaterialItemViewModel> _allMaterials = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string? materialTypeSearch;

    [ObservableProperty]
    private string? materialNameSearch;

    public ObservableCollection<MaterialItemViewModel> FilteredMaterials { get; } = new();

    public MaterialsListPageViewModel(IMaterialsService materialsService)
    {
        _materialsService = materialsService;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var materials = await _materialsService.GetListAsync(isActive: true);
            _allMaterials = materials?.Select(m => new MaterialItemViewModel(m)).ToList() ?? new();
            ApplyFilters();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync("MaterialDetailsEditPage");
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(MaterialItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync("MaterialDetailsViewPage", new Dictionary<string, object>
        {
            { "MaterialId", item.Id.ToString() }
        });
    }

    [RelayCommand]
    private async Task EditAsync(MaterialItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync("MaterialDetailsEditPage", new Dictionary<string, object>
        {
            { "MaterialId", item.Id.ToString() }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync(MaterialItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", "Вы уверены, что хотите деактивировать материал?", "Да", "Отмена");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            await _materialsService.DeleteAsync(item.Id);

            _allMaterials = _allMaterials.Where(m => m.Id != item.Id).ToList();
            ApplyFilters();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnMaterialTypeSearchChanged(string? value) => ApplyFilters();
    partial void OnMaterialNameSearchChanged(string? value) => ApplyFilters();

    private void ApplyFilters()
    {
        var query = _allMaterials.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(MaterialTypeSearch))
        {
            var term = MaterialTypeSearch.Trim();
            query = query.Where(m => m.MaterialTypeName.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(MaterialNameSearch))
        {
            var term = MaterialNameSearch.Trim();
            query = query.Where(m => m.MaterialName.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        var result = query.ToList();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            FilteredMaterials.Clear();
            foreach (var item in result)
                FilteredMaterials.Add(item);
        });
    }

    public partial class MaterialItemViewModel : ObservableObject
    {
        public Guid Id { get; }
        public string MaterialTypeName { get; }
        public string MaterialName { get; }
        public decimal TotalQty { get; }

        public MaterialItemViewModel(MaterialListItemResponse response)
        {
            Id = response.Id;
            MaterialTypeName = response.MaterialType;
            MaterialName = response.Name;
            TotalQty = response.TotalQty;
        }
    }
}

