using System;

namespace MyFactory.MauiClient.Models.Employees;

public record EmployeeCardResponse(
    Guid Id,
    string FullName,
    string Position,
    int Grade,
    bool IsActive,
    string EmployeeCode,
    DateOnly HireDate
);
