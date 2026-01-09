using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Pages.Orders.Customers;
using MyFactory.MauiClient.Services.Customers;

namespace MyFactory.MauiClient.ViewModels.Orders.Customers;

public partial class CustomersListPageViewModel : ObservableObject
{
    private readonly ICustomersService _customersService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<CustomerItemViewModel> Customers { get; } = new();

    public CustomersListPageViewModel(ICustomersService customersService)
    {
        _customersService = customersService;
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
            Customers.Clear();

            var items = await _customersService.GetListAsync();
            if (items is not null)
            {
                foreach (var item in items)
                {
                    Customers.Add(new CustomerItemViewModel(item));
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
        await Shell.Current.DisplayAlert("Инфо", "Добавление клиента не реализовано", "OK");
    }

    [RelayCommand]
    private async Task EditAsync(CustomerItemViewModel? item)
    {
        if (item is null)
            return;

        await Shell.Current.DisplayAlert("Инфо", "Редактирование клиента не реализовано", "OK");
    }

    [RelayCommand]
    private async Task DeleteAsync(CustomerItemViewModel? item)
    {
        if (item is null)
            return;

        var confirm = await Shell.Current.DisplayAlert("Удалить", $"Деактивировать клиента {item.Name}?", "Да", "Нет");
        if (!confirm)
            return;

        try
        {
            await _customersService.DeactivateAsync(item.Id);
            Customers.Remove(item);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(CustomerItemViewModel? item)
    {
        if (item is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "CustomerId", item.Id.ToString() }
        };

        await Shell.Current.GoToAsync(nameof(CustomerDetailsPage), parameters);
    }

    public sealed class CustomerItemViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public string? Phone { get; }
        public string? Email { get; }
        public string? Address { get; }

        public CustomerItemViewModel(CustomerListItemResponse response)
        {
            Id = response.Id;
            Name = response.Name;
            Phone = response.Phone;
            Email = response.Email;
            Address = response.Address;
        }
    }
}

