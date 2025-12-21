using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Application.OldDTOs.Shifts;

public sealed record ShiftPlanDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeFullName,
    Guid SpecificationId,
    string SpecificationSku,
    string SpecificationName,
    DateOnly ShiftDate,
    string ShiftType,
    decimal PlannedQuantity,
    IReadOnlyCollection<ShiftResultDto> Results)
{
    public static ShiftPlanDto FromEntity(ShiftPlan plan, IReadOnlyCollection<ShiftResult> results)
    {
        var employeeFullName = plan.Employee?.FullName ?? string.Empty;
        var specificationSku = plan.Specification?.Sku ?? string.Empty;
        var specificationName = plan.Specification?.Name ?? string.Empty;

        var resultDtos = results
            .OrderBy(result => result.RecordedAt)
            .Select(ShiftResultDto.FromEntity)
            .ToList();

        return new ShiftPlanDto(
            plan.Id,
            plan.EmployeeId,
            employeeFullName,
            plan.SpecificationId,
            specificationSku,
            specificationName,
            plan.ShiftDate,
            plan.ShiftType,
            plan.PlannedQuantity,
            resultDtos);
    }
}
