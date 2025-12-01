using System;

namespace MyFactory.MauiClient.Models.Employees;

public record EmployeeListResponse(
    Guid Id,
    string FullName,
    string Position,
    int Grade,
    bool IsActive
);
