using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

[QueryProperty(nameof(MaterialId), "MaterialId")]
public partial class MaterialDetailsEditPageViewModel : ObservableObject
{
    private readonly IMaterialsService _materialsService;
    private readonly ISuppliersService _suppliersService;

    private Guid _materialTypeId = Guid.Empty;
    private Guid _unitId = Guid.Empty;
    private List<SupplierListItemResponse> _supplierOptions = new();

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? color;

    [ObservableProperty]
    private string? selectedMaterialType;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<string> MaterialTypes { get; } = new();
    public ObservableCollection<EditablePurchaseItemViewModel> EditablePurchaseHistory { get; } = new();

    public MaterialDetailsEditPageViewModel(IMaterialsService materialsService, ISuppliersService suppliersService)
    {
        _materialsService = materialsService;
        _suppliersService = suppliersService;
        _ = LoadAsync();
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            EditablePurchaseHistory.Clear();
            MaterialTypes.Clear();

            var suppliers = await _suppliersService.GetListAsync();
            var supplierOptions = suppliers?.ToList() ?? new List<SupplierListItemResponse>();
            _supplierOptions = supplierOptions;

            if (MaterialId is null)
            {
                SelectedMaterialType = null;
                Name = string.Empty;
                Color = string.Empty;
                return;
            }

            var details = await _materialsService.GetDetailsAsync(MaterialId.Value);
            if (details is null)
                return;

            Name = details.Name;
            SelectedMaterialType = details.MaterialType;
            Color = details.Color;
            _materialTypeId = Guid.Empty;
            _unitId = Guid.Empty;

            if (!string.IsNullOrWhiteSpace(details.MaterialType))
            {
                MaterialTypes.Add(details.MaterialType);
            }

            foreach (var history in details.PurchaseHistory.OrderByDescending(p => p.PurchaseDate))
            {
                var vm = new EditablePurchaseItemViewModel(supplierOptions)
                {
                    Supplier = supplierOptions.FirstOrDefault(s => s.Name.Equals(history.SupplierName, StringComparison.OrdinalIgnoreCase)),
                    Qty = history.Qty,
                    UnitPrice = history.UnitPrice,
                    PurchaseDate = history.PurchaseDate
                };
                EditablePurchaseHistory.Add(vm);
            }
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
    private Task AddPurchaseRowAsync()
    {
        EditablePurchaseHistory.Add(new EditablePurchaseItemViewModel(_supplierOptions)
        {
            PurchaseDate = DateTime.Now
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task EditPurchaseAsync(EditablePurchaseItemViewModel? item)
    {
        if (item is null)
            return;

        var qtyString = await Shell.Current.DisplayPromptAsync("Количество", "Введите количество", initialValue: item.Qty.ToString());
        if (decimal.TryParse(qtyString, out var qty))
        {
            item.Qty = qty;
        }

        var priceString = await Shell.Current.DisplayPromptAsync("Цена", "Введите цену", initialValue: item.UnitPrice.ToString());
        if (decimal.TryParse(priceString, out var price))
        {
            item.UnitPrice = price;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (MaterialId is null)
            return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите название материала", "OK");
            return;
        }

        if (_materialTypeId == Guid.Empty || _unitId == Guid.Empty)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Недостаточно данных для обновления материала (неизвестны идентификаторы типа или единицы).", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var request = new UpdateMaterialRequest(Name.Trim(), _materialTypeId, _unitId, string.IsNullOrWhiteSpace(Color) ? null : Color.Trim());
            await _materialsService.UpdateAsync(MaterialId.Value, request);
            await Shell.Current.DisplayAlertAsync("Успех", "Материал обновлен", "OK");
            await Shell.Current.GoToAsync("..", true);
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
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public partial class EditablePurchaseItemViewModel : ObservableObject
    {
        public ObservableCollection<SupplierListItemResponse> SupplierOptions { get; } = new();

        [ObservableProperty]
        private SupplierListItemResponse? supplier;

        [ObservableProperty]
        private decimal qty;

        [ObservableProperty]
        private decimal unitPrice;

        [ObservableProperty]
        private DateTime purchaseDate = DateTime.Now;

        public EditablePurchaseItemViewModel()
        {
        }

        public EditablePurchaseItemViewModel(IEnumerable<SupplierListItemResponse> options)
        {
            foreach (var option in options)
            {
                SupplierOptions.Add(option);
            }
            Supplier = SupplierOptions.FirstOrDefault();
        }
    }
}

