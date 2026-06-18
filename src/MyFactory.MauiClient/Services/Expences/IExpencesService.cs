using MyFactory.MauiClient.Models.Expences;

namespace MyFactory.MauiClient.Services.Expences;

public interface IExpencesService
{
    Task<IReadOnlyList<ExpenseListItemResponse>?> GetListAsync(DateOnly from, DateOnly to, Guid? expenseTypeId = null);
    Task<CreateExpenseResponse?> CreateAsync(CreateExpenseRequest request);
    Task UpdateAsync(Guid id, UpdateExpenseRequest request);
    Task DeleteAsync(Guid id);
}
