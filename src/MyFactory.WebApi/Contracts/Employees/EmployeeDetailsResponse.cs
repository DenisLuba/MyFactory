namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeDetailsResponse(
    Guid Id,
    string FullName,
    DepartmentInfoResponse Department,
    PositionInfoResponse Position,
    int Grade,
    decimal RatePerNormHour,
    decimal? PremiumPercent,
    DateOnly HiredAt,
    DateOnly? FiredAt,
    bool IsActive,
    IReadOnlyList<ContactResponse> Contacts);
