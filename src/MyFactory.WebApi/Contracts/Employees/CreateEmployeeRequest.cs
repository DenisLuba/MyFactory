namespace MyFactory.WebApi.Contracts.Employees;

public record CreateEmployeeRequest(
    string FullName,
    Guid PositionId,
    int Grade,
    decimal RatePerNormHour,
    decimal PremiumPercent,
    DateTime HiredAt,
    bool IsActive);
