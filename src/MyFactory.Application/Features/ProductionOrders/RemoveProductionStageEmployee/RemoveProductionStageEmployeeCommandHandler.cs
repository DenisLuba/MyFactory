using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ProductionOrders.RemoveProductionStageEmployee;

public sealed class RemoveProductionStageEmployeeCommandHandler
    : IRequestHandler<RemoveProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public RemoveProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RemoveProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var order = await _db.ProductionOrders
            .Include(x => x.ProductionOrderDepartmentEmployees)
            .Include(x => x.SewingOperations)
            .FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found.");

        var assignment = order.ProductionOrderDepartmentEmployees
            .FirstOrDefault(x => x.Id == request.AssignmentId)
            ?? throw new NotFoundException("Stage assignment not found.");

        if (assignment.Stage != request.Stage)
            throw new DomainApplicationException("Assignment stage does not match request stage.");

        order.RemoveStageAssignment(request.AssignmentId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
