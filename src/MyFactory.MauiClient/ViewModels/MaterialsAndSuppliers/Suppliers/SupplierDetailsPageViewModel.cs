using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.MaterialPurchaseOrders;
using MyFactory.MauiClient.Models.Suppliers;
using MyFactory.MauiClient.Services.MaterialPurchaseOrders;
using MyFactory.MauiClient.Services.Suppliers;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MyFactory.MauiClient.ViewModels.MaterialsAndSuppliers.Suppliers;

[QueryProperty(nameof(SupplierIdParameter), "SupplierId")]
public partial class SupplierDetailsPageViewModel : ObservableObject
{
    private readonly ISuppliersService _suppliersService;
    private readonly IMaterialPurchaseOrdersService _materialPurchaseOrdersService;

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
    private string? materialTypeFilter;

    [ObservableProperty]
    private string? materialNameFilter;

    public ObservableCollection<string> StatusOptions { get; } = new();

    [ObservableProperty]
    private string? selectedStatus;

    [ObservableProperty]
    private DateTime? fromDateFilter;

    [ObservableProperty]
    private DateTime? toDateFilter;

    [ObservableProperty]
    private SupplierPurchaseHistoryItemViewModel? selectedPurchase;

    public bool HasSelection => SelectedPurchase is not null;

    public ObservableCollection<SupplierPurchaseHistoryItemViewModel> PurchaseHistory { get; } = new();

    private List<SupplierPurchaseHistoryItemViewModel> _allPurchases = new();

    public SupplierDetailsPageViewModel(ISuppliersService suppliersService, IMaterialPurchaseOrdersService materialPurchaseOrdersService)
    {
        _suppliersService = suppliersService;
        _materialPurchaseOrdersService = materialPurchaseOrdersService;
        Contacts = string.Empty;
        StatusOptions.Add("Все");
        StatusOptions.Add("Новый");
        StatusOptions.Add("Подтвержден");
        StatusOptions.Add("Получен");
        StatusOptions.Add("Отменен");
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

    partial void OnMaterialTypeFilterChanged(string? value) => ApplyFilter();

    partial void OnMaterialNameFilterChanged(string? value) => ApplyFilter();

    partial void OnSelectedStatusChanged(string? value) => ApplyFilter();

    partial void OnFromDateFilterChanged(DateTime? value) => ApplyFilter();

    partial void OnToDateFilterChanged(DateTime? value) => ApplyFilter();

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
            _allPurchases.Clear();
            SelectedPurchase = null;
            SelectedStatus = StatusOptions.FirstOrDefault();

            var details = await _suppliersService.GetDetailsAsync(SupplierId.Value);
            if (details is null)
                return;

            Name = details.Name;
            Description = details.Description;
            Contacts = string.Empty;

            foreach (var item in details.Purchases.OrderByDescending(p => p.Date))
                _allPurchases.Add(new SupplierPurchaseHistoryItemViewModel(item));

            ApplyFilter();
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
    private async Task DeleteOrderAsync()
    {
        if (SelectedPurchase is null)
            return;
        var confirm = await Shell.Current.DisplayAlert("Подтвердите удаление", "Вы уверены, что хотите удалить этот заказ?", "Да", "Нет");
        if (!confirm)
            return;
        try
        {
            IsBusy = true;
            ErrorMessage = null;

            await _materialPurchaseOrdersService.CancelAsync(SelectedPurchase.OrderId);

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
    private async Task BackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private void ApplyFilter()
    {
        PurchaseHistory.Clear();

        var typeFilter = MaterialTypeFilter?.Trim();
        var nameFilter = MaterialNameFilter?.Trim();
        var statusFilter = SelectedStatus?.Trim();
        if (string.Equals(statusFilter, "Все", StringComparison.InvariantCultureIgnoreCase))
            statusFilter = null;
        var fromDate = FromDateFilter?.Date;
        var toDate = ToDateFilter?.Date;

        foreach (var item in _allPurchases)
        {
            if (!MatchesFilter(item, typeFilter, nameFilter, statusFilter, fromDate, toDate))
                continue;

            PurchaseHistory.Add(item);
        }
    }

    private static bool MatchesFilter(
        SupplierPurchaseHistoryItemViewModel item,
        string? typeFilter,
        string? nameFilter,
        string? statusFilter,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var comparison = StringComparison.InvariantCultureIgnoreCase;

        if (!string.IsNullOrWhiteSpace(typeFilter) && item.MaterialType?.Contains(typeFilter, comparison) != true)
            return false;

        if (!string.IsNullOrWhiteSpace(nameFilter) && item.MaterialName?.Contains(nameFilter, comparison) != true)
            return false;

        if (!string.IsNullOrWhiteSpace(statusFilter) && !string.Equals(item.StatusText, statusFilter, comparison))
            return false;

        if (fromDate.HasValue && item.Date.Date < fromDate.Value)
            return false;

        if (toDate.HasValue && item.Date.Date > toDate.Value)
            return false;

        return true;
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

