using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Models.Units;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.MaterialTypes;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.Services.Units;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Materials;

[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
public partial class MaterialDetailsEditPageViewModel : ObservableObject
{
    private readonly IMaterialsService _materialsService;
    private readonly ISuppliersService _suppliersService;
    private readonly IMaterialTypesService _materialTypesService;
    private readonly IUnitsService _unitsService;

    private Guid _materialTypeId = Guid.Empty;
    private Guid _unitId = Guid.Empty;
    private List<SupplierListItemResponse> _supplierOptions = new();
    private Dictionary<string, Guid> _materialTypeLookup = new(StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, Guid> _unitLookup = new(StringComparer.OrdinalIgnoreCase);

    [ObservableProperty]
    private Guid? materialId;

    [ObservableProperty]
    private string? materialIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? color;

    [ObservableProperty]
    private string? selectedMaterialType;

    [ObservableProperty]
    private string? selectedUnit;

    [ObservableProperty]
    private string? _unitCode;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<string> MaterialTypes { get; } = new();
    public ObservableCollection<string> Units { get; } = new();
    public ObservableCollection<EditablePurchaseItemViewModel> EditablePurchaseHistory { get; } = new();

    public MaterialDetailsEditPageViewModel(
        IMaterialsService materialsService,
        ISuppliersService suppliersService,
        IMaterialTypesService materialTypesService,
        IUnitsService unitsService)
    {
        _materialsService = materialsService;
        _suppliersService = suppliersService;
        _materialTypesService = materialTypesService;
        _unitsService = unitsService;
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

    partial void OnSelectedUnitChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _unitId = Guid.Empty;
            UnitCode = null;
            return;
        }

        UnitCode = value;
        if (_unitLookup.TryGetValue(value, out var id))
        {
            _unitId = id;
        }
    }

    partial void OnSelectedMaterialTypeChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            _materialTypeId = Guid.Empty;
            return;
        }

        if (_materialTypeLookup.TryGetValue(value, out var id))
        {
            _materialTypeId = id;
        }
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
            Units.Clear();
            _materialTypeLookup.Clear();
            _unitLookup.Clear();

            var units = await _unitsService.GetListAsync();
            foreach (var unit in units ?? Array.Empty<UnitResponse>())
            {
                Units.Add(unit.Code);
                _unitLookup[unit.Code] = unit.Id;
            }

            var materialTypes = await _materialTypesService.GetListAsync();
            foreach (var type in materialTypes)
            {
                _materialTypeLookup[type.Name] = type.Id;
                MaterialTypes.Add(type.Name);
            }

            var suppliers = await _suppliersService.GetListAsync();
            var supplierOptions = suppliers?.ToList() ?? new List<SupplierListItemResponse>();
            _supplierOptions = supplierOptions;

            if (MaterialId is null)
            {
                SelectedMaterialType = null;
                SelectedUnit = Units.FirstOrDefault();
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
            SelectedUnit = Units.FirstOrDefault(u => u.Equals(details.UnitCode, StringComparison.OrdinalIgnoreCase));
            _materialTypeId = _materialTypeLookup.GetValueOrDefault(details.MaterialType, Guid.Empty);
            _unitId = SelectedUnit is not null && _unitLookup.TryGetValue(SelectedUnit, out var uId) ? uId : Guid.Empty;
            UnitCode = SelectedUnit;

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
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Укажите название материала", "OK");
            return;
        }

        if (_materialTypeId == Guid.Empty || _unitId == Guid.Empty)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Недостаточно данных для сохранения материала (неизвестны идентификаторы типа или единицы).", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var request = new UpdateMaterialRequest(Name.Trim(), _materialTypeId, _unitId, string.IsNullOrWhiteSpace(Color) ? null : Color.Trim());

            if (MaterialId is null)
            {
                var createRequest = new CreateMaterialRequest(request.Name, request.MaterialTypeId, request.UnitId, request.Color);
                var newId = await _materialsService.CreateAsync(createRequest);
                MaterialId = newId;
                await Shell.Current.DisplayAlertAsync("Успех", "Материал создан", "OK");
            }
            else
            {
                await _materialsService.UpdateAsync(MaterialId.Value, request);
                await Shell.Current.DisplayAlertAsync("Успех", "Материал обновлен", "OK");
            }

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

