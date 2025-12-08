using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Finance.Commands.RevenueReports;

public sealed class RecordRevenueCommandHandler : IRequestHandler<RecordRevenueCommand, RevenueReportDto>
{
    private readonly IApplicationDbContext _context;

    public RecordRevenueCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RevenueReportDto> Handle(RecordRevenueCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.SpecificationId, cancellationToken)
            ?? throw new InvalidOperationException("Specification not found.");

        var report = await _context.RevenueReports
            .FirstOrDefaultAsync(
                entity => entity.PeriodMonth == request.PeriodMonth
                    && entity.PeriodYear == request.PeriodYear
                    && entity.SpecificationId == request.SpecificationId,
                cancellationToken);

        if (report is null)
        {
            if (request.IsPaid && request.PaymentDate is null)
            {
                throw new InvalidOperationException("Payment date must be provided when marking revenue as paid.");
            }

            report = new RevenueReport(
                request.PeriodMonth,
                request.PeriodYear,
                request.SpecificationId,
                request.Quantity,
                request.UnitPrice,
                request.IsPaid,
                request.IsPaid ? request.PaymentDate : null);

            await _context.RevenueReports.AddAsync(report, cancellationToken);
        }
        else
        {
            report.UpdateSales(request.Quantity, request.UnitPrice);

            if (request.IsPaid)
            {
                if (!request.PaymentDate.HasValue)
                {
                    throw new InvalidOperationException("Payment date must be provided when marking revenue as paid.");
                }

                report.MarkPaid(request.PaymentDate.Value);
            }
            else
            {
                report.MarkUnpaid();
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return RevenueReportDto.FromEntity(report, specification.Name);
    }
}
