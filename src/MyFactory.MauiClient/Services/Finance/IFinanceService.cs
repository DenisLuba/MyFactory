using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Finance;

public interface IFinanceService : IGetListService<PayrollAccrualListItemResponse>
{
    Task<ListResponse<PayrollAccrualListItemResponse>?> GetPayrollAccrualsAsync(
        DateOnly from,
        DateOnly to,
        Guid? employeeId = null,
        Guid? departmentId = null,
        string? sortBy = null,
        bool sortDesc = false,
        int skip = 0,
        int take = 30);
    Task<EmployeePayrollAccrualDetailsResponse?> GetEmployeePayrollAccrualsAsync(Guid employeeId, int year, int month);
    Task CalculateDailyAccrualAsync(CalculateDailyPayrollAccrualRequest request);
    Task CalculatePeriodAccrualsAsync(CalculatePayrollAccrualsForPeriodRequest request);
    Task AdjustAccrualAsync(Guid accrualId, AdjustPayrollAccrualRequest request);
    Task<CreatePayrollPaymentResponse?> CreatePayrollPaymentAsync(CreatePayrollPaymentRequest request);
}
