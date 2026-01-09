using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.Suppliers;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

[QueryProperty(nameof(SupplierIdParameter), "SupplierId")]
public partial class SupplierDetailsPageViewModel : ObservableObject
{
    private readonly ISuppliersService _suppliersService;

    [ObservableProperty]
    private Guid? supplierId;

    [ObservableProperty]
    private string? supplierIdParameter;

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

    [ObservableProperty]
    private SupplierPurchaseHistoryItemViewModel? selectedPurchase;

    public bool HasSelection => SelectedPurchase is not null;

    public ObservableCollection<SupplierPurchaseHistoryItemViewModel> PurchaseHistory { get; } = new();

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

    partial void OnSupplierIdParameterChanged(string? value)
    {
        SupplierId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnSelectedPurchaseChanged(SupplierPurchaseHistoryItemViewModel? value)
    {
        OnPropertyChanged(nameof(HasSelection));
    }

    [RelayCommand]
    public async Task LoadAsync()
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
            SelectedPurchase = null;

            var details = await _suppliersService.GetDetailsAsync(SupplierId.Value);
            if (details is null)
                return;

            Name = details.Name;
            Description = details.Description;
            Contacts = string.Empty;

            foreach (var item in details.Purchases.OrderByDescending(p => p.Date))
                PurchaseHistory.Add(new SupplierPurchaseHistoryItemViewModel(item));
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
            IsBusy = false;
            await LoadAsync();
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
    private async Task AddPurchaseAsync()
    {
        if (SupplierId is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderCreatePage", new Dictionary<string, object>
        {
            { "SupplierId", SupplierId.Value.ToString() }
        });
    }

    [RelayCommand]
    private async Task CreateOrderAsync()
    {
        await AddPurchaseAsync();
    }

    [RelayCommand]
    private async Task EditOrderAsync()
    {
        if (SelectedPurchase is null)
            return;

        await Shell.Current.GoToAsync("SupplierOrderUpdatePage", new Dictionary<string, object>
        {
            { "PurchaseOrderId", SelectedPurchase.OrderId.ToString() },
            { "SupplierId", SupplierId?.ToString() ?? string.Empty }
        });
    }

    [RelayCommand]
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public sealed class SupplierPurchaseHistoryItemViewModel
    {
        public Guid OrderId { get; }
        public string MaterialType { get; }
        public string MaterialName { get; }
        public decimal Qty { get; }
        public decimal UnitPrice { get; }
        public DateTime Date { get; }
        public string StatusText { get; }

        public SupplierPurchaseHistoryItemViewModel(SupplierPurchaseHistoryResponse source)
        {
            OrderId = source.OrderId;
            MaterialType = source.MaterialType;
            MaterialName = source.MaterialName;
            Qty = source.Qty;
            UnitPrice = source.UnitPrice;
            Date = source.Date;
            StatusText = source.Status switch
            {
                PurchaseOrderStatus.New => "Новый",
                PurchaseOrderStatus.Confirmed => "Подтвержден",
                PurchaseOrderStatus.Received => "Получен",
                PurchaseOrderStatus.Cancelled => "Отменен",
                _ => source.Status.ToString()
            };
        }
    }
}

