using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Products;

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

    public ProductEditPageViewModel(IProductsService productsService, IMaterialsService materialsService)
    {
        _productsService = productsService;
        _materialsService = materialsService;
        SubscribeSelectionCollections();
        _ = LoadAsync();
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

            Name = details.Name;
            PlanPerHour = details.PlanPerHour?.ToString();

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
            await Shell.Current.DisplayAlertAsync("������", "������� ��������", "OK");
            return;
        }

        int? plan = null;
        if (!string.IsNullOrWhiteSpace(PlanPerHour) && int.TryParse(PlanPerHour, out var parsed))
        {
            plan = parsed;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (ProductId is null)
            {
                var createResponse = await _productsService.CreateAsync(new CreateProductRequest(
                    Sku: $"PRD-{Guid.NewGuid():N}"[..8],
                    Name: Name.Trim(),
                    Status: ProductStatus.Active,
                    PlanPerHour: plan));

                if (createResponse is null)
                    throw new InvalidOperationException("�� ������� ������� �����");

                ProductId = createResponse.Id;
            }
            else
            {
                await _productsService.UpdateAsync(ProductId.Value, new UpdateProductRequest(Name.Trim(), plan, ProductStatus.Active));
            }

            if (ProductId is not null)
            {
                var newBomItems = EditableBom.Where(b => b.IsNew && b.Material is not null);
                foreach (var bom in newBomItems)
                {
                    await _productsService.AddMaterialAsync(ProductId.Value, new AddProductMaterialRequest(bom.Material!.Id, bom.Quantity));
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

            await Shell.Current.DisplayAlertAsync("�����", "���������", "OK");
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("������", ex.Message, "OK");
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

