using System;
using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.OldFeatures.Payroll.Queries.GetTimesheetEntriesByEmployee;

public sealed record GetTimesheetEntriesByEmployeeQuery(
    Guid EmployeeId,
    DateOnly? FromDate,
    DateOnly? ToDate) : IRequest<IReadOnlyCollection<TimesheetEntryDto>>;
