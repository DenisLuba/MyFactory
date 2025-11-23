namespace MyFactory.WebApi.Contracts.Payroll;

public record PayrollCalculateResponse(
    PayrollCalculatingStatus Status,
    DateTime From,
    DateTime To
);


