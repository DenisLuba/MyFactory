using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Common;
using MyFactory.MauiClient.Models.Customers;
using MyFactory.MauiClient.Pages.Orders.Customers;
using MyFactory.MauiClient.Services.Customers;
using System;
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Orders.Customers;

public partial class CustomerDetailsPageViewModel(ICustomersService customersService) : ObservableObject, IQueryAttributable
{
    #region Observable Properties
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

    [ObservableProperty]
    private bool isPhoneVisible;

    [ObservableProperty]
    private bool isEmailVisible;

    [ObservableProperty]
    private bool isAddressVisible;

    [ObservableProperty]
    private bool isHistoryVisible;
    #endregion

    #region Properties
    public bool IsReadOnly => !IsEditMode;

    public bool IsReadyToSave => IsEditMode && !string.IsNullOrWhiteSpace(Name);
    #endregion

    #region ObservableCollections
    public ObservableCollection<CustomerOrderItemViewModel> Orders { get; } = [];
    #endregion

    #region On Changed
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
    #endregion

    #region ApplyQueryAttributes
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
    #endregion

    #region LoadCommand
    [RelayCommand]
    public async Task LoadAsync() => await RunSafeActionAsync(LoadMethodAsync);
    #endregion

    #region LoadMethod
    private async Task LoadMethodAsync()
    {
        if (IsEditMode)
        {
            IsPhoneVisible = IsEmailVisible = IsAddressVisible = true;
            IsHistoryVisible = false;
        }

        if (CustomerId is null)
            return;

        Orders.Clear();

        var card = await customersService.GetCardAsync(CustomerId.Value);

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

            if (IsReadOnly)
            {
                IsPhoneVisible = Phone is not null;
                IsEmailVisible = Email is not null;
                IsAddressVisible = Address is not null;

                IsHistoryVisible = card.Orders.Count > 0;
            }
        }
    }
    #endregion

    #region SaveCommand
    [RelayCommand]
    private async Task SaveAsync() => await RunSafeActionAsync(async () =>
    {

        #region Guard
        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlertAsync("Ошибка!", "Имя клиента не может быть пустым.", "OK");
            return;
        }

        if (!string.IsNullOrWhiteSpace(Phone))
        {
            var digitsOnly = Regex.Replace(Phone, "[^0-9]", string.Empty);
            if (string.IsNullOrWhiteSpace(digitsOnly))
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", "Номер телефона должен содержать только цифры.", "OK");
                return;
            }

            Phone = digitsOnly;
        }

        if (!string.IsNullOrWhiteSpace(Email))
        {
            try
            {
                var mail = new MailAddress(Email);
                Email = mail.Address;
            }
            catch
            {
                await Shell.Current.DisplayAlertAsync("Ошибка!", "Некорректный адрес электронной почты.", "OK");
                return;
            }
        }
        #endregion

        if (IsEditMode && CustomerId is not null)
        {
            var request = new UpdateCustomerRequest
            (
                Name: Name,
                Phone: Phone,
                Email: Email,
                Address: Address
            );

            await customersService.UpdateAsync(CustomerId.Value, request);
            await Shell.Current.DisplayAlertAsync("Успех!", "Данные клиента успешно обновлены.", "OK");
            await BackExtensionAsync();
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

            await customersService.CreateAsync(request);
            await Shell.Current.DisplayAlertAsync("Успех!", "Клиент успешно создан.", "OK");
            await BackExtensionAsync();
        }

    });
    #endregion

    #region EditCommand
    [RelayCommand]
    private async Task EditAsync() => await RunSafeActionAsync(async () =>
    {
        IsEditMode = true;
        await LoadMethodAsync();
    });
    #endregion

    #region BackCommand
    [RelayCommand]
    private async Task BackAsync() => await BackExtensionAsync();
    #endregion

    #region BackExtension
    private async Task BackExtensionAsync() =>
        await Shell.Current.BackSafeAsync(fallbackRoute: nameof(CustomersListPage), animate: true);
    #endregion

    #region RUN SAFE ACTION
    protected async Task RunSafeActionAsync(Func<Task> action)
    {
        await action.RunSafeAsync(
            getIsBusy: () => IsBusy,
            setIsBusy: (value) => IsBusy = value,
            setError: (message) => ErrorMessage = message,
            showError: async (message) => await Shell.Current.DisplayAlertAsync("Ошибка!", message, "OK"));
    }
    #endregion

    #region CustomerOrderItemViewModel
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
    #endregion
}
