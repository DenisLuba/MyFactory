using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

    // [ObservableProperty]
    public ObservableCollection<WarehouseItemViewModel> Warehouses { get; } = new();

    public WarehousesListPageViewModel(IWarehousesService warehousesService)
    {
        _warehousesService = warehousesService;
        _ = LoadAsync();
    }
    
    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            var items = await _warehousesService.GetListAsync(includeInactive: true);
            _all = items?.Select(WarehouseItemViewModel.FromResponse).ToList() ?? new();
            RefreshCollection();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        if (IsBusy)
            return;

        var name = await Shell.Current.DisplayPromptAsync("Новый склад", "Введите название склада", initialValue: string.Empty, maxLength: 200);
        if (string.IsNullOrWhiteSpace(name))
            return;

        var typeOptions = Enum.GetValues<WarehouseType>().Select(t => t.ToString()).ToArray();
        var selectedType = await Shell.Current.DisplayActionSheetAsync("Тип склада", "Отмена", null, typeOptions);
        if (string.IsNullOrWhiteSpace(selectedType) || selectedType == "Отмена")
            return;

        if (!Enum.TryParse<WarehouseType>(selectedType, out var type))
            return;

        try
        {
            IsBusy = true;
            var createResponse = await _warehousesService.CreateAsync(new CreateWarehouseRequest(name.Trim(), type))
                ?? throw new InvalidOperationException("Не удалось создать склад");

            IsBusy = false;

            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditAsync(WarehouseItemViewModel? item)
    {
        if (item is null)
            return;

        if (IsBusy)
            return;

        var name = await Shell.Current.DisplayPromptAsync("Редактировать склад", "Название", initialValue: item.Name, maxLength: 200);
        if (string.IsNullOrWhiteSpace(name))
            return;

        var typeOptions = Enum.GetValues<WarehouseType>().Select(t => t.ToString()).ToArray();
        var selectedType = await Shell.Current.DisplayActionSheetAsync("Тип склада", "Отмена", null, typeOptions);
        if (string.IsNullOrWhiteSpace(selectedType) || selectedType == "Отмена")
            return;

        if (!Enum.TryParse<WarehouseType>(selectedType, out var type))
            return;

        try
        {
            IsBusy = true;
            await _warehousesService.UpdateAsync(item.Id, new UpdateWarehouseRequest(name.Trim(), type));
            IsBusy = false;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ActivateAsync(WarehouseItemViewModel? item)
    {
        if (item is null)
            return;

        if (IsBusy)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Активировать", $"Активировать склад '{item.Name}'?", "Да", "Нет");

        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            await _warehousesService.ActivateAsync(item.Id);
            IsBusy = false;
            await LoadAsync();
        }
        catch(Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task DeactivateAsync(WarehouseItemViewModel? item)
    {
        if (item is null)
            return;

        if (IsBusy)
            return;

        var confirm = await Shell.Current.DisplayAlertAsync("Деактивировать", $"Деактивировать склад '{item.Name}'?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            IsBusy = true;
            await _warehousesService.DeactivateAsync(item.Id);
            IsBusy = false;
            await LoadAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
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

    public sealed record WarehouseItemViewModel(Guid Id, string Name, string Type, bool IsActive)
    {
        public string Status => IsActive ? "Активен" : "Неактивен";
        public string ActionLabel => IsActive ? "Деактивировать" : "Активировать";
        public bool IsInactive => !IsActive;

        public static WarehouseItemViewModel FromResponse(WarehouseListItemResponse response)
        {
            return new WarehouseItemViewModel(response.Id, response.Name, response.Type.ToString(), response.IsActive);
        }
    }
}

