using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Services.Common;
using MyFactory.MauiClient.Services.Departments;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.ProductTypes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MyFactory.MauiClient.ViewModels.Products;

[QueryProperty(nameof(ProductIdParameter), "ProductId")]
public partial class ProductEditPageViewModel : ObservableObject
{
    #region SERVICES AND FIELDS
    private readonly IProductsService _productsService;
    private readonly IMaterialsService _materialsService;
    private readonly IProductTypesService _productTypesService;
    private readonly IDepartmentsService _departmentsService;
    private readonly IPopupService _popupService;
    #endregion

    #region OBSERVABLE PROPERTIES
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
    private string status = ProductStatus.Active.ProductRusStatus();

    [ObservableProperty]
    private string? version;

    [ObservableProperty]
    private string? planPerHour;

    [ObservableProperty]
    private string? productType;

    [ObservableProperty]
    private ImageItemViewModel? selectedImage;

    [ObservableProperty]
    private bool isSelected = false;

    [ObservableProperty]
    private ProductTypeOptionViewModel? selectedProductTypeOption;

    [ObservableProperty]
    private string pageTitle = string.Empty;

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
    #endregion

    #region OBSERVABLE COLLECTIONS
    public ObservableCollection<MaterialOptionViewModel> MaterialOptions { get; } = new();
    public ObservableCollection<DepartmentOptionViewModel> DepartmentOptions { get; } = new();
    public ObservableCollection<BomEditItemViewModel> EditableBom { get; } = new();
    public ObservableCollection<ProductionCostEditViewModel> EditableProductionCosts { get; } = new();
    public ObservableCollection<ProductTypeOptionViewModel> ProductTypeOptions { get; } = new();
    public ObservableCollection<ImageItemViewModel> Images { get; } = [];
    ObservableCollection<ImageItemViewModel> RemovedImages { get; } = [];
    #endregion

    #region COLLECTIONS
    public IReadOnlyCollection<string> StatusOptions { get; } = [.. Enum.GetValues<ProductStatus>().Select(s => s.ProductRusStatus())];
    private HashSet<Guid> _originalMaterialIds { get; } = new();
    private Dictionary<Guid, decimal> _originalMaterialQuantities { get; } = new();
    #endregion


    #region CONSTRUCTOR
    public ProductEditPageViewModel(IProductsService productsService, IMaterialsService materialsService, IDepartmentsService departmentsService, IProductTypesService productTypesService, IPopupService popupService)
    {
        _productsService = productsService;
        _materialsService = materialsService;
        _productTypesService = productTypesService;
        _departmentsService = departmentsService;

        _popupService = popupService;

        SubscribeSelectionCollections();
    }
    #endregion

    #region ON CHANGING/CHANGED
    partial void OnSelectedImageChanged(ImageItemViewModel? value)
    {
        IsSelected = value is null ? false : true;
    }

    partial void OnSelectedProductTypeOptionChanged(ProductTypeOptionViewModel? value)
    {
        ProductType = value?.Type;
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
        if (!IsBusy)
            _ = LoadAsync();
    }

    partial void OnProductIdParameterChanged(string? value)
    {
        ProductId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region LOADING
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        MaterialOptions.Clear();
        DepartmentOptions.Clear();
        EditableBom.Clear();
        EditableProductionCosts.Clear();
        _originalMaterialIds.Clear();
        _originalMaterialQuantities.Clear();
        ProductTypeOptions.Clear();

        // ProductTypes

        var productTypes = await _productTypesService.GetListAsync();
        foreach (var pt in productTypes ?? [])
            ProductTypeOptions.Add(new ProductTypeOptionViewModel(pt.Id, pt.Type));

        // Materials

        var materials = (await _materialsService.GetListAsync())?.Items.OrderBy(m => m.Name).ToList();
        foreach (var m in materials ?? [])
            MaterialOptions.Add(new MaterialOptionViewModel(m.Id, m.Name));

        // Title and selected type of product
        if (ProductId is null)
        {
            PageTitle = "Создание товара";

            if (ProductTypeOptions.Count > 0)
                SelectedProductTypeOption = ProductTypeOptions.First();
            return;
        }
        else if (ProductId is not null)
        {
            PageTitle = "Редактирование товара";
            if (ProductTypeOptions.Count > 0)
            {
                var productTypeDto = await _productTypesService.GetByProductIdAsync(ProductId.Value);
                SelectedProductTypeOption = ProductTypeOptions.FirstOrDefault(pt => pt.Id == productTypeDto?.Id)
                    ?? ProductTypeOptions.FirstOrDefault(pt => pt.Type == productTypeDto?.Type)
                    ?? ProductTypeOptions.FirstOrDefault();
            }
        }

        // Details of product

        ProductDetailsResponse? details = null;

        if (ProductId is not null)
        {
            details = await _productsService.GetDetailsAsync(ProductId.Value);
        }

        if (details is null)
            return;

        Sku = details.Sku;
        Name = details.Name;
        Description = details.Description;
        Status = details.Status.ProductRusStatus();
        Version = details.Version?.ToString();
        PlanPerHour = details.PlanPerHour?.ToString();

        // BOM

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
                Unit = bom.Unit,
                IsNew = false
            });

            _originalMaterialQuantities[bom.MaterialId] = bom.QtyPerUnit;
        }

        // Production Costs

        var departments = await _departmentsService.GetListAsync();

        foreach (var dept in departments ?? [])
        {
            var option = new DepartmentOptionViewModel(dept.Id, dept.Name);
            DepartmentOptions.Add(option);
        }

        foreach (var cost in details.ProductionCosts)
        {
            var option = new DepartmentOptionViewModel(cost.DepartmentId, cost.DepartmentName);

            EditableProductionCosts.Add(new ProductionCostEditViewModel(option)
            {
                Cutting = cost.CutCost,
                Sewing = cost.SewingCost,
                Packaging = cost.PackCost,
                Other = cost.Expenses
            });
        }

        // Images

        await LoadImagesAsync();

        SelectedImage = null;
    });
    #endregion

    #region BOM HANDLING
    #region SELECT MATERIAL FOR BOM
    [RelayCommand]
    private void SelectMaterialForBom(object? parameter)
    {
        if (parameter is string materialName && EditableBom.Count > 0)
        {
            var lastItem = EditableBom.Last();
            var material = MaterialOptions.FirstOrDefault(m => m.Name == materialName);
            if (material != null)
            {
                lastItem.Material = material;
            }
        }
    }
    #endregion

    #region ADD MATERIAL
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
    #endregion

    #region REMOVE MATERIALS
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
    #endregion
    #endregion

    #region DEPARTMENT COSTS HANDLING
    #region SELECT DEPARTMENT FOR COST
    [RelayCommand]
    private void SelectDepartmentForCosts(object? parameter)
    {
        if (parameter is string departmentName && EditableProductionCosts.Count > 0)
        {
            var lastItem = EditableProductionCosts.Last();
            var department = DepartmentOptions.FirstOrDefault(d => d.Name == departmentName);
            if (department != null)
            {
                lastItem.Department = department;
            }
        }
    }
    #endregion

    #region ADD DEPARTMENT
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
    #endregion

    #region REMOVE DEPARTMENT
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
    #endregion
    #endregion

    #region SAVING
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        var trimmedName = Name?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Необходимо добавить название продукта.", "OK");
            return;
        }
        decimal? plan = string.IsNullOrWhiteSpace(PlanPerHour) ? null : PlanPerHour.StringToDecimal();

        decimal? ver = string.IsNullOrWhiteSpace(Version) ? null : Version.StringToDecimal();

        var selectedTypeId = SelectedProductTypeOption?.Id;

        var existingProducts = (await _productsService.GetListAsync())?.Items;
        var currentProductId = ProductId;

        var duplicateExists = existingProducts?.Any(p =>
            (currentProductId is null || p.Id != currentProductId) &&
            string.Equals(p.Name.Trim(), trimmedName, StringComparison.OrdinalIgnoreCase) &&
            p.ProductTypeId == selectedTypeId) is true;

        if (duplicateExists)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Товар с этим типом и наименованием уже существует", "OK");
            return;
        }

        if (ProductId is null)
        {
            var createResponse = await _productsService.CreateAsync(new CreateProductRequest(
                Name: trimmedName!.CapitalizeFirst(),
                ProductTypeId: selectedTypeId,
                Status: Status.ProductStatusFromRus(),
                PlanPerHour: plan,
                Description: Description,
                Version: ver))
                ?? throw new InvalidOperationException("Couldn't get a response from the server.");

            ProductId = createResponse.Id;
        }
        else
        {
            await _productsService.UpdateAsync(ProductId.Value, new UpdateProductRequest(
                Name: trimmedName!,
                ProductTypeId: selectedTypeId,
                PlanPerHour: plan,
                Status: Status.ProductStatusFromRus(),
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

            var costs = EditableProductionCosts
                .Where(c => c.Department is not null)
                .Select(c => new ProductDepartmentCostRequest(
                    c.Department!.Id,
                    c.Cutting,
                    c.Sewing,
                    c.Packaging,
                    c.Other))
                .ToList();

            await _productsService.SetProductionCostsAsync(
                ProductId.Value,
                new SetProductProductionCostsRequest(costs));
        }

        await SaveImagesAsync();

        await Shell.Current.DisplayAlertAsync("Успешно!", "Товар сохранен.", "OK");
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(ProductsListPage));
    });
    #endregion

    #region CANCELLING
    [RelayCommand]
    private async Task CancelAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(ProductsListPage));
    });
    #endregion

    #region IMAGE HANDLING
    [RelayCommand]
    #region ADD IMAGE
    private async Task AddImageAsync() => await RunSafeActionAsync(async () =>
    {
        var imagesList = await FilePicker.PickMultipleAsync(PickOptions.Images);
        if (imagesList is null)
            return;

        foreach (var image in imagesList)
        {
            if (image is null)
                continue;

            await using var pickedStream = await image.OpenReadAsync();
            var compressed = await pickedStream.CompressPhotoAsync(1920, 1920, 75);

            Images.Add(new ImageItemViewModel(
                id: null,
                fileName: Path.ChangeExtension(image.FileName, ".jpg"),
                contentType: "image/jpeg",
                content: compressed));
        }
    });
    #endregion

    #region SAVE IMAGES
    private async Task SaveImagesAsync()
    {
        if (ProductId is null)
            return;
        foreach (var image in Images)
        {
            if (image.Id is not null || image.Content is null)
                continue;

            await _productsService.UploadImageAsync(
            productId: ProductId.Value,
            content: new MemoryStream(image.Content),
            fileName: image.FileName,
            contentType: image.ContentType ?? "application/octet-stream");
        }

        if (RemovedImages.Count == 0)
            return;

        foreach (var image in RemovedImages)
        {
            if (image.Id is null)
                continue;

            Guid imageId = image.Id.Value;
            await _productsService.DeleteImageAsync(imageId);
        }

    }
    #endregion

    #region DELETE IMAGE
    [RelayCommand]
    private async Task DeleteImageAsync(ImageItemViewModel image) => await RunSafeActionAsync(async () =>
    {
        if (image is null)
            return;

        Images.Remove(image);

        RemovedImages.Add(image);
    });
    #endregion

    #region LOAD IMAGES
    private async Task LoadImagesAsync() => await RunSafeActionAsync(async () =>
    {
        if (ProductId is null)
            return;

        Images.Clear();
        var imagesList = await _productsService.GetImagesAsync(ProductId.Value);
        if (imagesList is null)
            return;

        foreach (var i in imagesList)
        {
            var bytes = i.Content ?? await _productsService.GetImageContentAsync(i.Id) ?? [];

            Images.Add(new ImageItemViewModel(
                id: i.Id,
                fileName: i.FileName,
                contentType: i.ContentType,
                content: bytes));
        }
    });
    #endregion
    #endregion

    #region POPUP HANDLING
    [RelayCommand]
    public async Task MaterialEntryFocusedAsync(BomEditItemViewModel item) => await RunSafeActionAsync(async () =>
    {
        Guid id = await _popupService.EntryFocusedAsync<MaterialListItemResponse>(_materialsService);

        if (id == Guid.Empty) return;

        var selectedMaterial = await _materialsService.GetDetailsAsync(id);

        if (selectedMaterial is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Материал не найден", "OK");
            return;
        }

        item.Material = new MaterialOptionViewModel(selectedMaterial.Id, selectedMaterial.Name);
    });
    #endregion

    #region RUN SAFE ACTION
    public async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region INNER CLASSES
    public sealed record MaterialOptionViewModel(Guid Id, string Name);

    public sealed record DepartmentOptionViewModel(Guid Id, string Name);

    public partial class BomEditItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private MaterialOptionViewModel? material;

        [ObservableProperty]
        private decimal quantity;

        [ObservableProperty]
        private string? unit;

        public bool IsNew { get; set; } = true;
    }

    public sealed record ProductTypeOptionViewModel(Guid Id, string Type);

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

    public sealed class ImageItemViewModel
    {
        public Guid? Id { get; }
        public string FileName { get; }
        public string? ContentType { get; }
        public byte[]? Content { get; }
        public ImageSource? Source => Content is { Length: > 0 }
                ? ImageSource.FromStream(() => new MemoryStream(Content))
                : null;

        public ImageItemViewModel(Guid? id, string fileName, string? contentType, byte[]? content)
        {
            Id = id;
            FileName = fileName;
            ContentType = contentType;
            Content = content;
        }
    }
    #endregion
}

