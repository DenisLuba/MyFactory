using MyFactory.MauiClient.Models.Finance;

namespace MyFactory.MauiClient.Services.Finance;

public interface IFinanceService
{
    Task<IReadOnlyList<PayrollAccrualListItemResponse>?> GetPayrollAccrualsAsync(DateOnly from, DateOnly to, Guid? employeeId = null, Guid? departmentId = null);
    Task<EmployeePayrollAccrualDetailsResponse?> GetEmployeePayrollAccrualsAsync(Guid employeeId, int year, int month);
    Task CalculateDailyAccrualAsync(CalculateDailyPayrollAccrualRequest request);
    Task CalculatePeriodAccrualsAsync(CalculatePayrollAccrualsForPeriodRequest request);
    Task AdjustAccrualAsync(Guid accrualId, AdjustPayrollAccrualRequest request);
    Task<CreatePayrollPaymentResponse?> CreatePayrollPaymentAsync(CreatePayrollPaymentRequest request);
}
