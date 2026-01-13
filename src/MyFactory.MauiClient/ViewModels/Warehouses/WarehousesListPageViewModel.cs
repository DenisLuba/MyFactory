using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Warehouses;
using MyFactory.MauiClient.Services.Warehouses;

namespace MyFactory.MauiClient.ViewModels.Warehouses;

public partial class WarehousesListPageViewModel : ObservableObject
{
    private readonly IWarehousesService _warehousesService;
    private List<WarehouseItemViewModel> _all = new();

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<WarehouseItemViewModel> Warehouses { get; } = new();

    public WarehousesListPageViewModel(IWarehousesService warehousesService)
    {
        _warehousesService = warehousesService;
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

            var items = await _warehousesService.GetListAsync();
            _all = items?.Select(WarehouseItemViewModel.FromResponse).ToList() ?? new();
            RefreshCollection();
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
        await Shell.Current.DisplayAlertAsync("Действие", "Создание склада не реализовано", "OK");
    }

    [RelayCommand]
    private async Task EditAsync(WarehouseItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Действие", "Редактирование склада не реализовано", "OK");
    }

    [RelayCommand]
    private async Task DeleteAsync(WarehouseItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlertAsync("Действие", "Удаление склада не реализовано", "OK");
    }

    [RelayCommand]
    private async Task OpenStockAsync(WarehouseItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.GoToAsync(nameof(Pages.Warehouses.WarehouseStockPage), new Dictionary<string, object>
        {
            { "WarehouseId", item.Id.ToString() },
            { "WarehouseName", item.Name }
        });
    }

    private void RefreshCollection()
    {
        Warehouses.Clear();
        foreach (var item in _all)
            Warehouses.Add(item);
    }

    public sealed record WarehouseItemViewModel(Guid Id, string Name, string Type)
    {
        public static WarehouseItemViewModel FromResponse(WarehouseListItemResponse response)
        {
            return new WarehouseItemViewModel(response.Id, response.Name, response.Type.ToString());
        }
    }
}

