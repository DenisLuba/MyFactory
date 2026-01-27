using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Services.Customers;

namespace MyFactory.MauiClient.ViewModels.Orders.Customers;

public partial class CustomerDetailsPageViewModel : ObservableObject, IQueryAttributable
{
    private readonly ICustomersService _customersService;

    [ObservableProperty]
    private Guid? customerId;

    [ObservableProperty]
    private string? customerIdParameter;

    [ObservableProperty]
    private bool isEditMode;

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

    public bool IsReadOnly => !IsEditMode;

    public bool IsReadyToSave => IsEditMode && !string.IsNullOrWhiteSpace(Name);

    public ObservableCollection<CustomerOrderItemViewModel> Orders { get; } = new();

    public CustomerDetailsPageViewModel(ICustomersService customersService)
    {
        _customersService = customersService;
    }

    partial void OnCustomerIdChanged(Guid? value)
    {
        if (!IsBusy)
            _ = LoadAsync();
    }

    partial void OnNameChanged(string value)
    {
        if (IsEditMode)
            OnPropertyChanged(nameof(IsReadyToSave));
    }

    partial void OnPhoneChanged(string? value)
    {
        if (IsEditMode)
            OnPropertyChanged(nameof(IsReadyToSave));
    }

    partial void OnEmailChanged(string? value)
    {
        if (IsEditMode)
            OnPropertyChanged(nameof(IsReadyToSave));
    }

    partial void OnAddressChanged(string? value)
    {
        if (IsEditMode)
            OnPropertyChanged(nameof(IsReadyToSave));
    }

    partial void OnIsEditModeChanged(bool value)
    {
        OnPropertyChanged(nameof(IsReadOnly));
    }

    partial void OnCustomerIdParameterChanged(string? value)
    {
        CustomerId = Guid.TryParse(value, out var id) ? id : null;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("CustomerId", out var customerIdObj) && customerIdObj is string customerIdStr)
        {
            CustomerIdParameter = customerIdStr;
        }

        if (query.TryGetValue("IsEditMode", out var isEditModeObj) && isEditModeObj is string isEditModeStr)
        {
            IsEditMode = bool.TryParse(isEditModeStr, out var isEdit) && isEdit;
        }

        if (isEditModeObj is bool isEditModeBool)
        {
            IsEditMode = isEditModeBool;
        }
    }

    [RelayCommand]
    public async Task LoadAsync()
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
            await Shell.Current.DisplayAlertAsync("Ошибка!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Имя клиента не может быть пустым.", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            if (IsEditMode && CustomerId is not null)
            {
                var request = new UpdateCustomerRequest
                (
                    Name: Name,
                    Phone: Phone,
                    Email: Email,
                    Address: Address
                );

                await _customersService.UpdateAsync(CustomerId.Value, request);
                await Shell.Current.DisplayAlertAsync("Успех!", "Данные клиента успешно обновлены.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else if (IsEditMode && CustomerId is null)
            {
                var request = new CreateCustomerRequest
                (
                    Name: Name,
                    Phone: Phone,
                    Email: Email,
                    Address: Address
                );

                await _customersService.CreateAsync(request);
                await Shell.Current.DisplayAlertAsync("Успех!", "Клиент успешно создан.", "OK");
                await Shell.Current.GoToAsync("..");
            }
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
    private async Task EditAsync()
    {
        IsEditMode = true;
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
