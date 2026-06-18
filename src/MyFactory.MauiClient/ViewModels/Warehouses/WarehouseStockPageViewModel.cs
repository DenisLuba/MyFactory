using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Materials;
using MyFactory.MauiClient.Pages.Products;
using MyFactory.MauiClient.Pages.Warehouses;
using MyFactory.MauiClient.Services.Common;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.Warehouses;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.ViewModels.Warehouses;

[QueryProperty(nameof(WarehouseIdParameter), "WarehouseId")]
[QueryProperty(nameof(WarehouseName), "WarehouseName")]
public partial class WarehouseStockPageViewModel(
    IWarehousesService warehousesService,
    IMaterialsService materialsService,
    IProductsService productsService,
    IPopupService popupService) : ObservableObject
{
    #region OBSERVABLE PROPERTIES
    [ObservableProperty]
    private WarehouseType warehouseType;

    [ObservableProperty]
    private Guid? warehouseId;

    [ObservableProperty]
    private string? warehouseIdParameter;

    [ObservableProperty]
    private string? warehouseName;

    [ObservableProperty]
    private string? productOrMaterial;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private decimal qtyPerPackage;

    [ObservableProperty]
    private decimal? packageCount;

    [ObservableProperty]
    private decimal totalQty;

    [ObservableProperty]
    private string? units;

    [ObservableProperty]
    private bool isEditMode = false;

    [ObservableProperty]
    private bool isViewMode = true;

    [ObservableProperty]
    private bool isEditQtyMode = false;

    [ObservableProperty]
    private bool isViewQtyMode = true;

    [ObservableProperty]
    private bool isEditQtyButtonVisible; // редактировать

    [ObservableProperty]
    private bool isSaveQtyChangesButtonVisible; // сохранить изменения

    [ObservableProperty]
    private bool isAddItemButtonVisible; // +

    [ObservableProperty]
    private bool isSaveAddedItemButtonVisible; // сохранить

    [ObservableProperty]
    private StockItemViewModel? item;
    #endregion

    #region COLLECTIONS
    public ICollection<StockItemViewModel> ProductItems { get; } = [];

    public ICollection<StockItemViewModel> MaterialItems { get; } = [];

    public ICollection<StockItemViewModel> RemovedItems { get; } = [];

    public ICollection<StockItemViewModel> СhangedItems { get; } = [];
    private readonly HashSet<Guid> _changedItemIds = []; // для отслеживания измененных элементов и предотвращения дублирования в СhangedItems

    private void MarkChangedItem(StockItemViewModel item)
    {
        if (_changedItemIds.Add(item.Id)) // так быстрее (O(1)), чем проверять наличие элемента в СhangedItems (O(n))
        {
            СhangedItems.Add(item);
        }
    }
    #endregion

    #region OBSERVABLE COLLECTIONS
    public ObservableCollection<StockItemViewModel> Items { get; } = [];

    public ObservableCollection<StockItemViewModel> StockItems { get; } = [];

    #endregion
    #region CONSTRUCTOR
    #endregion

    #region ON ITEM CHANGED
    partial void OnQtyPerPackageChanged(decimal value)
    {
        TotalQty = value * (PackageCount ?? 1);
    }

    partial void OnPackageCountChanged(decimal? value)
    {
        TotalQty = (value ?? 1) * QtyPerPackage;
    }

    partial void OnItemChanged(StockItemViewModel? value)
    {
        UpdateUnits();
    }

    partial void OnIsEditModeChanged(bool value) // +
    {
        IsViewMode = !IsEditMode;

        if (value)
        {
            IsEditQtyMode = false;

            IsSaveAddedItemButtonVisible = true;

            IsAddItemButtonVisible = false;
            IsSaveQtyChangesButtonVisible = false;
            IsEditQtyButtonVisible = false;
        }
        else if (!IsEditQtyMode)
        {
            IsEditQtyButtonVisible = true;
            IsAddItemButtonVisible = true;

            IsSaveQtyChangesButtonVisible = false;
            IsSaveAddedItemButtonVisible = false;
        }
    }

    partial void OnIsEditQtyModeChanged(bool value) // редактировать
    {
        IsViewQtyMode = !IsEditQtyMode;

        if (value)
        {
            IsEditMode = false;

            IsSaveQtyChangesButtonVisible = true;

            IsEditQtyButtonVisible = false;
            IsSaveAddedItemButtonVisible = false;
            IsAddItemButtonVisible = false;
        }
        else if (!IsEditMode)
        {
            IsEditQtyButtonVisible = true;
            IsAddItemButtonVisible = true;

            IsSaveQtyChangesButtonVisible = false;
            IsSaveAddedItemButtonVisible = false;
        }
    }

    partial void OnWarehouseIdChanged(Guid? value)
    {
        _ = RunSafeActionAsync(LoadAsync);
    }

    partial void OnWarehouseIdParameterChanged(string? value)
    {
        WarehouseId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region LOAD 
    [RelayCommand]
    public async Task LoadAsync()
    {
        if (WarehouseId is null)
            return;

        IsAddItemButtonVisible = true;
        IsEditQtyButtonVisible = true;

        StockItems.Clear();

        var info = await warehousesService.GetInfoAsync(WarehouseId.Value);
        if (info is not null)
        {
            WarehouseName = info.Name;
            WarehouseType = info.Type;
        }

        ProductOrMaterial = WarehouseType is WarehouseType.FinishedGoods
            ? "Товар"
            : "Материал";

        var items = await warehousesService.GetStockAsync(WarehouseId.Value);
        foreach (var item in items ?? Enumerable.Empty<WarehouseStockItemResponse>())
        {
            StockItems.Add(new StockItemViewModel(item, WarehouseType, MarkChangedItem));
        }
    }
    #endregion

    #region BACK
    [RelayCommand]
    private async Task BackAsync() => await RunSafeActionAsync(async () =>
    {
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(WarehousesListPage));
    });
    #endregion

    #region ADD ITEM
    [RelayCommand]
    private async Task AddItemAsync() => await RunSafeActionAsync(async () =>
    {
        var isAgreed = await Shell.Current.DisplayAlertAsync(
            "Добавить новый элемент на склад?",
            "Вы уверены, что хотите добавить новый элемент на склад?",
            "Да",
            "Нет");
        if (!isAgreed)
            return;

        await EnsureItemsForWarehouseTypeAsync();
        IsEditMode = true;
        PackageCount = null;
    });
    #endregion

    #region SAVE NEW ITEM
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        try
        {
            if (Item is null)
            {
                await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите элемент для добавления.", "Ок");
                return;
            }

            if (QtyPerPackage < 0)
            {
                await Shell.Current.DisplayAlertAsync("Внимание!", "Количество не может быть меньше нуля.", "Ок");
                return;
            }

            if (QtyPerPackage == 0)
            {
                IsEditMode = false;
                return;
            }

            var packageCountValue = PackageCount == 0 ? null : PackageCount;

            if (WarehouseId is Guid id)
            {
                if (WarehouseType == WarehouseType.FinishedGoods)
                {
                    var productRequest = new AddProductToWarehouseRequest(Item.Id, QtyPerPackage, packageCountValue);
                    await warehousesService.AddProductAsync(id, productRequest);
                }
                else
                {
                    var materialRequest = new AddMaterialToWarehouseRequest(Item.Id, QtyPerPackage, packageCountValue);
                    await warehousesService.AddMaterialAsync(id, materialRequest);
                }

                await LoadAsync();
            }

            else throw new NullReferenceException("WarehouseId cannot be null.");
        }
        finally
        {
            IsEditMode = false;
        }
    });
    #endregion

    #region EDIT QTY
    [RelayCommand]
    private async Task EditQtyAsync()
    {
        IsEditQtyMode = true;
    }
    #endregion

    #region SAVE QTY
    [RelayCommand]
    private async Task SaveQtyAsync() => await RunSafeActionAsync(async () =>
    {
        if (WarehouseId is null)
            return;

        var changedItemsList = СhangedItems.Except(RemovedItems).ToList(); // исключаем удаленные элементы из списка измененных

        if (changedItemsList.Count == 0)
        {
            IsEditQtyMode = false;
            return;
        }

        var isAgreed = await Shell.Current.DisplayAlertAsync(
             "Сохранить изменения количества?",
             "Вы уверены, что хотите сохранить изменения количества на складе?",
             "Да",
             "Нет");
        if (!isAgreed)
            return;


        if (WarehouseType is WarehouseType.FinishedGoods)
        {
            foreach (var item in changedItemsList)
            {
                var request = new UpdateWarehouseProductQtyRequest(item.QtyPerPackage, item.PackageCount);
                await warehousesService.UpdateProductQtyAsync(WarehouseId.Value, item.Id, request);
            }
        }
        if (WarehouseType is WarehouseType.Materials or WarehouseType.Aux)
        {
            foreach (var item in changedItemsList)
            {
                var request = new UpdateWarehouseMaterialQtyRequest(item.QtyPerPackage, item.PackageCount);
                await warehousesService.UpdateMaterialQtyAsync(WarehouseId.Value, item.Id, request);
            }
        }

        СhangedItems.Clear();
        RemovedItems.Clear();
    });
    #endregion

    #region TRANSFER
    [RelayCommand]
    private async Task TransferAsync(StockItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null || WarehouseId is null)
            return;

        var parameters = new Dictionary<string, object?>
            {
                { "WarehouseId", WarehouseId.Value.ToString() },
                { "WarehouseName", WarehouseName },
                { "ItemName", item.Name },
                { "UnitCode", item.UnitCode ?? string.Empty },
                { "AvailableQty", item.TotalQty.ToString() },
                { item.IsProduct ? "ProductId" : "MaterialId", item.Id.ToString() }
            };

        await Shell.Current.GoToAsync(nameof(TransferFromWarehousePage), parameters);
    });
    #endregion

    #region REMOVE ITEM
    [RelayCommand]
    private async Task RemoveItemAsync(StockItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null || WarehouseId is null)
            return;

        if (item.IsProduct)
        {
            await warehousesService.RemoveProductAsync(WarehouseId.Value, item.Id);
        }
        else
        {
            await warehousesService.RemoveMaterialAsync(WarehouseId.Value, item.Id);
        }

        RemovedItems.Add(item); // добавляем в список удаленных, чтобы исключить из изменений при сохранении

        await LoadAsync();

        OnIsEditQtyModeChanged(true);
    });
    #endregion

    #region OPEN ITEM
    [RelayCommand]
    private async Task OpenItemAsync(StockItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        if (item.Id == Guid.Empty)
            return;

        (string pageName, string idParameterName) = item.IsProduct
            ? (nameof(ProductDetailsPage), "ProductId")
            : (nameof(MaterialDetailsViewPage), "MaterialId");

        var parameters = new Dictionary<string, object?>
        {
            { idParameterName, item.Id.ToString() }
        };

        await Shell.Current.GoToAsync(pageName, parameters);
    });
    #endregion

    #region ITEM ENTRY FOCUSED - POPUP
    [RelayCommand]
    public async Task ItemEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = Guid.Empty;

        if (WarehouseType == WarehouseType.FinishedGoods)
        {
            id = await popupService.EntryFocusedAsync(productsService);
        }
        else
        {
            id = await popupService.EntryFocusedAsync(materialsService);
        }

        if (id == Guid.Empty) return;

        var selectedItem = await GetItemById(id);

        if (selectedItem is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Элемент не найден", "OK");
            return;
        }

        Item = selectedItem;
    });
    #endregion

    #region ADDITIONAL METHODS
    private async Task<StockItemViewModel?> GetItemById(Guid itemId)
    {
        if (WarehouseType is WarehouseType.FinishedGoods)
        {
            var product = await productsService.GetDetailsAsync(itemId);
            if (product is not null)
            {
                return new StockItemViewModel(
                    id: product.Id,
                    name: product.Name,
                    qtyPerPackage: 0,
                    unitCode: "шт.",
                    isProduct: true,
                    MarkChangedItem);
            }
        }

        else
        {
            var material = await materialsService.GetDetailsAsync(itemId);
            if (material is not null)
            {
                return new StockItemViewModel(
                    id: material.Id,
                    name: material.Name,
                    qtyPerPackage: 0,
                    unitCode: material.UnitCode,
                    isProduct: false,
                    MarkChangedItem);
            }
        }

        return null;
    }

    private async Task EnsureItemsForWarehouseTypeAsync()
    {
        Items.Clear();

        if (WarehouseType == WarehouseType.FinishedGoods)
        {
            if (ProductItems.Count == 0)
                await LoadProductsAsync();
            else
            {
                foreach (var product in ProductItems)
                    Items.Add(product);
                SetFirstItem();
            }
        }
        else
        {
            if (MaterialItems.Count == 0)
                await LoadMaterialsAsync();
            else
            {
                foreach (var material in MaterialItems)
                    Items.Add(material);
                SetFirstItem();
            }
        }
    }

    private async Task LoadProductsAsync()
    {
        var products = (await productsService.GetListAsync())?.Items;
        if (products is null || products.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Информация", "Нет доступных товаров.", "OK");
            return;
        }
        Items.Clear();
        foreach (var product in products)
        {
            var stockItem = new StockItemViewModel(
                id: product.Id,
                name: product.Name,
                qtyPerPackage: 0,
                unitCode: "шт.",
                isProduct: true,
                MarkChangedItem);

            ProductItems.Add(stockItem);
            Items.Add(stockItem);
        }
        SetFirstItem();
    }

    private async Task LoadMaterialsAsync()
    {
        var materials = (await materialsService.GetListAsync(isActive: true))?.Items;
        if (materials is null || materials.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Информация", "Нет доступных материалов.", "OK");
            return;
        }
        Items.Clear();
        foreach (var material in materials)
        {
            var stockItem = new StockItemViewModel(
                id: material.Id,
                name: material.Name,
                qtyPerPackage: material.TotalQty,
                unitCode: material.UnitCode,
                isProduct: false,
                markChangedItem: MarkChangedItem);

            MaterialItems.Add(stockItem);
            Items.Add(stockItem);
        }
        SetFirstItem();
    }

    private void SetFirstItem()
    {
        Item = Items.FirstOrDefault();
        UpdateUnits();
    }

    private void UpdateUnits()
    {
        Units = Item is null
            ? null
            : WarehouseType == WarehouseType.FinishedGoods
                ? "шт"
                : Item.UnitCode;
    }
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
    public partial class StockItemViewModel : ObservableObject
    {
        #region OBSERVABLE PROPERTIES
        [ObservableProperty]
        private decimal qtyPerPackage;

        [ObservableProperty]
        private decimal? packageCount;

        [ObservableProperty]
        private decimal totalQty;
        #endregion

        #region PROPERTIES
        public Guid Id { get; }
        public string Name { get; }
        public string? UnitCode { get; }
        public bool IsProduct { get; }
        #endregion

        private readonly Action<StockItemViewModel> _markChangedItem;
        private bool _trackChanges; // флаг для контроля отслеживания изменений

        #region CONSTRUCTORS
        public StockItemViewModel(WarehouseStockItemResponse response, WarehouseType warehouseType, Action<StockItemViewModel> markChangedItem)
        {
            _markChangedItem = markChangedItem;

            _trackChanges = false; // временно отключаем отслеживание изменений во время инициализации

            Id = response.ItemId;
            Name = response.Name;
            QtyPerPackage = response.QtyPerPackage;
            PackageCount = response.PackageCount;
            TotalQty = response.TotalQty;
            UnitCode = response.UnitCode;
            IsProduct = warehouseType == WarehouseType.FinishedGoods;

            _trackChanges = true; // включаем отслеживание изменений после инициализации
        }

        public StockItemViewModel(Guid id, string name, decimal qtyPerPackage, string? unitCode, bool isProduct, Action<StockItemViewModel> markChangedItem, decimal? packageCount = null)
        {
            _markChangedItem = markChangedItem;

            _trackChanges = false; // временно отключаем отслеживание изменений во время инициализации

            Id = id;
            Name = name;
            QtyPerPackage = qtyPerPackage;
            PackageCount = packageCount;
            TotalQty = QtyPerPackage * (PackageCount ?? 1);
            UnitCode = unitCode;
            IsProduct = isProduct;

            _trackChanges = true; // включаем отслеживание изменений после инициализации
        }
        #endregion

        #region ON ITEM CHANGED
        partial void OnQtyPerPackageChanged(decimal value)
        {
            TotalQty = (PackageCount ?? 1) * value;
            if (_trackChanges)
            {
                _markChangedItem.Invoke(this);
            }
        }

        partial void OnPackageCountChanged(decimal? value)
        {
            TotalQty = (value ?? 1) * QtyPerPackage;
            if (_trackChanges)
            {
                _markChangedItem.Invoke(this);
            }
        }
        #endregion
    }
    #endregion
}


