using System;
using System.Collections.Generic;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.OldDTOs.Production;

public sealed record WorkerAssignmentDto(
    Guid Id,
    Guid ProductionStageId,
    Guid EmployeeId,
    string EmployeeName,
    decimal QuantityAssigned,
    decimal? QuantityCompleted,
    string Status,
    DateTime AssignedAt)
{
    public static WorkerAssignmentDto FromEntity(
        WorkerAssignment assignment,
        IReadOnlyDictionary<Guid, Employee> employees)
    {
        var employeeName = employees.TryGetValue(assignment.EmployeeId, out var employee)
            ? employee.FullName
            : string.Empty;

        return new WorkerAssignmentDto(
            assignment.Id,
            assignment.ProductionStageId,
            assignment.EmployeeId,
            employeeName,
            assignment.QuantityAssigned,
            assignment.QuantityCompleted,
            assignment.Status,
            assignment.AssignedAt);
    }
}
