using MyFactory.MauiClient.Models.ExpenceTypes;

namespace MyFactory.MauiClient.Services.ExpenceTypes;

public interface IExpenceTypesService
{
    Task<IReadOnlyList<ExpenseTypeResponse>?> GetListAsync();
    Task<ExpenseTypeResponse?> GetDetailsAsync(Guid id);
    Task<CreateExpenseTypeResponse?> CreateAsync(CreateExpenseTypeRequest request);
    Task UpdateAsync(Guid id, UpdateExpenseTypeRequest request);
    Task DeleteAsync(Guid id);
}
