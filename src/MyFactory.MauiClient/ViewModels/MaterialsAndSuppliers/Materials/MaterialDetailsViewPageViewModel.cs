using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Services.Materials;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
public partial class MaterialDetailsViewPageViewModel : ObservableObject
{
    private readonly IMaterialsService _materialsService;

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? materialType;

    [ObservableProperty]
    private string? color;

    [ObservableProperty]
    private decimal totalQty;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<WarehouseQtyResponse> Warehouses { get; } = new();
    public ObservableCollection<MaterialPurchaseHistoryItemResponse> PurchaseHistory { get; } = new();

    public MaterialDetailsViewPageViewModel(IMaterialsService materialsService)
    {
        _materialsService = materialsService;
        _ = LoadAsync();
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (MaterialId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Warehouses.Clear();
            PurchaseHistory.Clear();

            var details = await _materialsService.GetDetailsAsync(MaterialId.Value);
            if (details is null)
                return;

            Name = details.Name;
            MaterialType = details.MaterialType;
            Color = details.Color;
            TotalQty = details.TotalQty;

            foreach (var w in details.Warehouses)
                Warehouses.Add(w);

            foreach (var h in details.PurchaseHistory.OrderByDescending(p => p.PurchaseDate))
                PurchaseHistory.Add(h);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditAsync()
    {
        if (MaterialId is null)
            return;

        await Shell.Current.GoToAsync("MaterialDetailsEditPage", new Dictionary<string, object>
        {
            { "MaterialId", MaterialId.Value.ToString() }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (MaterialId is null)
            return;

        var confirm = await Shell.Current.DisplayAlert("Удаление", "Вы уверены, что хотите деактивировать материал?", "Да", "Отмена");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            await _materialsService.DeleteAsync(MaterialId.Value);
            await Shell.Current.DisplayAlert("Успех", "Материал деактивирован", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddPriceAsync()
    {
        if (MaterialId is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderCreatePage", new Dictionary<string, object>
        {
            { "MaterialId", MaterialId.Value.ToString() }
        });
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private async Task OpenSupplierAsync(Guid supplierId)
    {
        await Shell.Current.GoToAsync("SupplierDetailsPage", new Dictionary<string, object>
        {
            { "SupplierId", supplierId.ToString() }
        });
    }
}

