using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

[QueryProperty(nameof(SupplierId), "SupplierId")]
public partial class SupplierDetailsPageViewModel : ObservableObject
{
    private readonly ISuppliersService _suppliersService;

    [ObservableProperty]
    private Guid? supplierId;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? description;

    [ObservableProperty]
    private string? contacts;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<SupplierPurchaseHistoryResponse> PurchaseHistory { get; } = new();

    public SupplierDetailsPageViewModel(ISuppliersService suppliersService)
    {
        _suppliersService = suppliersService;
        Contacts = string.Empty;
        _ = LoadAsync();
    }

    partial void OnSupplierIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
            return;

        if (SupplierId is null)
            return;

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            PurchaseHistory.Clear();

            var details = await _suppliersService.GetDetailsAsync(SupplierId.Value);
            if (details is null)
                return;

            Name = details.Name;
            Description = details.Description;
            Contacts = string.Empty;

            foreach (var item in details.Purchases.OrderByDescending(p => p.Date))
                PurchaseHistory.Add(item);
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
    private async Task EditAsync()
    {
        if (SupplierId is null)
            return;

        var newName = await Shell.Current.DisplayPromptAsync("Редактировать", "Введите новое название", initialValue: Name ?? string.Empty);
        if (string.IsNullOrWhiteSpace(newName))
            return;

        var newDescription = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание", initialValue: Description ?? string.Empty);

        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _suppliersService.UpdateAsync(SupplierId.Value, new UpdateSupplierRequest(newName.Trim(), string.IsNullOrWhiteSpace(newDescription) ? null : newDescription.Trim()));
            await LoadAsync();
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
    private async Task AddPurchaseAsync()
    {
        if (SupplierId is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderCreatePage", new Dictionary<string, object>
        {
            { "SupplierId", SupplierId.Value }
        });
    }

    [RelayCommand]
    private async Task CreateOrderAsync()
    {
        await AddPurchaseAsync();
    }
}

