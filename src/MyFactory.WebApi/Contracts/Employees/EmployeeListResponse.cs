using System;

namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeListResponse(
    Guid Id,
    string FullName,
    string Position,
    int Grade,
    bool IsActive
);
