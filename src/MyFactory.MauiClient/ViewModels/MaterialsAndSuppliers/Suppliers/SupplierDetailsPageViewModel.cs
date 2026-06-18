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

    public ObservableCollection<string> StatusOptions { get; } = new();

    [ObservableProperty]
    private string? selectedStatus;

    [ObservableProperty]
    private DateTime? fromDateFilter;

    [ObservableProperty]
    private DateTime? toDateFilter;

    [ObservableProperty]
    private SupplierPurchaseHistoryViewModel? selectedPurchase;

    public bool HasSelection => SelectedPurchase != null
        && (SelectedPurchase.Status == PurchaseOrderStatus.New || SelectedPurchase.Status == PurchaseOrderStatus.Confirmed);

    public bool HasSelectionForDetails => SelectedPurchase != null && !HasSelection;

    public ObservableCollection<SupplierPurchaseHistoryViewModel> PurchaseHistory { get; } = new();

    private List<SupplierPurchaseHistoryViewModel> _allPurchases = new();

    public static readonly string[] StatusList =
    [
        "Все",
        "Новое",
        "Завершено",
        "Получено",
        "Отменено"
    ];

    public SupplierDetailsPageViewModel(ISuppliersService suppliersService, IMaterialPurchaseOrdersService materialPurchaseOrdersService)
    {
        _suppliersService = suppliersService;
        _materialPurchaseOrdersService = materialPurchaseOrdersService;
        Contacts = string.Empty;
        StatusOptions.Add(StatusList[0]);
        StatusOptions.Add(StatusList[1]);
        StatusOptions.Add(StatusList[2]);
        StatusOptions.Add(StatusList[3]);
        StatusOptions.Add(StatusList[4]);
    }

    partial void OnSupplierIdChanged(Guid? value)
    {
        if (!IsBusy)
            _ = LoadAsync();
    }

    partial void OnSupplierIdParameterChanged(string? value)
    {
        SupplierId = Guid.TryParse(value, out var id) ? id : null;
    }

    partial void OnSelectedPurchaseChanged(SupplierPurchaseHistoryViewModel? value)
    {
        OnPropertyChanged(nameof(HasSelection));
        OnPropertyChanged(nameof(HasSelectionForDetails));
    }

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
                _allPurchases.Add(new SupplierPurchaseHistoryViewModel(item));

            ApplyFilter();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("������", ex.Message, "OK");
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

        var newName = await Shell.Current.DisplayPromptAsync("�������������", "������� ����� ��������", initialValue: Name ?? string.Empty);
        if (string.IsNullOrWhiteSpace(newName))
            return;

        var newDescription = await Shell.Current.DisplayPromptAsync("��������", "������� ��������", initialValue: Description ?? string.Empty);

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
            await Shell.Current.DisplayAlertAsync("������", ex.Message, "OK");
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
        var confirm = await Shell.Current.DisplayAlertAsync("����������� ��������", "�� �������, ��� ������ ������� ���� �����?", "��", "���");
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
            await Shell.Current.DisplayAlertAsync("������", ex.Message, "OK");
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

        var statusFilter = SelectedStatus?.Trim();
        if (string.Equals(statusFilter, "���", StringComparison.InvariantCultureIgnoreCase))
            statusFilter = null;
        var fromDate = FromDateFilter?.Date;
        var toDate = ToDateFilter?.Date;

        foreach (var item in _allPurchases)
        {
            if (!MatchesFilter(item, statusFilter, fromDate, toDate))
                continue;

            PurchaseHistory.Add(item);
        }
    }

    private static bool MatchesFilter(
        SupplierPurchaseHistoryViewModel item,
        string? statusFilter,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var comparison = StringComparison.InvariantCultureIgnoreCase;

        if (!string.IsNullOrWhiteSpace(statusFilter) && !string.Equals(item.StatusText, statusFilter, comparison))
            return false;

        if (fromDate.HasValue && item.Date.Date < fromDate.Value)
            return false;

        if (toDate.HasValue && item.Date.Date > toDate.Value)
            return false;

        return true;
    }

    public sealed class SupplierPurchaseHistoryViewModel
    {
        public Guid OrderId { get; }
        public decimal PurchaseNumber { get; }
        public string PurchaseNumberText => PurchaseNumber.ToString("0");
        public DateTime Date { get; }
        public PurchaseOrderStatus Status { get; }
        public string StatusText { get; }

        public SupplierPurchaseHistoryViewModel(SupplierPurchaseHistoryResponse source)
        {
            OrderId = source.OrderId;
            PurchaseNumber = source.PurchaseNumber;
            Date = source.Date;
            Status = source.Status;

            StatusText = source.Status switch
            {
                PurchaseOrderStatus.New => StatusList[1],
                PurchaseOrderStatus.Confirmed => StatusList[2],
                PurchaseOrderStatus.Received => StatusList[3],
                PurchaseOrderStatus.Cancelled => StatusList[4],
                _ => source.Status.ToString()
            };
        }
    }

    public sealed class SupplierPurchaseHistoryItemViewModel
    {
        public string MaterialType { get; }
        public string MaterialName { get; }
        public decimal Qty { get; }
        public decimal UnitPrice { get; }

        public SupplierPurchaseHistoryItemViewModel(SupplierPurchaseHistoryItemResponse source)
        {
            MaterialType = source.MaterialType;
            MaterialName = source.MaterialName;
            Qty = source.Qty;
            UnitPrice = source.UnitPrice;
        }
    }
}