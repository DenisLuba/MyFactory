using MyFactory.MauiClient.Models.PayrollRules;

namespace MyFactory.MauiClient.Services.PayrollRules;

public interface IPayrollRulesService
{
    Task<IReadOnlyList<PayrollRuleResponse>?> GetListAsync();
    Task<PayrollRuleResponse?> GetDetailsAsync(Guid id);
    Task<CreatePayrollRuleResponse?> CreateAsync(CreatePayrollRuleRequest request);
    Task UpdateAsync(Guid id, UpdatePayrollRuleRequest request);
    Task DeleteAsync(Guid id);
}
