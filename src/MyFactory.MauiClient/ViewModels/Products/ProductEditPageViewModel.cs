using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Common;

namespace MyFactory.MauiClient.ViewModels.Products;

[QueryProperty(nameof(ProductIdParameter), "ProductId")]
public partial class ProductEditPageViewModel : ObservableObject
{
    private readonly IProductsService _productsService;
    private readonly IMaterialsService _materialsService;

    [ObservableProperty]
    private Guid? productId;

    [ObservableProperty]
    private string? productIdParameter;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? sku;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string status = ProductStatus.Active.RusStatus();

    [ObservableProperty]
    private string? version;

    [ObservableProperty]
    private string? planPerHour;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private bool areMaterialsSelected;

    [ObservableProperty]
    private bool areDepartmentsSelected;

    [ObservableProperty]
    private ObservableCollection<object?> selectedMaterials = [];

    [ObservableProperty]
    private ObservableCollection<object?> selectedDepartments = [];

    public ObservableCollection<MaterialOptionViewModel> MaterialOptions { get; } = new();
    public ObservableCollection<DepartmentOptionViewModel> DepartmentOptions { get; } = new();
    public ObservableCollection<BomEditItemViewModel> EditableBom { get; } = new();
    public ObservableCollection<ProductionCostEditViewModel> EditableProductionCosts { get; } = new();
    public IReadOnlyCollection<string> StatusOptions { get; } = [.. Enum.GetValues<ProductStatus>().Select(s => s.RusStatus())];

    private HashSet<Guid> _originalMaterialIds { get; } = new();
    private Dictionary<Guid, decimal> _originalMaterialQuantities { get; } = new();

    public ProductEditPageViewModel(IProductsService productsService, IMaterialsService materialsService)
    {
        _productsService = productsService;
        _materialsService = materialsService;
        SubscribeSelectionCollections();
    }

    private void SubscribeSelectionCollections()
    {
        SelectedMaterials.CollectionChanged += SelectedMaterialsCollectionChanged;
        SelectedDepartments.CollectionChanged += SelectedDepartmentsCollectionChanged;
        AreMaterialsSelected = SelectedMaterials.Count > 0;
        AreDepartmentsSelected = SelectedDepartments.Count > 0;
    }

    partial void OnSelectedMaterialsChanging(ObservableCollection<object?> value)
    {
        // отписываемся от старой коллекции
        if (SelectedMaterials is not null)
            SelectedMaterials.CollectionChanged -= SelectedMaterialsCollectionChanged;
    }

    partial void OnSelectedMaterialsChanged(ObservableCollection<object?> value)
    {
        // подписываемся на новую коллекцию
        if (value is not null)
            value.CollectionChanged += SelectedMaterialsCollectionChanged;
        AreMaterialsSelected = value?.Count > 0;
    }

    partial void OnSelectedDepartmentsChanging(ObservableCollection<object?> value)
    {
        // отписываемся от старой коллекции
        if (SelectedDepartments is not null)
            SelectedDepartments.CollectionChanged -= SelectedDepartmentsCollectionChanged;
    }

    partial void OnSelectedDepartmentsChanged(ObservableCollection<object?> value)
    {
        // подписываемся на новую коллекцию
        if (value is not null)
            value.CollectionChanged += SelectedDepartmentsCollectionChanged;
        AreDepartmentsSelected = value?.Count > 0;
    }

    private void SelectedMaterialsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        AreMaterialsSelected = SelectedMaterials.Count > 0;
    }

    private void SelectedDepartmentsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        AreDepartmentsSelected = SelectedDepartments.Count > 0;
    }

    partial void OnProductIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnProductIdParameterChanged(string? value)
    {
        ProductId = Guid.TryParse(value, out var id) ? id : null;
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

            MaterialOptions.Clear();
            DepartmentOptions.Clear();
            EditableBom.Clear();
            EditableProductionCosts.Clear();
            _originalMaterialIds.Clear();
            _originalMaterialQuantities.Clear();

            var materials = await _materialsService.GetListAsync();
            foreach (var m in materials ?? Array.Empty<MaterialListItemResponse>())
                MaterialOptions.Add(new MaterialOptionViewModel(m.Id, m.Name));

            if (ProductId is null)
            {
                return;
            }

            var details = await _productsService.GetDetailsAsync(ProductId.Value);
            if (details is null)
                return;

            Sku = details.Sku;
            Name = details.Name;
            Description = details.Description;
            Status = details.Status.RusStatus();
            Version = details.Version?.ToString();
            PlanPerHour = details.PlanPerHour?.ToString();

            foreach (var materialId in details.Bom.Select(b => b.MaterialId))
                _originalMaterialIds.Add(materialId);

            foreach (var bom in details.Bom)
            {
                var option = MaterialOptions.FirstOrDefault(o => o.Id == bom.MaterialId) ?? new MaterialOptionViewModel(bom.MaterialId, bom.MaterialName);
                if (!MaterialOptions.Contains(option))
                    MaterialOptions.Add(option);

                EditableBom.Add(new BomEditItemViewModel
                {
                    Material = option,
                    Quantity = bom.QtyPerUnit,
                    IsNew = false
                });

                _originalMaterialQuantities[bom.MaterialId] = bom.QtyPerUnit;
            }

            foreach (var cost in details.ProductionCosts)
            {
                var option = new DepartmentOptionViewModel(cost.DepartmentId, cost.DepartmentName);
                DepartmentOptions.Add(option);

                EditableProductionCosts.Add(new ProductionCostEditViewModel(option)
                {
                    Cutting = cost.CutCost,
                    Sewing = cost.SewingCost,
                    Packaging = cost.PackCost,
                    Other = cost.Expenses
                });
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
    private Task AddMaterialAsync()
    {
        var option = MaterialOptions.FirstOrDefault();
        EditableBom.Add(new BomEditItemViewModel
        {
            Material = option,
            Quantity = 1,
            IsNew = true
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private void RemoveMaterials()
    {
        foreach (var item in SelectedMaterials.Cast<BomEditItemViewModel>().ToList())
        {
            if (EditableBom.Contains(item))
            {
                EditableBom.Remove(item);
            }
        }
    }

    [RelayCommand]
    private Task AddDepartmentAsync()
    {
        var option = DepartmentOptions.FirstOrDefault();
        EditableProductionCosts.Add(new ProductionCostEditViewModel(option)
        {
            Cutting = 0,
            Sewing = 0,
            Packaging = 0,
            Other = 0
        });
        return Task.CompletedTask;
    }

    [RelayCommand]
    private void RemoveDepartments()
    {
        foreach (var item in SelectedDepartments.Cast<ProductionCostEditViewModel>().ToList())
        {
            if (EditableProductionCosts.Contains(item))
            {
                EditableProductionCosts.Remove(item);
            }
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Необходимо добавить название продукта.", "OK");
            return;
        }

        try
        {

            decimal? plan = PlanPerHour?.StringToDecimal();

            decimal? ver = Version?.StringToDecimal();

            IsBusy = true;
            ErrorMessage = null;

            if (ProductId is null)
            {
                var createResponse = await _productsService.CreateAsync(new CreateProductRequest(
                    Name: Name.Trim().CapitalizeFirst(),
                    Status: Status.StatusFromRus(),
                    PlanPerHour: plan,
                    Description: Description,
                    Version: ver));

                if (createResponse is null)
                    throw new InvalidOperationException("Couldn't get a response from the server.");

                ProductId = createResponse.Id;
            }
            else
            {
                await _productsService.UpdateAsync(ProductId.Value, new UpdateProductRequest(
                    Name: Name.Trim(),
                    PlanPerHour: plan,
                    Status: Status.StatusFromRus(),
                    Description: Description,
                    Version: ver));
            }

            if (ProductId is not null)
            {
                // fail fast if there are duplicate materials in the BOM
                var duplicateGroup = EditableBom
                    .Where(b => b.Material is not null)
                    .GroupBy(b => b.Material!.Id)
                    .FirstOrDefault(g => g.Count() > 1);

                if (duplicateGroup is not null)
                {
                    await Shell.Current.DisplayAlertAsync("Внимание!", "Материал уже добавлен", "OK");
                    return;
                }

                var currentMaterials = EditableBom
                    .Where(b => b.Material is not null)
                    .ToList();

                var currentMaterialIds = currentMaterials
                    .Select(b => b.Material!.Id)
                    .ToHashSet();

                var removedMaterialIds = _originalMaterialIds.Except(currentMaterialIds);
                foreach (var id in removedMaterialIds)
                    await _productsService.RemoveMaterialAsync(ProductId.Value, id);

                var changedMaterialIds = currentMaterials
                    .Where(b => _originalMaterialQuantities.TryGetValue(b.Material!.Id, out var oldQty) && b.Quantity != oldQty)
                    .Select(b => b.Material!.Id)
                    .ToHashSet();

                foreach (var id in changedMaterialIds)
                    await _productsService.RemoveMaterialAsync(ProductId.Value, id);

                // add new materials and re-add changed ones with updated qty
                var materialsToAdd = currentMaterials
                    .Where(b =>
                        !_originalMaterialQuantities.ContainsKey(b.Material!.Id) // brand new
                        || changedMaterialIds.Contains(b.Material!.Id))         // changed qty
                    .GroupBy(b => b.Material!.Id)
                    .Select(g => g.First());

                foreach (var bom in materialsToAdd)
                    await _productsService.AddMaterialAsync(ProductId.Value, new AddProductMaterialRequest(bom.Material!.Id, bom.Quantity));

                _originalMaterialIds.Clear();
                _originalMaterialQuantities.Clear();
                foreach (var mat in currentMaterials)
                {
                    _originalMaterialIds.Add(mat.Material!.Id);
                    _originalMaterialQuantities[mat.Material!.Id] = mat.Quantity;
                }

                if (EditableProductionCosts.Any())
                {
                    var costs = EditableProductionCosts
                        .Where(c => c.Department is not null)
                        .Select(c => new ProductDepartmentCostRequest(
                            c.Department!.Id,
                            c.Cutting,
                            c.Sewing,
                            c.Packaging,
                            c.Other))
                        .ToList();

                    if (costs.Count > 0)
                        await _productsService.SetProductionCostsAsync(ProductId.Value, new SetProductProductionCostsRequest(costs));
                }
            }

            await Shell.Current.DisplayAlertAsync("Успешно!", "Товар сохранен.", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
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

    public sealed record MaterialOptionViewModel(Guid Id, string Name);

    public sealed record DepartmentOptionViewModel(Guid Id, string Name);

    public partial class BomEditItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private MaterialOptionViewModel? material;

        [ObservableProperty]
        private decimal quantity;

        public bool IsNew { get; set; } = true;
    }

    public partial class ProductionCostEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private DepartmentOptionViewModel? department;

        public ProductionCostEditViewModel(DepartmentOptionViewModel? department)
        {
            Department = department;
        }

        [ObservableProperty]
        private decimal cutting;

        [ObservableProperty]
        private decimal sewing;

        [ObservableProperty]
        private decimal packaging;

        [ObservableProperty]
        private decimal other;

        public decimal Total => Cutting + Sewing + Packaging + Other;

        partial void OnCuttingChanged(decimal value) => OnPropertyChanged(nameof(Total));
        partial void OnSewingChanged(decimal value) => OnPropertyChanged(nameof(Total));
        partial void OnPackagingChanged(decimal value) => OnPropertyChanged(nameof(Total));
        partial void OnOtherChanged(decimal value) => OnPropertyChanged(nameof(Total));
    }
}

