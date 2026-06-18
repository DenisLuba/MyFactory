using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFactory.MauiClient.Models.PayrollRules;
using MyFactory.MauiClient.Services.PayrollRules;

namespace MyFactory.MauiClient.ViewModels.Finance.Payroll;

[QueryProperty(nameof(RuleId), "RuleId")]
public partial class PayrollRuleDetailsPageViewModel : ObservableObject
{
    private readonly IPayrollRulesService _payrollRulesService;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? errorMessage;

    [ObservableProperty]
    private Guid? ruleId;

    [ObservableProperty]
    private DateTime effectiveFrom = DateTime.Today;

    [ObservableProperty]
    private decimal premiumPercent;

    [ObservableProperty]
    private string description = string.Empty;

    public PayrollRuleDetailsPageViewModel(IPayrollRulesService payrollRulesService)
    {
        _payrollRulesService = payrollRulesService;
    }

    partial void OnRuleIdChanged(Guid? value)
    {
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        if (RuleId is null || RuleId == Guid.Empty)
        {
            return;
        }

        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var rule = await _payrollRulesService.GetDetailsAsync(RuleId.Value);
            if (rule is not null)
            {
                EffectiveFrom = rule.EffectiveFrom.ToDateTime(TimeOnly.MinValue);
                PremiumPercent = rule.PremiumPercent;
                Description = rule.Description;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var request = new CreatePayrollRuleRequest(DateOnly.FromDateTime(EffectiveFrom), PremiumPercent, Description ?? string.Empty);
            if (RuleId is null || RuleId == Guid.Empty)
            {
                await _payrollRulesService.CreateAsync(request);
            }
            else
            {
                var updateRequest = new UpdatePayrollRuleRequest(request.EffectiveFrom, request.PremiumPercent, request.Description);
                await _payrollRulesService.UpdateAsync(RuleId.Value, updateRequest);
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            await Shell.Current.DisplayAlertAsync("Œ¯Ë·Í‡", ex.Message, "Œ ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}

