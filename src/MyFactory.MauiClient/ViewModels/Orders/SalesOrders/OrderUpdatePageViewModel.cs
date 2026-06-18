using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Pages.Orders.SalesOrders;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.ProductTypes;
using MyFactory.MauiClient.Services.SalesOrders;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

[QueryProperty(nameof(OrderIdParameter), "OrderId")]
public partial class OrderUpdatePageViewModel : ObservableObject
{
    #region Constants
    private const string NoProductType = "Без типа";
    #endregion

    #region Services
    private readonly ISalesOrdersService _salesOrdersService;
    private readonly IProductsService _productsService;
    private readonly IProductTypesService _productTypesService;
    private readonly IPopupService _popupService;
    #endregion

    #region RemovedItemIds
    private readonly HashSet<Guid> _removedItemIds = [];
    #endregion

    #region ObservableProperties
    [ObservableProperty] private Guid? orderId;
    [ObservableProperty] private string? orderIdParameter;

    [ObservableProperty] private int orderNumber;
    [ObservableProperty] private SalesOrderStatus orderStatus;
    [ObservableProperty] private string statusText = string.Empty;

    [ObservableProperty] private CustomerDetailsResponse? customer;
    [ObservableProperty] private DateTime orderDate = DateTime.Today;
    [ObservableProperty] private TimeSpan orderTime;

    [ObservableProperty] private bool isEdit;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;

    [ObservableProperty] private decimal? totalOrdersCost;
    [ObservableProperty] private string? totalOrdersCostString;
    #endregion

    #region Variables
    private bool _isLoadingItems;
    #endregion

    #region Properties
    public string TitleText => OrderNumber > 0 ? $"Заказ #{OrderNumber}" : "Редактирование заказа";
    #endregion

    #region ObservableCollections
    public ObservableCollection<string> ProductTypeOptions { get; } = new();
    public ObservableCollection<OrderItemViewModel> Items { get; } = new();
    #endregion

    #region Constructor
    public OrderUpdatePageViewModel(
            ISalesOrdersService salesOrdersService,
            IProductsService productsService,
            IProductTypesService productTypesService,
            IPopupService popupService)
    {
        _salesOrdersService = salesOrdersService;
        _productsService = productsService;
        _productTypesService = productTypesService;
        _popupService = popupService;
    }
    #endregion

    #region OnChanged
    partial void OnOrderIdParameterChanged(string? value)
    {
        OrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnOrderNumberChanged(int value) => OnPropertyChanged(nameof(TitleText));

    partial void OnTotalOrdersCostChanged(decimal? value)
    {
        TotalOrdersCostString = $"Итого: {value:0.##} р.";
    }
    #endregion

    #region Load
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        if (OrderId is null)
            return;

        await EnsureProductTypesAsync();

        var details = await _salesOrdersService.GetDetailsAsync(OrderId.Value)
            ?? throw new InvalidOperationException("Заказ не найден.");

        _removedItemIds.Clear();
        Items.Clear();

        OrderNumber = details.OrderNumber;
        OrderStatus = details.Status;
        StatusText = details.Status.SalesOrderRusStatus();
        Customer = details.Customer;
        OrderDate = details.OrderDate.Date;
        OrderTime = details.OrderDate.TimeOfDay;

        IsEdit = details.Status is SalesOrderStatus.New or SalesOrderStatus.Confirmed;

        _isLoadingItems = true;
        foreach (var item in details.Items)
        {
            var product = await _productsService.GetDetailsAsync(item.ProductId);
            var productType = await ResolveProductTypeByProductIdAsync(item.ProductId);

            Items.Add(new OrderItemViewModel(this)
            {
                OrderItemId = item.Id,
                OriginalProductId = item.ProductId,
                Product = product,
                ProductName = product?.Name ?? item.ProductName,
                ProductType = productType,
                Qty = item.QtyOrdered
            });
        }
        _isLoadingItems = false;
        RecalculateTotal();
    });
    #endregion

    #region AddItemCommand
    [RelayCommand]
    private async Task AddItemAsync() => await RunSafeActionAsync(async () =>
    {
        if (!IsEdit)
            return;

        var initialType = ProductTypeOptions.FirstOrDefault() ?? NoProductType;
        var initialProduct = await ResolveFirstProductAsync(initialType);

        Items.Add(new OrderItemViewModel(this)
        {
            ProductType = initialType,
            Product = initialProduct,
            ProductName = initialProduct?.Name ?? string.Empty,
            Qty = 1
        });
    });
    #endregion

    #region RemoveItemCommand
    [RelayCommand]
    private async Task RemoveItemAsync(OrderItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (!IsEdit || item is null)
            return;

        if (item.OrderItemId.HasValue)
            _removedItemIds.Add(item.OrderItemId.Value);

        Items.Remove(item);

        if (Items.Count == 0)
            await AddItemAsync();
    });
    #endregion

    #region ProductEntryFocusedCommand
    [RelayCommand]
    private async Task ProductEntryFocusedAsync(OrderItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (!IsEdit || item is null)
            return;

        var id = await _popupService.EntryFocusedAsync(_productsService, type: NormalizeType(item.ProductType));
        if (id == Guid.Empty)
            return;

        item.Product = await _productsService.GetDetailsAsync(id);
        item.ProductName = item.Product?.Name ?? item.ProductName;

        if (item.Product is null)
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Товар не найден.", "OK");
    });
    #endregion

    #region SaveCommand
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (!IsEdit || OrderId is null || Customer is null)
            return;

        var validItems = Items.Where(x => x.Product is not null && x.Qty > 0).ToList();
        if (validItems.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Добавьте хотя бы один товар с количеством больше 0.", "OK");
            return;
        }

        var orderDateTime = OrderDate.Date.Add(OrderTime);
        await _salesOrdersService.UpdateAsync(OrderId.Value, new UpdateSalesOrderRequest(Customer.Id, orderDateTime));

        foreach (var removedId in _removedItemIds)
            await _salesOrdersService.RemoveItemAsync(removedId);

        foreach (var item in validItems)
        {
            if (item.Product is null)
                continue;

            if (!item.OrderItemId.HasValue)
            {
                await _salesOrdersService.AddItemAsync(OrderId.Value, new AddSalesOrderItemRequest(item.Product.Id, item.Qty));
                continue;
            }

            if (item.OriginalProductId.HasValue && item.OriginalProductId.Value != item.Product.Id)
            {
                await _salesOrdersService.RemoveItemAsync(item.OrderItemId.Value);
                await _salesOrdersService.AddItemAsync(OrderId.Value, new AddSalesOrderItemRequest(item.Product.Id, item.Qty));
                continue;
            }

            await _salesOrdersService.UpdateItemAsync(item.OrderItemId.Value, new UpdateSalesOrderItemRequest(item.Qty));
        }

        await Shell.Current.DisplayAlertAsync("Успешно!", "Заказ обновлён.", "OK");
        await BackNavigationAsync();
    });
    #endregion

    #region DeleteOrderCommand
    [RelayCommand]
    private async Task DeleteOrderAsync() => await RunSafeActionAsync(async () =>
    {
        if (!IsEdit || OrderId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync(
            "Внимание!",
            "Вы уверены, что хотите удалить заказ?",
            "Да",
            "Нет");

        if (!confirm)
            return;

        await _salesOrdersService.DeleteAsync(OrderId.Value);
        await BackNavigationAsync();
    });
    #endregion

    #region CancelOrderCommand
    [RelayCommand]
    private async Task CancelOrderAsync() => await RunSafeActionAsync(async () =>
    {
        if (!IsEdit || OrderId is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync(
            "Внимание!",
            "Вы уверены, что хотите отменить заказ?",
            "Да",
            "Нет");

        if (!confirm) return;

        await _salesOrdersService.CancelAsync(OrderId.Value);
        await Shell.Current.DisplayAlertAsync("Успешно!", "Заказ отменён.", "OK");
        await BackNavigationAsync();
    });
    #endregion

    #region BackCommand
    [RelayCommand]
    private async Task BackAsync() => await BackNavigationAsync();
    #endregion

    #region EnsureProductTypes
    private async Task EnsureProductTypesAsync()
    {
        if (ProductTypeOptions.Count > 0)
            return;

        var types = await _productTypesService.GetListAsync();

        ProductTypeOptions.Clear();
        foreach (var type in types)
            ProductTypeOptions.Add(type.Type);

        ProductTypeOptions.Add(NoProductType);
    }
    #endregion

    #region ResolveFirstProduct
    private async Task<ProductDetailsResponse?> ResolveFirstProductAsync(string? type)
    {
        var list = (await _productsService.GetListAsync(take: 1, searchType: NormalizeType(type)))?.Items;
        var first = list?.FirstOrDefault();

        if (first is null)
            return null;

        return await _productsService.GetDetailsAsync(first.Id);
    }
    #endregion

    #region ResolveProductTypeByProductId
    private async Task<string> ResolveProductTypeByProductIdAsync(Guid productId)
    {
        var type = await _productTypesService.GetByProductIdAsync(productId);
        return type?.Type ?? NoProductType;
    }
    #endregion

    #region NormalizeType
    private static string NormalizeType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return string.Empty;

        return string.Equals(type, NoProductType, StringComparison.OrdinalIgnoreCase)
            ? string.Empty
            : type;
    }
    #endregion

    #region RecalculateTotal
    private void RecalculateTotal()
    {
        TotalOrdersCost = Items.Sum(i => (i.Product?.TotalCost ?? 0) * i.Qty);
    }
    #endregion

    #region BackNavigation
    private async Task BackNavigationAsync() => await Shell.Current.BackSafeAsync(
            fallbackRoute: nameof(OrdersListPage),
            animate: true);
    #endregion

    #region RunSafeAction
    private async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: value => IsBusy = value,
            setError: message => ErrorMessage = message,
            showError: message => Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region OrderItemViewModel
    public partial class OrderItemViewModel(OrderUpdatePageViewModel parent) : ObservableObject
    {
        [ObservableProperty] private Guid? orderItemId;
        [ObservableProperty] private Guid? originalProductId;
        [ObservableProperty] private string productName = string.Empty;
        [ObservableProperty] private string? productType;
        [ObservableProperty] private ProductDetailsResponse? product;
        [ObservableProperty] private decimal qty;
        [ObservableProperty] public string qtyText = "0 шт.";
        [ObservableProperty] public string totalCost = $"0 р.";

        partial void OnQtyChanged(decimal value)
        {
            QtyText = $"{value:0.##} шт.";
            var costOfProducts = (Product?.TotalCost ?? 0) * value;
            TotalCost = $"{costOfProducts:0.##} р.";

            if (parent._isLoadingItems) return;
            parent.RecalculateTotal();
        }

        partial void OnProductChanged(ProductDetailsResponse? value)
        {
            if (value is not null)
                ProductName = value.Name;

            var costOfProducts = (Product?.TotalCost ?? 0) * Qty;
            TotalCost = $"{costOfProducts:0.##} р.";

            if (parent._isLoadingItems) return;
            parent.RecalculateTotal();
        }

        partial void OnProductTypeChanged(string? value)
        {
            if (!parent.IsEdit)
                return;

            _ = ChangeProductByTypeAsync(value);
        }

        private async Task ChangeProductByTypeAsync(string? type)
        {
            var product = await parent.ResolveFirstProductAsync(type);
            if (product is null)
                return;

            Product = product;
            ProductName = product.Name;
        }
    }
    #endregion
}
