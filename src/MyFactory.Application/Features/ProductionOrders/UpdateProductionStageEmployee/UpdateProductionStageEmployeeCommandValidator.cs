using FluentValidation;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;

public sealed class UpdateProductionStageEmployeeCommandValidator : AbstractValidator<UpdateProductionStageEmployeeCommand>
{
    public UpdateProductionStageEmployeeCommandValidator()
    {
        RuleFor(x => x.OperationId).NotEmpty();
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.Qty).GreaterThan(0);
        RuleFor(x => x.Stage).IsInEnum();
        RuleFor(x => x.Date).NotEmpty();
        When(x => x.Stage == ProductionOrderStatus.Sewing, () =>
        {
            RuleFor(x => x.HoursWorked).NotNull().GreaterThan(0);
        });
    }
}
