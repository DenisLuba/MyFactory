using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.UpdateProductionStageEmployee;

public sealed class UpdateProductionStageEmployeeCommandValidator : AbstractValidator<UpdateProductionStageEmployeeCommand>
{
    public UpdateProductionStageEmployeeCommandValidator()
    {
        RuleFor(x => x.AssignmentId).NotEmpty();
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.AssignedQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CompletedQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Stage).IsInEnum();

        RuleFor(x => x)
            .Must(x => x.CompletedQty <= x.AssignedQty)
            .WithMessage("Completed quantity cannot exceed assigned quantity.");
    }
}
