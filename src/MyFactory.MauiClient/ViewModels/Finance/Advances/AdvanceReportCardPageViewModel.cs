using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.Services.FinanceServices;
using MyFactory.MauiClient.UIModels.Finance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.ViewModels.Finance.Advances;

public partial class AdvanceReportCardPageViewModel : ObservableObject
{
    private readonly IFinanceService _financeService;
    private readonly RelayCommand _addReportItemCommand;
    private readonly RelayCommand<AdvanceReportItem?> _deleteReportItemCommand;
    private readonly AsyncRelayCommand _saveReportCommand;
    private readonly AsyncRelayCommand _submitReportCommand;
    private AdvancesTablePageViewModel? _parentTableViewModel;

    [ObservableProperty]
    private AdvanceItem advance = new("", "", 0, "", AdvanceStatus.Pending);

    [ObservableProperty]
    private ObservableCollection<AdvanceReportItem> reportItems = new();

    [ObservableProperty]
    private string reportDescription = string.Empty;

    [ObservableProperty]
    private decimal totalSpent;

    [ObservableProperty]
    private decimal advanceAmount;

    [ObservableProperty]
    private decimal balance;

    [ObservableProperty]
    private bool isOverSpent;

    [ObservableProperty]
    private string newItemName = string.Empty;

    [ObservableProperty]
    private decimal newItemAmount;

    [ObservableProperty]
    private DateTime newItemDate = DateTime.Today;

    [ObservableProperty]
    private string newItemComment = string.Empty;

    [ObservableProperty]
    private string newItemReceiptUri = string.Empty;

    [ObservableProperty]
    private AdvanceReportCategories newItemCategory = AdvanceReportCategories.Inventory;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private bool hasDraft;

    public AdvanceReportCardPageViewModel(IFinanceService financeService)
    {
        _financeService = financeService;

        _addReportItemCommand = new RelayCommand(AddReportItem, CanAddReportItem);
        _deleteReportItemCommand = new RelayCommand<AdvanceReportItem?>(DeleteReportItem);
        _saveReportCommand = new AsyncRelayCommand(SaveReportAsync, CanPersistReport);
        _submitReportCommand = new AsyncRelayCommand(SubmitReportAsync, CanPersistReport);

        ReportItems.CollectionChanged += ReportItemsCollectionChanged;
    }

    public IRelayCommand AddReportItemCommand => _addReportItemCommand;
    public IRelayCommand<AdvanceReportItem?> DeleteReportItemCommand => _deleteReportItemCommand;
    public IAsyncRelayCommand SaveReportCommand => _saveReportCommand;
    public IAsyncRelayCommand SubmitReportCommand => _submitReportCommand;
    public IReadOnlyList<AdvanceReportCategories> Categories { get; } = Enum.GetValues<AdvanceReportCategories>();

    public void Initialize(AdvanceItem advance, IEnumerable<AdvanceReportItem>? existingItems = null)
    {
        Advance = advance;
        AdvanceAmount = advance?.AdvanceAmount ?? 0;

        ReportItems = existingItems != null
            ? new ObservableCollection<AdvanceReportItem>(existingItems)
            : new ObservableCollection<AdvanceReportItem>();

        ReportDescription = string.Empty;
        ResetNewItemForm();

        HasDraft = ReportItems.Any();
        RecalculateTotals();
        UpdateCommandStates();
    }

    public void AttachParentTable(AdvancesTablePageViewModel parentViewModel)
        => _parentTableViewModel = parentViewModel;

    private void ReportItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        HasDraft = ReportItems.Any();
        RecalculateTotals();
        UpdateCommandStates();
    }

    partial void OnReportItemsChanging(ObservableCollection<AdvanceReportItem> value)
    {
        if (ReportItems != null)
        {
            ReportItems.CollectionChanged -= ReportItemsCollectionChanged;
        }
    }

    partial void OnReportItemsChanged(ObservableCollection<AdvanceReportItem> value)
    {
        if (ReportItems != null)
        {
            ReportItems.CollectionChanged += ReportItemsCollectionChanged;
        }
        RecalculateTotals();
        UpdateCommandStates();
    }

    partial void OnAdvanceChanged(AdvanceItem value)
    {
        AdvanceAmount = value?.AdvanceAmount ?? 0;
        ResetNewItemForm();
        RecalculateTotals();
    }

    partial void OnNewItemNameChanged(string value) => _addReportItemCommand.NotifyCanExecuteChanged();
    partial void OnNewItemAmountChanged(decimal value) => _addReportItemCommand.NotifyCanExecuteChanged();
    partial void OnIsBusyChanged(bool value)
    {
        UpdateCommandStates();
        _addReportItemCommand.NotifyCanExecuteChanged();
    }
    partial void OnNewItemDateChanged(DateTime value) => _addReportItemCommand.NotifyCanExecuteChanged();

    private void AddReportItem()
    {
        if (!CanAddReportItem())
        {
            return;
        }

        var item = new AdvanceReportItem(
            NewItemName.Trim(),
            NewItemDate,
            NewItemAmount,
            NewItemComment?.Trim() ?? string.Empty,
            NewItemCategory,
            string.IsNullOrWhiteSpace(NewItemReceiptUri) ? null : NewItemReceiptUri.Trim());
        ReportItems.Add(item);
        HasDraft = true;

        ResetNewItemForm();
    }

    private bool CanAddReportItem()
        => !IsBusy && !string.IsNullOrWhiteSpace(NewItemName) && NewItemAmount > 0;

    private void DeleteReportItem(AdvanceReportItem? item)
    {
        if (item == null)
        {
            return;
        }

        ReportItems.Remove(item);
        HasDraft = ReportItems.Any();
    }

    private void ResetNewItemForm()
    {
        NewItemName = string.Empty;
        NewItemAmount = 0;
        NewItemDate = GetDefaultExpenseDate();
        NewItemComment = string.Empty;
        NewItemCategory = AdvanceReportCategories.Inventory;
        NewItemReceiptUri = string.Empty;
    }

    private DateTime GetDefaultExpenseDate()
    {
        if (Advance != null && DateTime.TryParse(Advance.Date, out var parsedDate))
        {
            return parsedDate;
        }

        return DateTime.Today;
    }

    private bool CanPersistReport()
        => !IsBusy && ReportItems.Any();

    private async Task SaveReportAsync()
    {
        if (!ValidateReport(out var validationMessage))
        {
            await ShowAlertAsync("Отчет неполон", validationMessage);
            return;
        }

        HasDraft = true;
        await ShowAlertAsync("Черновик сохранен", "Вы можете вернуться к отчету позже.");
    }

    private async Task SubmitReportAsync()
    {
        if (!ValidateReport(out var validationMessage))
        {
            await ShowAlertAsync("Отчет неполон", validationMessage);
            return;
        }

        try
        {
            IsBusy = true;

            var request = new SubmitAdvanceReportRequest(
                TotalSpent,
                ReportDescription,
                ReportItems.ToList());

            await _financeService.SubmitAdvanceReportAsync(Advance.AdvanceNumber, request);

            HasDraft = false;
            await ShowAlertAsync("Отчет отправлен", "Финансовая служба получила отчет по выдаче.");

            _parentTableViewModel?.LoadAdvancesCommand.Execute(null);
            await Shell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Ошибка", $"Не удалось отправить отчет: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool ValidateReport(out string message)
    {
        if (Advance == null || string.IsNullOrWhiteSpace(Advance.AdvanceNumber))
        {
            message = "Не выбрана подотчетная выдача.";
            return false;
        }

        if (!ReportItems.Any())
        {
            message = "Добавьте хотя бы одну строку расходов.";
            return false;
        }

        message = string.Empty;
        return true;
    }

    private void RecalculateTotals()
    {
        TotalSpent = ReportItems.Sum(item => item.Amount);
        Balance = AdvanceAmount - TotalSpent;
        IsOverSpent = Balance < 0;
    }

    private void UpdateCommandStates()
    {
        _saveReportCommand.NotifyCanExecuteChanged();
        _submitReportCommand.NotifyCanExecuteChanged();
    }

    private Task ShowAlertAsync(string title, string message)
        => Shell.Current?.DisplayAlertAsync(title, message, "OK") ?? Task.CompletedTask;
}
