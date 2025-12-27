using FluentValidation;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed class AddProductionStageEmployeeCommandValidator : AbstractValidator<AddProductionStageEmployeeCommand>
{
    public AddProductionStageEmployeeCommandValidator()
    {
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
