namespace MyFactory.WebApi.Contracts.Payroll;

public record PayrollGetResponse(
    Guid EmployeeId,
    string Period,
    decimal Accrued,
    decimal Paid,
    decimal Outstanding
);


