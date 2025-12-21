using System;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Application.OldDTOs.Shifts;

public sealed record ShiftResultDto(
    Guid Id,
    Guid ShiftPlanId,
    decimal ActualQuantity,
    decimal HoursWorked,
    DateTime RecordedAt)
{
    public static ShiftResultDto FromEntity(ShiftResult result)
    {
        return new ShiftResultDto(
            result.Id,
            result.ShiftPlanId,
            result.ActualQuantity,
            result.HoursWorked,
            result.RecordedAt);
    }
}
