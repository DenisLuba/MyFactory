using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.ExpenceTypes;
using MyFactory.MauiClient.Services.ExpenceTypes;

namespace MyFactory.MauiClient.ViewModels.Finance.Expenses;

public partial class ExpenseCategoriesPageViewModel : ObservableObject
{
    private readonly IExpenceTypesService _expenceTypesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<ExpenseTypeResponse> ExpenseTypes { get; } = new();

    public ExpenseCategoriesPageViewModel(IExpenceTypesService expenceTypesService)
    {
        _expenceTypesService = expenceTypesService;
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        ErrorMessage = null;
        try
        {
            ExpenseTypes.Clear();
            var items = await _expenceTypesService.GetListAsync() ?? Array.Empty<ExpenseTypeResponse>();
            foreach (var item in items)
            {
                ExpenseTypes.Add(item);
            }
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

    [RelayCommand]
    private async Task AddAsync()
    {
        var name = await Shell.Current.DisplayPromptAsync("Новый тип", "Введите наименование", "Сохранить", "Отмена");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание", "Сохранить", "Пропустить");
        try
        {
            await _expenceTypesService.CreateAsync(new CreateExpenseTypeRequest(name.Trim(), description));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task EditAsync(ExpenseTypeResponse? expenseType)
    {
        if (expenseType is null)
        {
            return;
        }

        var name = await Shell.Current.DisplayPromptAsync("Редактирование", "Наименование", initialValue: expenseType.Name, accept: "Сохранить", cancel: "Отмена");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        var description = await Shell.Current.DisplayPromptAsync("Описание", "Введите описание", "Сохранить", "Пропустить", initialValue: expenseType.Description);
        try
        {
            await _expenceTypesService.UpdateAsync(expenseType.Id, new UpdateExpenseTypeRequest(name.Trim(), description));
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(ExpenseTypeResponse? expenseType)
    {
        if (expenseType is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Удаление", "Удалить тип расходов?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            await _expenceTypesService.DeleteAsync(expenseType.Id);
            ExpenseTypes.Remove(expenseType);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }
}

