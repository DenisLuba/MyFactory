using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.Warehouses;

namespace MyFactory.MauiClient.ViewModels.Warehouses;

[QueryProperty(nameof(WarehouseId), "WarehouseId")]
[QueryProperty(nameof(WarehouseName), "WarehouseName")]
public partial class WarehouseStockPageViewModel : ObservableObject
{
    private readonly IWarehousesService _warehousesService;

    [ObservableProperty]
    private Guid? warehouseId;

    [ObservableProperty]
    private string? warehouseName;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<StockItemViewModel> StockItems { get; } = new();

    public WarehouseStockPageViewModel(IWarehousesService warehousesService)
    {
        _warehousesService = warehousesService;
        _ = LoadAsync();
    }

    partial void OnWarehouseIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (WarehouseId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            StockItems.Clear();

            var info = await _warehousesService.GetInfoAsync(WarehouseId.Value);
            if (info is not null)
            {
                WarehouseName = info.Name;
            }

            var items = await _warehousesService.GetStockAsync(WarehouseId.Value);
            foreach (var item in items ?? Enumerable.Empty<WarehouseStockItemResponse>())
            {
                StockItems.Add(new StockItemViewModel(item));
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
    private async Task AddItemAsync()
    {
        await Shell.Current.DisplayAlertAsync("Действие", "Добавление позиции не реализовано", "OK");
    }

    [RelayCommand]
    private async Task TransferCommandAsync()
    {
        await Shell.Current.DisplayAlertAsync("Действие", "Перемещение не реализовано", "OK");
    }

    [RelayCommand]
    private async Task EditItemAsync(StockItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Действие", "Редактирование количества не реализовано", "OK");
    }

    [RelayCommand]
    private async Task DeleteItemAsync(StockItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Действие", "Удаление позиции не реализовано", "OK");
    }

    [RelayCommand]
    private async Task OpenItemAsync(StockItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Действие", "Открытие карточки не реализовано", "OK");
    }

    public sealed class StockItemViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Quantity { get; }

        public StockItemViewModel(WarehouseStockItemResponse response)
        {
            Id = response.ItemId;
            Name = response.Name;
            Quantity = response.UnitCode is string unit && !string.IsNullOrWhiteSpace(unit)
                ? $"{response.Qty} {unit}"
                : response.Qty.ToString();
        }
    }
}

