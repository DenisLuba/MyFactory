using System;

namespace MyFactory.WebApi.Contracts.Shifts;

[Obsolete("Use ShiftResultListResponse instead.")]
public record ShiftsGetResultsResponse(
    Guid ShiftPlanId,
    int ActualQty,
    double HoursWorked
);

