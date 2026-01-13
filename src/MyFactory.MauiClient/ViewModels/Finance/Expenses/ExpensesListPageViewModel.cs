using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using MyFactory.MauiClient.Models.ExpenceTypes;
using MyFactory.MauiClient.Models.Expences;
using MyFactory.MauiClient.Services.ExpenceTypes;
using MyFactory.MauiClient.Services.Expences;

namespace MyFactory.MauiClient.ViewModels.Finance.Expenses;

public partial class ExpensesListPageViewModel : ObservableObject
{
    private readonly IExpencesService _expencesService;
    private readonly IExpenceTypesService _expenceTypesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<string> Periods { get; } = new(["Текущий месяц", "Прошлый месяц", "Последние 7 дней"]);

    [ObservableProperty]
    private string selectedPeriod = "Текущий месяц";

    public ObservableCollection<ExpenseTypeResponse> ExpenseTypes { get; } = new();

    [ObservableProperty]
    private ExpenseTypeResponse? selectedExpenseType;

    public ObservableCollection<ExpenseListItemResponse> Expenses { get; } = new();

    [ObservableProperty]
    private decimal totalAmount;

    public ExpensesListPageViewModel(IExpencesService expencesService, IExpenceTypesService expenceTypesService)
    {
        _expencesService = expencesService;
        _expenceTypesService = expenceTypesService;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await LoadExpenseTypesAsync();
        await LoadExpensesAsync();
    }

    private (DateOnly from, DateOnly to) GetSelectedPeriodRange()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return SelectedPeriod switch
        {
            "Прошлый месяц" =>
                (new DateOnly(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 1),
                 new DateOnly(today.Year, today.Month, 1).AddDays(-1)),
            "Последние 7 дней" => (today.AddDays(-7), today),
            _ => (new DateOnly(today.Year, today.Month, 1), new DateOnly(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month)))
        };
    }

    private async Task LoadExpenseTypesAsync()
    {
        try
        {
            ExpenseTypes.Clear();
            var types = await _expenceTypesService.GetListAsync() ?? Array.Empty<ExpenseTypeResponse>();
            foreach (var type in types)
            {
                ExpenseTypes.Add(type);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task LoadExpensesAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            Expenses.Clear();
            var (from, to) = GetSelectedPeriodRange();
            var items = await _expencesService.GetListAsync(from, to, SelectedExpenseType?.Id);
            if (items is not null)
            {
                foreach (var item in items)
                {
                    Expenses.Add(item);
                }
            }

            TotalAmount = Expenses.Sum(x => x.Amount);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedPeriodChanged(string value)
    {
        _ = LoadExpensesAsync();
    }

    partial void OnSelectedExpenseTypeChanged(ExpenseTypeResponse? value)
    {
        _ = LoadExpensesAsync();
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        if (SelectedExpenseType is null)
        {
            await Shell.Current.DisplayAlertAsync("Тип", "Выберите тип расходов", "ОК");
            return;
        }

        var amountText = await Shell.Current.DisplayPromptAsync("Сумма", "Введите сумму", "Сохранить", "Отмена", keyboard: Keyboard.Numeric);
        if (!decimal.TryParse(amountText, out var amount) || amount <= 0)
        {
            return;
        }

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание", "Сохранить", "Пропустить");
        var (from, to) = GetSelectedPeriodRange();
        var request = new CreateExpenseRequest(SelectedExpenseType.Id, to, amount, description);
        try
        {
            await _expencesService.CreateAsync(request);
            await LoadExpensesAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task EditAsync(ExpenseListItemResponse? expense)
    {
        if (expense is null)
        {
            return;
        }

        var amountText = await Shell.Current.DisplayPromptAsync("Сумма", "Введите сумму", initialValue: expense.Amount.ToString(), accept: "Сохранить", cancel: "Отмена", keyboard: Keyboard.Numeric);
        if (!decimal.TryParse(amountText, out var amount) || amount <= 0)
        {
            return;
        }

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание", "Сохранить", "Пропустить", initialValue: expense.Description);

        var type = SelectedExpenseType ?? ExpenseTypes.FirstOrDefault(t => t.Name == expense.ExpenseTypeName);
        if (type is null)
        {
            await Shell.Current.DisplayAlertAsync("Тип", "Не удалось определить тип расходов", "ОК");
            return;
        }

        try
        {
            await _expencesService.UpdateAsync(expense.Id, new UpdateExpenseRequest(expense.ExpenseDate, type.Id, amount, description));
            await LoadExpensesAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(ExpenseListItemResponse? expense)
    {
        if (expense is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", "Удалить расход?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            await _expencesService.DeleteAsync(expense.Id);
            Expenses.Remove(expense);
            TotalAmount = Expenses.Sum(x => x.Amount);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }
}
