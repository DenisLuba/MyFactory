using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.Advances;
using MyFactory.MauiClient.Services.Advances;

namespace MyFactory.MauiClient.ViewModels.Finance.Expenses;


[QueryProperty(nameof(CashAdvanceJsonParameter), "CashAdvanceJson")]
public partial class CashAdvanceDetailsPageViewModel : ObservableObject
{
    private readonly IAdvancesService _advancesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private CashAdvanceListItemResponse? advanceItem;

    [ObservableProperty]
    private string? cashAdvanceJsonParameter;

    [ObservableProperty]
    private string title = "Карточка аванса";

    [ObservableProperty]
    private string employeeName = string.Empty;

    [ObservableProperty]
    private string issueDate = string.Empty;

    [ObservableProperty]
    private decimal issuedAmount;

    [ObservableProperty]
    private decimal spentAmount;

    [ObservableProperty]
    private decimal returnedAmount;

    [ObservableProperty]
    private decimal balance;

    [ObservableProperty]
    private string status = string.Empty;

    [ObservableProperty]
    private string additionalAmount = string.Empty;

    [ObservableProperty]
    private string returnAmount = string.Empty;

    public ObservableCollection<CashAdvanceOperationItem> Operations { get; } = new();

    public CashAdvanceDetailsPageViewModel(IAdvancesService advancesService)
    {
        _advancesService = advancesService;
    }

    partial void OnAdvanceItemChanged(CashAdvanceListItemResponse? value)
    {
        if (value is null)
        {
            return;
        }

        Title = $"Аванс {value.Id.ToString()[..6]}";
        EmployeeName = value.EmployeeName;
        IssueDate = value.IssueDate.ToString("dd.MM.yyyy");
        IssuedAmount = value.IssuedAmount;
        SpentAmount = value.SpentAmount;
        ReturnedAmount = value.ReturnedAmount;
        Balance = value.Balance;
        Status = value.IsClosed ? "Закрыт" : "Открыт";
    }

    partial void OnCashAdvanceJsonParameterChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }
        try
        {
            var advance = System.Text.Json.JsonSerializer.Deserialize<CashAdvanceListItemResponse>(value);
            AdvanceItem = advance;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка при загрузке данных аванса: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task AddAmountAsync()
    {
        if (AdvanceItem is null)
        {
            return;
        }

        if (!decimal.TryParse(AdditionalAmount, out var amount) || amount <= 0)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Введите сумму", "ОК");
            return;
        }

        try
        {
            await _advancesService.AddAmountAsync(AdvanceItem.Id, new AddCashAdvanceAmountRequest(DateOnly.FromDateTime(DateTime.Today), amount));
            IssuedAmount += amount;
            Balance = IssuedAmount - SpentAmount - ReturnedAmount;
            AdditionalAmount = string.Empty;
            Operations.Insert(0, new CashAdvanceOperationItem(DateOnly.FromDateTime(DateTime.Today).ToString("dd.MM.yyyy"), "Выдано", string.Empty, amount));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task ReturnAsync()
    {
        if (AdvanceItem is null)
        {
            return;
        }

        if (!decimal.TryParse(ReturnAmount, out var amount) || amount <= 0)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Введите сумму", "ОК");
            return;
        }

        try
        {
            await _advancesService.AddReturnAsync(AdvanceItem.Id, new CreateCashAdvanceReturnRequest(DateOnly.FromDateTime(DateTime.Today), amount, null));
            ReturnedAmount += amount;
            Balance = IssuedAmount - SpentAmount - ReturnedAmount;
            ReturnAmount = string.Empty;
            Operations.Insert(0, new CashAdvanceOperationItem(DateOnly.FromDateTime(DateTime.Today).ToString("dd.MM.yyyy"), "Возврат", string.Empty, -amount));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task AddExpenseAsync()
    {
        if (AdvanceItem is null)
        {
            return;
        }

        var amountText = await Shell.Current.DisplayPromptAsync("Новый расход", "Введите сумму", "Сохранить", "Отмена", keyboard: Keyboard.Numeric);
        if (!decimal.TryParse(amountText, out var amount) || amount <= 0)
        {
            return;
        }

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание", "Сохранить", "Пропустить");

        try
        {
            await _advancesService.AddExpenseAsync(AdvanceItem.Id, new CreateCashAdvanceExpenseRequest(DateOnly.FromDateTime(DateTime.Today), amount, description));
            SpentAmount += amount;
            Balance = IssuedAmount - SpentAmount - ReturnedAmount;
            Operations.Insert(0, new CashAdvanceOperationItem(DateOnly.FromDateTime(DateTime.Today).ToString("dd.MM.yyyy"), "Расход", description ?? string.Empty, amount));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Ошибка", ex.Message, "ОК");
        }
    }
}

public record CashAdvanceOperationItem(string Date, string Type, string Description, decimal Amount);
