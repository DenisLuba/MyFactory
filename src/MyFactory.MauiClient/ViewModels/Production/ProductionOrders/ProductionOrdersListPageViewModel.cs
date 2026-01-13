using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.ProductionOrders;
using MyFactory.MauiClient.Pages.Production;
using MyFactory.MauiClient.Pages.Production.ProductionOrders;
using MyFactory.MauiClient.Services.ProductionOrders;

namespace MyFactory.MauiClient.ViewModels.Production.ProductionOrders;

[QueryProperty(nameof(SalesOrderId), "SalesOrderId")]
public partial class ProductionOrdersListPageViewModel : ObservableObject
{
    private readonly IProductionOrdersService _productionOrdersService;

    [ObservableProperty]
    private Guid? salesOrderId;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<ProductionOrderItemViewModel> Orders { get; } = new();

    public ProductionOrdersListPageViewModel(IProductionOrdersService productionOrdersService)
    {
        _productionOrdersService = productionOrdersService;
        _ = LoadAsync();
    }

    partial void OnSalesOrderIdChanged(Guid? value)
    {
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

            IReadOnlyList<ProductionOrderListItemResponse>? items;
            if (SalesOrderId.HasValue)
            {
                items = await _productionOrdersService.GetBySalesOrderAsync(SalesOrderId.Value);
            }
            else
            {
                items = await _productionOrdersService.GetListAsync();
            }

            foreach (var item in items ?? Array.Empty<ProductionOrderListItemResponse>())
            {
                Orders.Add(new ProductionOrderItemViewModel(item));
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
    private async Task AddAsync()
    {
        await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage));
    }

    [RelayCommand]
    private async Task OpenOrderAsync(ProductionOrderItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", item.Id.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage), parameters);
    }

    [RelayCommand]
    private async Task OpenStagesAsync(ProductionOrderItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", item.Id.ToString() },
            { "ProductionOrderNumber", item.ProductionOrderNumber },
            { "ProductInfo", item.ProductName }
        };
        await Shell.Current.GoToAsync(nameof(ProductionStagesPage), parameters);
    }

    [RelayCommand]
    private async Task DeleteAsync(ProductionOrderItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Удалить", $"Удалить ПЗ {item.ProductionOrderNumber}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _productionOrdersService.DeleteAsync(item.Id);
            Orders.Remove(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(ProductionOrderItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "ProductionOrderId", item.Id.ToString() }
        };
        await Shell.Current.GoToAsync(nameof(ProductionOrderCreatePage), parameters);
    }

    [RelayCommand]
    private async Task EditAsync(ProductionOrderItemViewModel? item)
    {
        await OpenDetailsAsync(item);
    }

    public sealed class ProductionOrderItemViewModel
    {
        public Guid Id { get; }
        public string ProductionOrderNumber { get; }
        public string SalesOrderNumber { get; }
        public string ProductName { get; }
        public int Quantity { get; }
        public int Shipped { get; }
        public string Status { get; }

        public ProductionOrderItemViewModel(ProductionOrderListItemResponse response)
        {
            Id = response.Id;
            ProductionOrderNumber = response.ProductionOrderNumber;
            SalesOrderNumber = response.SalesOrderNumber;
            ProductName = response.ProductName;
            Quantity = response.QtyPlanned;
            Shipped = response.QtyFinished;
            Status = response.Status.ToString();
        }
    }
}

