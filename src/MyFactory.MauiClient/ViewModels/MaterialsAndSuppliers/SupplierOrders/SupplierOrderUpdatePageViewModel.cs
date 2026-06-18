using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Materials;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Pages.MaterialsAndSuppliers.Suppliers;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Materials;
using MyFactory.MauiClient.Services.MaterialTypes;
using MyFactory.MauiClient.Services.Printing;
using MyFactory.MauiClient.Services.SavingFile;
using MyFactory.MauiClient.Services.Suppliers;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.SupplierOrders;

[QueryProperty(nameof(SupplierIdParameter), "SupplierId")]
[QueryProperty(nameof(MaterialIdParameter), "MaterialId")]
[QueryProperty(nameof(PurchaseOrderIdParameter), "PurchaseOrderId")]
[QueryProperty(nameof(QtyMaterialParameter), "QtyMaterial")]
public partial class SupplierOrderUpdatePageViewModel : ObservableObject
{
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

    #region OBSERVABLE PROPERTIES
    [ObservableProperty] private Guid? supplierId;
    [ObservableProperty] private string? supplierIdParameter;
    [ObservableProperty] private SupplierDetailsResponse? supplier;
    [ObservableProperty] private Guid? materialId;
    [ObservableProperty] private string? materialIdParameter;
    [ObservableProperty] private Guid? purchaseOrderId;
    [ObservableProperty] private string? purchaseOrderIdParameter;
    [ObservableProperty] private decimal qtyMaterialParameter = 1;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    [ObservableProperty] private bool hasSelectedItems;
    [ObservableProperty] private bool isEditMode;
    [ObservableProperty] private ObservableCollection<object?> selectedItems = [];
    #endregion

    #region PROPERTIES
    public bool IsDetailsMode => !IsEditMode;
    public string Title => IsEditMode ? "Редактирование заказа" : "Детали заказа";
    public bool HasSelectedItemsForDelete => HasSelectedItems && IsEditMode;
    #endregion

    #region OBSERVABLE COLLECTIONS
    public ObservableCollection<string> MaterialTypeOptions { get; } = new();
    public ObservableCollection<OrderItemViewModel> Items { get; } = new();
    #endregion

    #region Constructor
    public SupplierOrderUpdatePageViewModel(
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

    #region ON CHANGED
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

    partial void OnHasSelectedItemsChanged(bool value)
    {
        OnPropertyChanged(nameof(HasSelectedItemsForDelete));
    }

    partial void OnIsEditModeChanged(bool value)
    {
        OnPropertyChanged(nameof(IsDetailsMode));
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(HasSelectedItemsForDelete));
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

    partial void OnPurchaseOrderIdParameterChanged(string? value)
    {
        PurchaseOrderId = Guid.TryParse(value, out var id) ? id : null;
    }
    #endregion

    #region LOAD
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

        // PurchaseOrder
        if (PurchaseOrderId is not null)
        {
            var details = await _ordersService.GetDetailsAsync(PurchaseOrderId.Value);
            if (details is not null)
            {
                SupplierId = details.SupplierId;

                Items.Clear();

                IsEditMode = details.Status is PurchaseOrderStatus.New or PurchaseOrderStatus.Confirmed;

                foreach (var item in details.Items)
                {
                    var material = await _materialsService.GetDetailsAsync(item.MaterialId);
                    Items.Add(new OrderItemViewModel(this)
                    {
                        Id = item.Id,
                        MaterialName = material?.Name,
                        MaterialType = material?.MaterialType ?? string.Empty,
                        Material = material,
                        Qty = item.Qty,
                        Price = item.UnitPrice
                    });
                }
            }
        }

        if (Items.Count == 0)
        {
            await AddOrderItemAsync();
        }
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
            Price = 0
        });
    }
    #endregion

    #region SAVE
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (Items.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Добавьте хотя бы одну позицию", "OK");
            return;
        }

        if (Supplier is null)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", "Выберите поставщика", "OK");
            return;
        }

        Guid orderId;
        if (PurchaseOrderId is null)
        {
            var createResponse = await _ordersService.CreateAsync(new CreateMaterialPurchaseOrderRequest(Supplier.Id, DateTime.UtcNow))
                ?? throw new InvalidOperationException("Не удалось создать заказ");
            orderId = createResponse.Id;
            PurchaseOrderId = orderId;
        }
        else
        {
            orderId = PurchaseOrderId.Value;

            var orderStatus = (await _ordersService.GetDetailsAsync(orderId))?.Status;
            if (orderStatus is PurchaseOrderStatus.Cancelled or PurchaseOrderStatus.Received)
            {
                await BackNavigationAsync();
                return;
            }
        }

        foreach (var item in Items)
        {
            if (item.Material is null)
                continue;

            if (item.Id.HasValue)
            {
                await _ordersService.UpdateItemAsync(item.Id.Value, new UpdateMaterialPurchaseOrderItemRequest(item.Qty, item.Price));
            }
            else
            {
                var request = new AddMaterialPurchaseOrderItemRequest(item.Material.Id, item.Qty, item.Price);
                await _ordersService.AddItemAsync(orderId, request);
            }
        }

        await _ordersService.ConfirmAsync(orderId);
        await Shell.Current.DisplayAlertAsync("Успех", "Заказ сохранен", "OK");
        await BackNavigationAsync();
    });
    #endregion

    #region REMOVE SELECTED
    [RelayCommand]
    private async Task RemoveSelectedAsync() => await RunSafeActionAsync(async () =>
    {
        if (SelectedItems.Count == 0)
            return;

        var toRemove = SelectedItems.OfType<OrderItemViewModel>().ToList();
        foreach (var item in toRemove)
        {
            if (item.Id.HasValue) await _ordersService.RemoveItemAsync(item.Id.Value);
            Items.Remove(item);
        }

        SelectedItems.Clear();
    });
    #endregion

    #region DELETE ORDER
    [RelayCommand]
    private async Task DeleteOrderAsync() => await RunSafeActionAsync(async () =>
    {
        if (PurchaseOrderId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удалить", "Удалить заказ?", "Да", "Нет");
        if (!confirm)
            return;

        await _ordersService.CancelAsync(PurchaseOrderId.Value);
        await Shell.Current.DisplayAlertAsync("Готово", "Заказ удален", "OK");
        await BackNavigationAsync();
    });
    #endregion

    #region COMPLETE
    [RelayCommand]
    private async Task CompleteAsync() => await RunSafeActionAsync(async () =>
    {
        if (PurchaseOrderId is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderCompletePage", new Dictionary<string, object>
        {
            { "PurchaseOrderId", PurchaseOrderId.Value.ToString() }
        });
    });
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

    #region SAVE FILE
    [RelayCommand]
    private async Task SaveFileAsync() => await RunSafeActionAsync(async () =>
    {
        var content = BuildPrintableText();
        await _saveFileService.SaveFileAsync("Заказ поставщику", content);
        await Shell.Current.DisplayAlertAsync("Сохранено", "Файл заказа сохранён.", "OK");
    });
    #endregion

    #region PRINT
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

    #region BUILD PRINTABLE TEXT
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

    #region BackNavigationAsync Method
    private async Task BackNavigationAsync() => await Shell.Current.BackSafeAsync(
        fallbackRoute: nameof(SupplierDetailsPage),
        parameters: new Dictionary<string, object> { { "SupplierId", SupplierId?.ToString() ?? string.Empty } },
        animate: true);
    #endregion

    #region RUN SAFE ACTION
    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region BACK
    [RelayCommand]
    private async Task BackAsync()
    {
        await BackNavigationAsync();
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

    #region ORDER ITEM
    public partial class OrderItemViewModel(SupplierOrderUpdatePageViewModel parent) : ObservableObject
    {
        [ObservableProperty]
        private Guid? id;

        [ObservableProperty]
        private string? materialType;

        [ObservableProperty]
        private string? materialName;

        [ObservableProperty]
        private MaterialDetailsResponse? material;

        [ObservableProperty]
        private decimal qty;

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private string? unitCode;

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