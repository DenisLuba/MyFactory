using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Application.Features.Production.Common;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.Production.Commands.AssignWorker;

public sealed class AssignWorkerCommandHandler : IRequestHandler<AssignWorkerCommand, ProductionOrderDto>
{
    private readonly IApplicationDbContext _context;

    public AssignWorkerCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductionOrderDto> Handle(AssignWorkerCommand request, CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(employee => employee.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var order = await _context.ProductionOrders
            .WithDetails()
            .FirstOrDefaultAsync(entity => entity.Stages.Any(stage => stage.Id == request.StageId), cancellationToken)
            ?? throw new InvalidOperationException("Production stage not found.");

        var stage = order.Stages.First(stage => stage.Id == request.StageId);

        var assignment = stage.AssignWorker(request.EmployeeId, request.QtyAssigned, DateTime.UtcNow);

        var assignmentExists = await _context.WorkerAssignments
            .AnyAsync(entity => entity.Id == assignment.Id, cancellationToken);

        if (!assignmentExists)
        {
            _context.WorkerAssignments.Add(assignment);
        }

        if (request.QtyCompleted > 0)
        {
            if (assignment.Status == WorkerAssignmentStatuses.Completed)
            {
                if (assignment.QuantityCompleted != request.QtyCompleted)
                {
                    throw new InvalidOperationException("Assignment already completed.");
                }
            }
            else
            {
                if (assignment.Status == WorkerAssignmentStatuses.Assigned)
                {
                    assignment.StartWork();
                }

                assignment.CompleteWork(request.QtyCompleted);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return await ProductionOrderDtoFactory.CreateAsync(_context, order, cancellationToken);
    }
}
