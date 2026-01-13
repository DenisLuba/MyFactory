using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Services.SalesOrders;

namespace MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

[QueryProperty(nameof(OrderIdParameter), "OrderId")]
public partial class OrderDetailsPageViewModel : ObservableObject
{
    private readonly ISalesOrdersService _salesOrdersService;

    [ObservableProperty]
    private Guid? orderId;

    [ObservableProperty]
    private string? orderIdParameter;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string orderTitle = string.Empty;

    [ObservableProperty]
    private string customerName = string.Empty;

    [ObservableProperty]
    private string orderDate = string.Empty;

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private string createdBy = "-";

    public ObservableCollection<OrderItemViewModel> Items { get; } = new();
    public ObservableCollection<ShipmentItemViewModel> Shipments { get; } = new();

    public OrderDetailsPageViewModel(ISalesOrdersService salesOrdersService)
    {
        _salesOrdersService = salesOrdersService;
        _ = LoadAsync();
    }

    partial void OnOrderIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    partial void OnOrderIdParameterChanged(string? value)
    {
        OrderId = Guid.TryParse(value, out var id) ? id : null;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (OrderId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            Items.Clear();
            Shipments.Clear();

            var details = await _salesOrdersService.GetDetailsAsync(OrderId.Value);
            if (details is not null)
            {
                OrderTitle = $"Заказ {details.OrderNumber}";
                CustomerName = details.Customer.Name;
                OrderDate = details.OrderDate.ToShortDateString();
                Status = details.Status.ToString();
                foreach (var item in details.Items)
                {
                    Items.Add(new OrderItemViewModel(item));
                }
            }

            var shipments = await _salesOrdersService.GetShipmentsAsync(OrderId.Value);
            if (shipments is not null)
            {
                foreach (var shipment in shipments)
                {
                    Shipments.Add(new ShipmentItemViewModel(shipment));
                }
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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    [RelayCommand]
    private async Task ProductionAsync()
    {
        await Shell.Current.DisplayAlertAsync("Инфо", "Переход в производство не реализован", "OK");
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await Shell.Current.DisplayAlertAsync("Инфо", "Добавление позиции не реализовано", "OK");
    }

    [RelayCommand]
    private async Task EditItemAsync(OrderItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Инфо", "Редактирование позиции не реализовано", "OK");
    }

    [RelayCommand]
    private async Task DeleteItemAsync(OrderItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Инфо", "Удаление позиции не реализовано", "OK");
    }

    [RelayCommand]
    private async Task OpenProductAsync(OrderItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Инфо", "Переход к товару не реализован", "OK");
    }

    public sealed class OrderItemViewModel
    {
        public Guid Id { get; }
        public string ProductName { get; }
        public string Quantity { get; }

        public OrderItemViewModel(SalesOrderItemResponse response)
        {
            Id = response.Id;
            ProductName = response.ProductName;
            Quantity = response.QtyOrdered.ToString();
        }
    }

    public sealed class ShipmentItemViewModel
    {
        public Guid Id { get; }
        public string ProductName { get; }
        public string ProductionOrderNumber { get; }
        public string WarehouseName { get; }
        public string Qty { get; }
        public string ShippedAt { get; }

        public ShipmentItemViewModel(SalesOrderShipmentResponse response)
        {
            Id = response.Id;
            ProductName = response.ProductName;
            ProductionOrderNumber = response.ProductionOrderNumber;
            WarehouseName = response.WarehouseName;
            Qty = response.Qty.ToString();
            ShippedAt = response.ShippedAt.ToShortDateString();
        }
    }
}

