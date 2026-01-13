using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.PayrollRules;
using MyFactory.MauiClient.Services.PayrollRules;

namespace MyFactory.MauiClient.ViewModels.Finance.Payroll;

public partial class PayrollRulesListPageViewModel : ObservableObject
{
    private readonly IPayrollRulesService _payrollRulesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    public ObservableCollection<PayrollRuleResponse> Rules { get; } = new();

    public PayrollRulesListPageViewModel(IPayrollRulesService payrollRulesService)
    {
        _payrollRulesService = payrollRulesService;
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
            Rules.Clear();
            var items = await _payrollRulesService.GetListAsync() ?? Array.Empty<PayrollRuleResponse>();
            foreach (var item in items.OrderBy(x => x.EffectiveFrom))
            {
                Rules.Add(item);
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
        await Shell.Current.GoToAsync(nameof(Pages.Finance.Payroll.PayrollRuleDetailsPage));
    }

    [RelayCommand]
    private async Task OpenDetailsAsync(PayrollRuleResponse? rule)
    {
        if (rule is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(Pages.Finance.Payroll.PayrollRuleDetailsPage), new Dictionary<string, object>
        {
            { "RuleId", rule.Id.ToString() }
        });
    }

    [RelayCommand]
    private async Task DeleteAsync(PayrollRuleResponse? rule)
    {
        if (rule is null)
        {
            return;
        }

        var confirm = await Shell.Current.DisplayAlertAsync("Удалить правило", "Удалить выбранное правило?", "Да", "Нет");
        if (!confirm)
        {
            return;
        }

        try
        {
            await _payrollRulesService.DeleteAsync(rule.Id);
            Rules.Remove(rule);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Ошибка", ex.Message, "ОК");
        }
    }
}

