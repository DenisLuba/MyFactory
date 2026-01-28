namespace MyFactory.MauiClient.Models.Employees;

public record CreateEmployeeRequest(
    string FullName,
    Guid PositionId,
    Guid DepartmentId,
    int? Grade,
    decimal? RatePerNormHour,
    decimal? PremiumPercent,
    DateTime HiredAt,
    bool IsActive);
