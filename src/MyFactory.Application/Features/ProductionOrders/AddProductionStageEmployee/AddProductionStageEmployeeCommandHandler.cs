using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed class AddProductionStageEmployeeCommandHandler : IRequestHandler<AddProductionStageEmployeeCommand>
{
    private readonly IApplicationDbContext _db;

    public AddProductionStageEmployeeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AddProductionStageEmployeeCommand request, CancellationToken cancellationToken)
    {
        var po = await _db.ProductionOrders.FirstOrDefaultAsync(x => x.Id == request.ProductionOrderId, cancellationToken)
            ?? throw new NotFoundException("Production order not found");
        if (po.Status != request.Stage)
            throw new DomainException("Cannot add operation: stage does not match order status.");
        
        if (request.Stage == ProductionOrderStatus.Cutting)
        {
            if (po.QtyCut + request.Qty > po.QtyPlanned)
                throw new DomainException("Cannot cut more than planned.");
            var op = new CuttingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.Qty, request.Date);
            _db.CuttingOperations.Add(op);
            po.AddCut(request.Qty);
        }
        else if (request.Stage == ProductionOrderStatus.Sewing)
        {
            if (po.QtySewn + request.Qty > po.QtyCut)
                throw new DomainException("Cannot sew more than cut.");
            var op = new SewingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.Qty, request.HoursWorked!.Value, request.Date);
            _db.SewingOperations.Add(op);
            po.AddSewn(request.Qty);
        }
        else if (request.Stage == ProductionOrderStatus.Packaging)
        {
            if (po.QtyPacked + request.Qty > po.QtySewn)
                throw new DomainException("Cannot pack more than sewn.");
            var op = new PackagingOperationEntity(request.ProductionOrderId, request.EmployeeId, request.QtyPlanned, request.Qty, request.Date);
            _db.PackagingOperations.Add(op);
            po.AddPacked(request.Qty);
        }
        else
        {
            throw new DomainException("Invalid stage for operation.");
        }
        await _db.SaveChangesAsync(cancellationToken);
    }
}
