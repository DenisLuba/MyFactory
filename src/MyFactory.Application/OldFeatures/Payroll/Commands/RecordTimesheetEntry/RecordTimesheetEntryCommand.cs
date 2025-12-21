using System;
using MediatR;
using MyFactory.Application.DTOs.Payroll;

namespace MyFactory.Application.OldFeatures.Payroll.Commands.RecordTimesheetEntry;

public sealed record RecordTimesheetEntryCommand(
    Guid EmployeeId,
    DateOnly WorkDate,
    decimal HoursWorked,
    Guid? ProductionOrderId) : IRequest<TimesheetEntryDto>;
