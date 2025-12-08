using System;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.DTOs.Advances;

public sealed record AdvanceReportDto(
    Guid Id,
    Guid AdvanceId,
    string Description,
    decimal Amount,
    DateOnly ReportedAt)
{
    public static AdvanceReportDto FromEntity(AdvanceReport report)
    {
        return new AdvanceReportDto(
            report.Id,
            report.AdvanceId,
            report.Description,
            report.Amount,
            report.ReportedAt);
    }
}
