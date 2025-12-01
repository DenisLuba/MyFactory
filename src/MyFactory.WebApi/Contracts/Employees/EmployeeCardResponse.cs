using System;

namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeCardResponse(
    Guid Id,
    string FullName,
    string Position,
    int Grade,
    bool IsActive,
    string EmployeeCode,
    DateOnly HireDate
);
