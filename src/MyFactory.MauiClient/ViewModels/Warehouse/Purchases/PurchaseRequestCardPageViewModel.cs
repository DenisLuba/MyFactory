using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Purchases;
using MyFactory.MauiClient.Pages.Warehouse.Purchases;
using MyFactory.MauiClient.Services.SuppliersServices;
using MyFactory.MauiClient.Services.PurchasesServices;
using MyFactory.MauiClient.UIModels.Warehouse;

namespace MyFactory.MauiClient.ViewModels.Warehouse.Purchases;

public partial class PurchaseRequestCardPageViewModel : ObservableObject
{
    private readonly IPurchasesService _purchasesService;
    private readonly ISuppliersService _suppliersService;
    private readonly IServiceProvider _serviceProvider;
    private Guid? _purchaseId;
    private bool _isInitialized;
    private bool _suppliersLoaded;

    public PurchaseRequestCardPageViewModel(IPurchasesService purchasesService, ISuppliersService suppliersService, IServiceProvider serviceProvider)
    {
        _purchasesService = purchasesService;
        _suppliersService = suppliersService;
        _serviceProvider = serviceProvider;
        Lines.CollectionChanged += OnLinesCollectionChanged;

        LoadCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
        RefreshCommand = new AsyncRelayCommand(LoadAsync, CanLoad);
        SaveCommand = new AsyncRelayCommand(SaveAsync, () => !IsBusy);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, () => !IsBusy && _purchaseId.HasValue);
        ConvertToOrderCommand = new AsyncRelayCommand(ConvertToOrderAsync, () => !IsBusy && _purchaseId.HasValue);
        AddLineCommand = new AsyncRelayCommand(AddLineAsync, () => !IsBusy);
        EditLineCommand = new AsyncRelayCommand<PurchaseRequestLineItem?>(EditLineAsync);
        DeleteLineCommand = new AsyncRelayCommand<PurchaseRequestLineItem?>(DeleteLineAsync);
    }

    public ObservableCollection<PurchaseRequestLineItem> Lines { get; } = new();
    public ObservableCollection<SupplierLookupItem> Suppliers { get; } = new();

    [ObservableProperty]
    private string documentNumber = string.Empty;

    [ObservableProperty]
    private DateTime createdAt = DateTime.Today;

    [ObservableProperty]
    private string warehouseName = string.Empty;

    [ObservableProperty]
    private SupplierLookupItem? selectedSupplier;

    [ObservableProperty]
    private string? comment;

    [ObservableProperty]
    private PurchasesStatus status = PurchasesStatus.Draft;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool hasLines;

    [ObservableProperty]
    private decimal totalAmount;

    public string Title => _purchaseId.HasValue ? $"Ведомость {DocumentNumber}" : "Новая ведомость";
    public bool IsNew => !_purchaseId.HasValue;
    public bool HasNoLines => !HasLines;
    public string TotalAmountDisplay => TotalAmount.ToString("N2", CultureInfo.CurrentCulture) + " ₽";
    public string StatusDisplay => Status switch
    {
        PurchasesStatus.Draft => "Черновик",
        PurchasesStatus.Created => "Создано",
        PurchasesStatus.Converted => "Преобразовано",
        PurchasesStatus.ConvertedToOrder => "Заказ создан",
        _ => Status.ToString()
    };

    public bool CanConvert => _purchaseId.HasValue && Lines.Count > 0 && Status != PurchasesStatus.ConvertedToOrder;

    public IAsyncRelayCommand LoadCommand { get; }
    public IAsyncRelayCommand RefreshCommand { get; }
    public IAsyncRelayCommand SaveCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public IAsyncRelayCommand ConvertToOrderCommand { get; }
    public IAsyncRelayCommand AddLineCommand { get; }
    public IAsyncRelayCommand<PurchaseRequestLineItem?> EditLineCommand { get; }
    public IAsyncRelayCommand<PurchaseRequestLineItem?> DeleteLineCommand { get; }

    public void Initialize(Guid? purchaseId = null)
    {
        _purchaseId = purchaseId;
        _isInitialized = true;
        if (!_purchaseId.HasValue)
        {
            PrepareNewDraft();
        }

        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(IsNew));
        RaiseCanExecute();
    }

    partial void OnDocumentNumberChanged(string value) => OnPropertyChanged(nameof(Title));

    partial void OnIsBusyChanged(bool value) => RaiseCanExecute();

    partial void OnHasLinesChanged(bool value)
    {
        OnPropertyChanged(nameof(HasNoLines));
        OnPropertyChanged(nameof(CanConvert));
    }

    partial void OnStatusChanged(PurchasesStatus value)
    {
        OnPropertyChanged(nameof(StatusDisplay));
        OnPropertyChanged(nameof(CanConvert));
    }

    partial void OnTotalAmountChanged(decimal value) => OnPropertyChanged(nameof(TotalAmountDisplay));

    private void RaiseCanExecute()
    {
        LoadCommand.NotifyCanExecuteChanged();
        RefreshCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
        DeleteCommand.NotifyCanExecuteChanged();
        ConvertToOrderCommand.NotifyCanExecuteChanged();
        AddLineCommand.NotifyCanExecuteChanged();
    }

    private void PrepareNewDraft()
    {
        DocumentNumber = $"REQ-{DateTime.Now:yyyyMMdd-HHmm}";
        CreatedAt = DateTime.Today;
        WarehouseName = string.Empty;
        SelectedSupplier = null;
        Comment = string.Empty;
        Status = PurchasesStatus.Draft;
        Lines.Clear();
        HasLines = false;
        TotalAmount = 0;
    }

    private void OnLinesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => UpdateAggregates();

    private void UpdateAggregates()
    {
        TotalAmount = Lines.Sum(l => l.TotalAmount);
        HasLines = Lines.Count > 0;
    }

    private bool CanLoad() => _isInitialized && !IsBusy;

    private async Task LoadAsync()
    {
        if (!CanLoad())
        {
            return;
        }

        try
        {
            IsBusy = true;
            await EnsureLookupsLoadedAsync();
            if (!_purchaseId.HasValue)
            {
                PrepareNewDraft();
                return;
            }

            var detail = await _purchasesService.GetPurchaseRequestAsync(_purchaseId.Value);
            if (detail is null)
            {
                await Shell.Current.DisplayAlertAsync("Ведомость", "Документ не найден", "OK");
                return;
            }

            DocumentNumber = detail.DocumentNumber;
            CreatedAt = detail.CreatedAt;
            WarehouseName = detail.WarehouseName;
            SelectedSupplier = detail.SupplierId is null
                ? null
                : Suppliers.FirstOrDefault(s => s.SupplierId == detail.SupplierId);
            Comment = detail.Comment;
            Status = detail.Status;

            Lines.Clear();
            foreach (var line in detail.Items.OrderBy(l => l.MaterialName))
            {
                Lines.Add(new PurchaseRequestLineItem(
                    line.LineId,
                    line.MaterialId,
                    line.MaterialName,
                    line.Quantity,
                    line.Unit,
                    line.Price,
                    line.TotalAmount,
                    line.Note));
            }

            TotalAmount = detail.TotalAmount;
            HasLines = Lines.Count > 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", $"Не удалось загрузить документ: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SaveAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var request = await BuildRequestAsync();
        if (request is null)
        {
            return;
        }

        var shouldReload = false;
        try
        {
            IsBusy = true;
            if (_purchaseId.HasValue)
            {
                var response = await _purchasesService.UpdatePurchaseAsync(_purchaseId.Value, request);
                if (response is not null)
                {
                    Status = response.Status;
                }
            }
            else
            {
                var response = await _purchasesService.CreatePurchaseAsync(request);
                if (response is not null)
                {
                    _purchaseId = response.PurchaseId;
                    Status = response.Status;
                    OnPropertyChanged(nameof(IsNew));
                    RaiseCanExecute();
                }
            }
            shouldReload = _purchaseId.HasValue;

            await Shell.Current.DisplayAlertAsync("Ведомость", "Документ сохранён.", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", $"Не удалось сохранить документ: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }

        if (shouldReload)
        {
            await LoadAsync();
        }
    }

    private async Task DeleteAsync()
    {
        if (!_purchaseId.HasValue || IsBusy)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить {DocumentNumber}?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await _purchasesService.DeletePurchaseAsync(_purchaseId.Value);
            await Shell.Current.Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", $"Не удалось удалить документ: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ConvertToOrderAsync()
    {
        if (!_purchaseId.HasValue || IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var response = await _purchasesService.ConvertToOrderAsync(_purchaseId.Value);
            if (response is not null)
            {
                Status = response.Status;
                await Shell.Current.DisplayAlertAsync("Ведомость", "Создан заказ на закупку.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", $"Не удалось преобразовать документ: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddLineAsync()
    {
        if (IsBusy)
        {
            return;
        }

        var result = await OpenLineEditorAsync(null);
        if (result?.Item is null)
        {
            return;
        }

        var lineId = result.LineId ?? Guid.NewGuid();
        Lines.Add(ToLineItem(lineId, result.Item));
    }

    private async Task EditLineAsync(PurchaseRequestLineItem? line)
    {
        if (line is null || IsBusy)
        {
            return;
        }

        var result = await OpenLineEditorAsync(line);
        if (result?.Item is null)
        {
            return;
        }

        var index = Lines.IndexOf(line);
        if (index >= 0)
        {
            Lines[index] = ToLineItem(line.LineId, result.Item);
        }
    }

    private async Task DeleteLineAsync(PurchaseRequestLineItem? line)
    {
        if (line is null || IsBusy)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", $"Удалить {line.MaterialName}?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        Lines.Remove(line);
    }

    private async Task<PurchaseRequestLineEditorResult?> OpenLineEditorAsync(PurchaseRequestLineItem? line)
    {
        if (Shell.Current?.Navigation is null)
        {
            return null;
        }

        var editorViewModel = _serviceProvider.GetRequiredService<PurchaseRequestLineEditorViewModel>();
        await editorViewModel.InitializeAsync(line);
        var modal = new PurchaseRequestLineEditorModal(editorViewModel);
        var completion = editorViewModel.WaitForResultAsync();
        await Shell.Current.Navigation.PushModalAsync(modal);
        return await completion;
    }

    private async Task<PurchasesCreateRequest?> BuildRequestAsync()
    {
        if (string.IsNullOrWhiteSpace(DocumentNumber))
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", "Номер документа обязателен.", "OK");
            return null;
        }

        if (string.IsNullOrWhiteSpace(WarehouseName))
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", "Склад обязателен.", "OK");
            return null;
        }

        if (Lines.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("Ведомость", "Добавьте хотя бы одну позицию.", "OK");
            return null;
        }

        var supplierId = SelectedSupplier?.SupplierId;

        var items = Lines.Select(line => new PurchaseItemRequest(
            line.MaterialId,
            line.MaterialName,
            line.Quantity,
            line.Unit,
            line.Price,
            line.Note)).ToArray();

        return new PurchasesCreateRequest(
            DocumentNumber.Trim(),
            CreatedAt,
            WarehouseName.Trim(),
            supplierId,
            string.IsNullOrWhiteSpace(Comment) ? null : Comment.Trim(),
            items);
    }

    private async Task EnsureLookupsLoadedAsync()
    {
        if (_suppliersLoaded)
        {
            return;
        }

        try
        {
            var suppliers = await _suppliersService.ListAsync();
            Suppliers.Clear();
            if (suppliers is not null)
            {
                foreach (var supplier in suppliers.OrderBy(s => s.Name))
                {
                    Suppliers.Add(new SupplierLookupItem(supplier.Id, supplier.Name));
                }
            }
            _suppliersLoaded = true;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Поставщики", $"Не удалось загрузить справочник: {ex.Message}", "OK");
        }
    }

    private static PurchaseRequestLineItem ToLineItem(Guid lineId, PurchaseItemRequest request)
    {
        var total = request.Price * (decimal)request.Quantity;
        return new PurchaseRequestLineItem(
            lineId,
            request.MaterialId,
            request.MaterialName,
            request.Quantity,
            request.Unit,
            request.Price,
            total,
            request.Note);
    }
}
