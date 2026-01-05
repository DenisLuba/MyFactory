using MyFactory.MauiClient.Models.Advances;

namespace MyFactory.MauiClient.Services.Advances;

public interface IAdvancesService
{
    Task<IReadOnlyList<CashAdvanceListItemResponse>?> GetListAsync(DateOnly? from = null, DateOnly? to = null, Guid? employeeId = null);
    Task<CreateCashAdvanceResponse?> IssueAsync(CreateCashAdvanceRequest request);
    Task AddAmountAsync(Guid cashAdvanceId, AddCashAdvanceAmountRequest request);
    Task<CreateCashAdvanceExpenseResponse?> AddExpenseAsync(Guid cashAdvanceId, CreateCashAdvanceExpenseRequest request);
    Task<CreateCashAdvanceReturnResponse?> AddReturnAsync(Guid cashAdvanceId, CreateCashAdvanceReturnRequest request);
    Task CloseAsync(Guid cashAdvanceId);
}
