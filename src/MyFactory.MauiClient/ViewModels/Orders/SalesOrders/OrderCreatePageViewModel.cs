using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Models.Products;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Pages.Orders.SalesOrders;
using MyFactory.MauiClient.Services.Customers;
using MyFactory.MauiClient.Services.Products;
using MyFactory.MauiClient.Services.ProductTypes;
using MyFactory.MauiClient.Services.SalesOrders;
using System.Collections.ObjectModel;

namespace MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

public partial class OrderCreatePageViewModel : ObservableObject
{
    #region Constants
    private const string NoProductType = "Без типа";
    #endregion

    #region Services
    private readonly ISalesOrdersService _salesOrdersService;
    private readonly ICustomersService _customersService;
    private readonly IProductsService _productsService;
    private readonly IProductTypesService _productTypesService;
    private readonly IPopupService _popupService;
    #endregion

    #region ObservableProperties
    [ObservableProperty] private CustomerDetailsResponse? customer;
    [ObservableProperty] private TimeSpan orderTime = DateTime.Now.TimeOfDay;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? errorMessage;
    #endregion

    #region ObservableCollections
    public ObservableCollection<string> ProductTypeOptions { get; } = new();
    public ObservableCollection<OrderItemViewModel> Items { get; } = new();
    #endregion

    #region Constructor
    public OrderCreatePageViewModel(
        ISalesOrdersService salesOrdersService,
        ICustomersService customersService,
        IProductsService productsService,
        IProductTypesService productTypesService,
        IPopupService popupService)
    {
        _salesOrdersService = salesOrdersService;
        _customersService = customersService;
        _productsService = productsService;
        _productTypesService = productTypesService;
        _popupService = popupService;
    }
    #endregion

    #region Load
    public async Task LoadAsync() => await RunSafeActionAsync(async () =>
    {
        await EnsureCustomerAsync();
        await EnsureProductTypesAsync();

        // if (Items.Count == 0)
        //     await AddOrderItemAsync();
    });
    #endregion

    #region AddItemCommand
    [RelayCommand]
    private async Task AddItemAsync() => await RunSafeActionAsync(AddOrderItemAsync);

    [RelayCommand]
    private async Task RemoveItemAsync(OrderItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        Items.Remove(item);

        if (Items.Count == 0)
            await AddOrderItemAsync();
    });
    #endregion

    #region CustomerEntryFocusedCommand
    [RelayCommand]
    private async Task CustomerEntryFocusedAsync() => await RunSafeActionAsync(async () =>
    {
        Guid id = await _popupService.EntryFocusedAsync(_customersService, isActive: true);
        if (id == Guid.Empty)
            return;

        var selectedCustomer = await _customersService.GetDetailsAsync(id);

        if (selectedCustomer is null)
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Заказчик не найден.", "OK");

        Customer = selectedCustomer;
    });
    #endregion

    #region ProductEntryFocusedCommand
    [RelayCommand]
    private async Task ProductEntryFocusedAsync(OrderItemViewModel? item) => await RunSafeActionAsync(async () =>
    {
        if (item is null)
            return;

        var id = await _popupService.EntryFocusedAsync(_productsService, type: NormalizeType(item.ProductType), isActive: true);
        if (id == Guid.Empty)
            return;

        var selectedProduct = await _productsService.GetDetailsAsync(id);

        if (selectedProduct is null)
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Товар не найден.", "OK");

        item.Product = selectedProduct;
    });
    #endregion

    #region SaveCommand
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {
        if (Customer is null)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Выберите заказчика.", "OK");
            return;
        }

        var validItems = Items.Where(x => x.Product is not null && x.Qty > 0).ToList();
        if (validItems.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Внимание!", "Добавьте хотя бы один товар с количеством больше 0.", "OK");
            return;
        }

        var orderDateLocal = DateTime.Today.Add(OrderTime);
        var orderDateUtc = DateTime.SpecifyKind(orderDateLocal, DateTimeKind.Local).ToUniversalTime();
        var created = await _salesOrdersService.CreateAsync(new CreateSalesOrderRequest(Customer.Id, orderDateUtc))
            ?? throw new InvalidOperationException("Не удалось создать заказ.");

        foreach (var item in validItems)
        {
            await _salesOrdersService.AddItemAsync(
                created.Id,
                new AddSalesOrderItemRequest(item.Product!.Id, item.Qty));
        }

        await Shell.Current.DisplayAlertAsync("Успешно!", "Заказ создан.", "OK");
        await BackNavigationAsync();
    });
    #endregion

    #region CancelCommand
    [RelayCommand]
    private async Task CancelAsync() => await BackNavigationAsync();
    #endregion

    #region EnsureCustomer
    private async Task EnsureCustomerAsync()
    {
        if (Customer is not null)
            return;

        var first = (await _customersService.GetListAsync(take: 1))?.Items?.FirstOrDefault();
        if (first is null)
            return;

        Customer = await _customersService.GetDetailsAsync(first.Id);
    }
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

    #region AddOrderItem
    private async Task AddOrderItemAsync()
    {
        var initialType = ProductTypeOptions.FirstOrDefault() ?? NoProductType;
        var initialProduct = await ResolveFirstProductAsync(initialType);

        Items.Add(new OrderItemViewModel(this)
        {
            ProductType = initialType,
            Product = initialProduct,
            Qty = 1
        });
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
    public partial class OrderItemViewModel : ObservableObject
    {
        private readonly OrderCreatePageViewModel _parent;

        public OrderItemViewModel(OrderCreatePageViewModel parent)
        {
            _parent = parent;
        }

        [ObservableProperty] private string? productType;
        [ObservableProperty] private ProductDetailsResponse? product;
        [ObservableProperty] private decimal qty;

        partial void OnProductTypeChanged(string? value)
        {
            _ = ChangeProductByTypeAsync(value);
        }

        private async Task ChangeProductByTypeAsync(string? type)
        {
            Product = await _parent.ResolveFirstProductAsync(type);
        }
    }
    #endregion
}
