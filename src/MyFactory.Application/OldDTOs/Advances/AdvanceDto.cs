using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.OldDTOs.Advances;

public sealed record AdvanceDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    decimal Amount,
    decimal ReportedAmount,
    decimal RemainingAmount,
    DateOnly IssuedAt,
    DateOnly? ClosedAt,
    string Status,
    string? Description,
    IReadOnlyCollection<AdvanceReportDto> Reports)
{
    public static AdvanceDto FromEntity(Advance advance, string employeeName)
    {
        var reports = advance.Reports.Select(AdvanceReportDto.FromEntity).ToArray();

        return new AdvanceDto(
            advance.Id,
            advance.EmployeeId,
            employeeName,
            advance.Amount,
            advance.ReportedAmount,
            advance.RemainingAmount,
            advance.IssuedAt,
            advance.ClosedAt,
            advance.Status,
            advance.Description,
            reports);
    }
}
