using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.SalesOrders;
using MyFactory.MauiClient.Pages.Orders.SalesOrders;
using MyFactory.MauiClient.Services.SalesOrders;

namespace MyFactory.MauiClient.ViewModels.Orders.SalesOrders;

public partial class OrdersListPageViewModel : ObservableObject
{
    private readonly ISalesOrdersService _salesOrdersService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<OrderItemViewModel> Orders { get; } = new();

    public OrdersListPageViewModel(ISalesOrdersService salesOrdersService)
    {
        _salesOrdersService = salesOrdersService;
        _ = LoadAsync();
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
            Orders.Clear();

            var items = await _salesOrdersService.GetListAsync();
            if (items is not null)
            {
                foreach (var item in items)
                {
                    Orders.Add(new OrderItemViewModel(item));
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        await Shell.Current.DisplayAlert("Инфо", "Создание заказа не реализовано", "OK");
    }

    [RelayCommand]
    private async Task EditAsync(OrderItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlert("Инфо", "Редактирование заказа не реализовано", "OK");
    }

    [RelayCommand]
    private async Task DeleteAsync(OrderItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlert("Удалить", $"Удалить заказ {item.OrderNumber}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _salesOrdersService.DeleteAsync(item.Id);
            Orders.Remove(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(OrderItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "OrderId", item.Id }
        };

        await Shell.Current.GoToAsync(nameof(OrderDetailsPage), parameters);
    }

    public sealed class OrderItemViewModel
    {
        public Guid Id { get; }
        public string OrderNumber { get; }
        public string CustomerName { get; }
        public string OrderDate { get; }
        public string Status { get; }

        public OrderItemViewModel(SalesOrderListItemResponse response)
        {
            Id = response.Id;
            OrderNumber = response.OrderNumber;
            CustomerName = response.CustomerName;
            OrderDate = response.OrderDate.ToShortDateString();
            Status = response.Status.ToString();
        }
    }
}

