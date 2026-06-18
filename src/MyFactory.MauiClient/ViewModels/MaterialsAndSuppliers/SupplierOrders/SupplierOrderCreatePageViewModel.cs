using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Controllers;
using MyFactory.MauiClient.Models.Departments;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.MaterialTypes;
using MyFactory.MauiClient.Services.Printing;
using MyFactory.MauiClient.Services.SavingFile;
using MyFactory.MauiClient.Services.Suppliers;
using MyFactory.MauiClient.ViewModels.Production.ProductionOrders;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

public partial class SupplierOrderCreatePageViewModel : ObservableObject, IQueryAttributable
{
    #region ApplyQueryAttributes
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("SupplierId", out var supplierIdValue) && supplierIdValue is string supplierIdResult)
            SupplierIdParameter = supplierIdResult;

        if (query.TryGetValue("MaterialId", out var materialIdValue) && materialIdValue is string materialIdResult)
            MaterialIdParameter = materialIdResult;

        if (query.TryGetValue("QtyMaterial", out var qtyMaterialValue) && qtyMaterialValue is decimal qtyMaterialResult)
            QtyMaterialParameter = qtyMaterialResult;

        if (query.TryGetValue("SelectedSalesOrder", out var selectedSalesOrderParameterValue)
            && selectedSalesOrderParameterValue is SalesOrderDetailsResponse selectedSalesOrderParameterResult)
            SelectedSalesOrderParameter = selectedSalesOrderParameterResult;

        if (query.TryGetValue("SelectedSalesOrderItem", out var selectedSalesOrderItemParameterValue)
            && selectedSalesOrderItemParameterValue is SalesOrderItemResponse selectedSalesOrderItemParameterResult)
            SelectedProductParameter = selectedSalesOrderItemParameterResult;

        if (query.TryGetValue("Qty", out var qtyValue) && qtyValue is decimal qty)
            QtyParameter = qty;

        if (query.TryGetValue("SelectedDepartment", out var selectedDepartmentParameterValue)
            && selectedDepartmentParameterValue is DepartmentListItemResponse selectedDepartmentParameterResult)
            SelectedDepartmentParameter = selectedDepartmentParameterResult;

        if (query.TryGetValue("Mode", out var modeValue) && modeValue is ProductionOrderCreatePageMode modeResult)
            ModeParameter = modeResult;
    }
    #endregion 

    #region Constants
    private const string NoMaterialType = "Без типа";
    #endregion

    #region Services
    private readonly IMaterialPurchaseOrdersService _ordersService;
    private readonly ISuppliersService _suppliersService;
    private readonly IMaterialsService _materialsService;
    private readonly IMaterialTypesService _materialTypesService;
    private readonly IPrintService _printService;
    private readonly ISaveFileService _saveFileService;
    private readonly IPopupService _popupService;
    #endregion

    #region Observable Properties
    [ObservableProperty] private Guid? supplierId;
    [ObservableProperty] private string? supplierIdParameter;
    [ObservableProperty] private SupplierDetailsResponse? supplier;
    [ObservableProperty] private Guid? materialId;
    [ObservableProperty] private string? materialIdParameter;
    [ObservableProperty] private decimal qtyMaterialParameter = 1;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private ObservableCollection<object?> selectedItems = [];
    [ObservableProperty] private bool hasSelectedItems;

    [ObservableProperty] private SalesOrderDetailsResponse? selectedSalesOrderParameter;
    [ObservableProperty] private SalesOrderItemResponse? selectedProductParameter;
    [ObservableProperty] private decimal? qtyParameter;
    [ObservableProperty] private DepartmentListItemResponse? selectedDepartmentParameter;
    [ObservableProperty] private ProductionOrderCreatePageMode? modeParameter;
    #endregion

    #region Observable Collections
    public ObservableCollection<string> MaterialTypeOptions { get; } = new();
    public ObservableCollection<OrderItemViewModel> Items { get; } = new();
    #endregion

    #region Constructor
    public SupplierOrderCreatePageViewModel(
        IMaterialPurchaseOrdersService ordersService,
        ISuppliersService suppliersService,
        IMaterialsService materialsService,
        IMaterialTypesService materialTypesService,
        IPrintService printService,
        ISaveFileService saveFileService,
        IPopupService popupService)
    {
        _ordersService = ordersService;
        _suppliersService = suppliersService;
        _materialsService = materialsService;
        _materialTypesService = materialTypesService;
        _printService = printService;
        _saveFileService = saveFileService;
        _popupService = popupService;

        SubscribeSelectionCollection();
    }
    #endregion

    #region On Changed
    private void SelectedItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        HasSelectedItems = SelectedItems.Count > 0;
    }

    private void SubscribeSelectionCollection()
    {
        SelectedItems.CollectionChanged += SelectedItemsCollectionChanged;
        HasSelectedItems = SelectedItems.Count > 0;
    }

    partial void OnSelectedItemsChanging(ObservableCollection<object?> value)
    {
        if (SelectedItems is not null)
            SelectedItems.CollectionChanged -= SelectedItemsCollectionChanged;
    }

    partial void OnSelectedItemsChanged(ObservableCollection<object?> value)
    {
        if (value is not null)
            value.CollectionChanged += SelectedItemsCollectionChanged;
        HasSelectedItems = value?.Count > 0;
    }

    partial void OnSupplierIdChanged(Guid? value)
    {
        if (value.HasValue)
        {
            _ = SetSupplierById(value.Value);
        }
    }

    partial void OnSupplierIdParameterChanged(string? value)
    {
        SupplierId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnMaterialIdChanged(Guid? value)
    {
        if (value.HasValue)
        {
            _ = SetMaterialById(value.Value);
        }
    }

    partial void OnMaterialIdParameterChanged(string? value)
    {
        MaterialId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region LoadCommand
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        // Supplier
        if (Supplier is null && SupplierId is not null)
        {
            var suppliers = (await _suppliersService.GetListAsync(take: 1))?.Items;
            if (suppliers is not null && suppliers.Count > 0)
            {
                var supplier = suppliers[0];
                Supplier = await _suppliersService.GetDetailsAsync(supplier.Id);
                SupplierId = Supplier?.Id;
            }
        }
        // MaterialTypes
        MaterialTypeOptions.Clear();

        var materialTypes = await _materialTypesService.GetListAsync(usedOnly: true);

        foreach (var t in materialTypes)
            MaterialTypeOptions.Add(t.Name);

        MaterialTypeOptions.Add(NoMaterialType);

        //if (Items.Count)
        //{
        //    await AddOrderItemAsync();
        //}
    });
    #endregion

    #region AddItemCommand
    [RelayCommand]
    private async Task AddItemAsync() => await RunSafeActionAsync(AddOrderItemAsync);
    #endregion

    #region AddOrderItem
    private async Task AddOrderItemAsync()
    {
        var initialType = MaterialTypeOptions.FirstOrDefault() ?? NoMaterialType;

        MaterialDetailsResponse? material = null;
        var materials = (await _materialsService.GetListAsync(take: 1, searchType: initialType))?.Items;
        if (materials is not null && materials.Count > 0)
        {
            var materialItem = materials[0];
            material = await _materialsService.GetDetailsAsync(materialItem.Id);
        }

        Items.Insert(0, new OrderItemViewModel(this)
        {
            MaterialName = material?.Name,
            MaterialType = initialType,
            Material = material,
            Qty = 1,
            UnitCode = material?.UnitCode,
            Price = 0
        });
    }
    #endregion

    #region SaveCommand
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (Items.Count <= 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "В заказе должна быть хотя бы одна позиция.", "OK");
            return;
        }

        if (Supplier is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Определите поставщика материалов.", "OK");
            return;
        }

        var createResponse = await _ordersService.CreateAsync(new CreateMaterialPurchaseOrderRequest(Supplier.Id, DateTime.UtcNow)) ?? throw new InvalidOperationException("Не удалось создать заказ.");
        foreach (var item in Items)
        {
            if (item.Material is null)
                continue;

            var request = new AddMaterialPurchaseOrderItemRequest(item.Material.Id, item.Qty, item.Price);
            await _ordersService.AddItemAsync(createResponse.Id, request);
        }

        await _ordersService.ConfirmAsync(createResponse.Id);
        await Shell.Current.DisplayAlertAsync("Успешно!", "Заказ создан.", "OK");
        await BackNavigationAsync();
    });
    #endregion

    #region SaveFileCommand
    [RelayCommand]
    private async Task SaveFileAsync() => await RunSafeActionAsync(async () =>
    {
        var content = BuildPrintableText();
        await _saveFileService.SaveFileAsync("Заказ поставщику", content);
        await Shell.Current.DisplayAlertAsync("Сохранено", "Файл заказа сохранён.", "OK");
    });
    #endregion

    #region PrintCommand
    [RelayCommand]
    private async Task PrintAsync() => await RunSafeActionAsync(async () =>
    {
        try
        {
            var content = BuildPrintableText();
            await _printService.PrintTextAsync("Заказ поставщику", content);
        }
        catch (PlatformNotSupportedException ex)
        {
            throw new PlatformNotSupportedException($"Печать: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка печати: {ex.Message}");
        }
    });
    #endregion

    #region BuildPrintableText
    private string BuildPrintableText()
    {
        var sb = new StringBuilder();
        var supplierName = Supplier?.Name ?? "Поставщик не выбран.";
        sb.AppendLine($"Поставщик: {supplierName}");
        sb.AppendLine($"Дата: {DateTime.Now:G}");
        sb.AppendLine(new string('-', 48));

        foreach (var item in Items)
        {
            var name = item.Material?.Name ?? item.MaterialType ?? "Материал";
            sb.AppendLine($"{name}");
            sb.AppendLine($"    Количество: {item.Qty}");
            sb.AppendLine($"    Цена: {item.Price}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
    #endregion

    #region RemoveSelectedCommand
    [RelayCommand]
    private async Task RemoveSelectedAsync() => await RunSafeActionAsync(async () =>
    {
        if (SelectedItems.Count == 0)
            return;

        var toRemove = SelectedItems.OfType<OrderItemViewModel>().ToList();
        foreach (var item in toRemove)
        {
            Items.Remove(item);
        }

        SelectedItems.Clear();
    });
    #endregion

    #region BackCommand
    [RelayCommand]
    private async Task BackAsync() => await BackNavigationAsync();
    #endregion

    #region MaterialEntryFocusedCommand
    [RelayCommand]
    public async Task MaterialEntryFocusedAsync(OrderItemViewModel item) => await RunSafeActionAsync(async () =>
    {
        var id = await _popupService.EntryFocusedAsync(service: _materialsService, type: item.MaterialType);

        if (id == default)
        {
            return;
        }

        var material = await _materialsService.GetDetailsAsync(id);
        if (material is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Материал не найден.", "OK");
            return;
        }

        item.Material = material;
    });
    #endregion

    #region SupplierEntryFocusedCommand
    [RelayCommand]
    public async Task SupplierEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        var id = await _popupService.EntryFocusedAsync(_suppliersService);

        if (id == default)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Пустой ID поставщика.", "OK");
            return;
        }

        Supplier = await _suppliersService.GetDetailsAsync(id);
        SupplierId = Supplier?.Id;

        if (Supplier == null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", $"Поставщик не найден.", "OK");
        }
    });
    #endregion

    #region BackNavigation
    private async Task BackNavigationAsync()
    {
        if (SelectedSalesOrderParameter is not null
            && SelectedProductParameter is not null
            && QtyParameter is not null
            && SelectedDepartmentParameter is not null
            && ModeParameter is not null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "SelectedSalesOrder", SelectedSalesOrderParameter },
                { "SelectedSalesOrderItem", SelectedProductParameter },
                { "Qty", QtyParameter },
                { "SelectedDepartment", SelectedDepartmentParameter },
                { "Mode", ModeParameter }
            };

            await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage), parameters);
            return;
        }

        await Shell.Current.BackSafeAsync(
            fallbackRoute: nameof(SupplierDetailsPage),
            parameters: new Dictionary<string, object> { { "SupplierId", SupplierId?.ToString() ?? string.Empty } },
            animate: true);
    }
    #endregion

    #region RunSafeAction
    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region SetMaterialById
    private async Task SetMaterialById(Guid id)
    {
        var material = await _materialsService.GetDetailsAsync(id);

        if (material is not null && Items.Count == 0)
        {
            var totalPrice = material.PurchaseHistory[0].UnitPrice * QtyMaterialParameter;

            Items.Add(new OrderItemViewModel(this)
            {
                MaterialName = material.Name,
                MaterialType = material.MaterialType,
                Material = material,
                Qty = QtyMaterialParameter,
                UnitCode = material.UnitCode,
                Price = totalPrice
            });
        }
    }
    #endregion

    #region SetSupplierById
    private async Task SetSupplierById(Guid supplierId)
    {
        Supplier = await _suppliersService.GetDetailsAsync(supplierId);
    }
    #endregion

    #region OrderItemViewModel
    public partial class OrderItemViewModel(SupplierOrderCreatePageViewModel parent) : ObservableObject
    {
        [ObservableProperty]
        private string? materialType;

        [ObservableProperty]
        private string? materialName;

        [ObservableProperty]
        private MaterialDetailsResponse? material;

        [ObservableProperty]
        private decimal qty;

        [ObservableProperty]
        private string? unitCode;

        [ObservableProperty]
        private decimal price;

        partial void OnMaterialTypeChanged(string? value)
        {
            _ = ChangeMaterialByType(value);
        }

        partial void OnMaterialChanged(MaterialDetailsResponse? value)
        {
            UnitCode = value?.UnitCode;
            MaterialName = value?.Name;
            // MaterialType = value?.MaterialType;
            RecalculatePrice();
        }

        partial void OnQtyChanged(decimal oldValue, decimal newValue)
        {
            RecalculatePrice();
        }

        private void RecalculatePrice()
        {
            var unitPrice = Material?.PurchaseHistory?.FirstOrDefault()?.UnitPrice ?? 0m;
            Price = unitPrice * Qty;
        }

        private async Task ChangeMaterialByType(string? value)
        {
            var materials = (await parent._materialsService.GetListAsync(take: 1, searchName: MaterialName, searchType: value))?.Items;

            if (materials is null || materials.Count == 0)
            {
                materials = (await parent._materialsService.GetListAsync(take: 1, searchType: value))?.Items;
            }

            if (materials is null || materials.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("Внимание!", "Не найден материал такого типа", "Ok");
                return;
            }

            Material = await parent._materialsService.GetDetailsAsync(materials[0].Id);
            MaterialName = Material?.Name;
        }
    }
    #endregion
}
