using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Services.Customers;

namespace MyFactory.MauiClient.ViewModels.Orders.Customers;

[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class CustomerDetailsPageViewModel : ObservableObject
{
    private readonly ICustomersService _customersService;

    [ObservableProperty]
    private Guid? customerId;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string? phone;

    [ObservableProperty]
    private string? email;

    [ObservableProperty]
    private string? address;

    public ObservableCollection<CustomerOrderItemViewModel> Orders { get; } = new();

    public CustomerDetailsPageViewModel(ICustomersService customersService)
    {
        _customersService = customersService;
        _ = LoadAsync();
    }

    partial void OnCustomerIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (CustomerId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            Orders.Clear();

            var card = await _customersService.GetCardAsync(CustomerId.Value);
            if (card is not null)
            {
                Name = card.Name;
                Phone = card.Phone;
                Email = card.Email;
                Address = card.Address;

                foreach (var order in card.Orders)
                {
                    Orders.Add(new CustomerOrderItemViewModel(order));
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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..", true);
    }

    public sealed class CustomerOrderItemViewModel
    {
        public Guid Id { get; }
        public string OrderNumber { get; }
        public string OrderDate { get; }
        public string Status { get; }

        public CustomerOrderItemViewModel(CustomerOrderItemResponse response)
        {
            Id = response.Id;
            OrderNumber = response.OrderNumber;
            OrderDate = response.OrderDate.ToShortDateString();
            Status = response.Status.ToString();
        }
    }
}

