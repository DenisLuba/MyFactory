using FluentValidation;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.AddProductionStageEmployee;

public sealed class AddProductionStageEmployeeCommandValidator : AbstractValidator<AddProductionStageEmployeeCommand>
{
    public AddProductionStageEmployeeCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.Stage).IsInEnum();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.AssignedQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CompletedQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Date).NotEmpty();

        RuleFor(x => x)
            .Must(x => x.CompletedQty <= x.AssignedQty)
            .WithMessage("Completed quantity cannot exceed assigned quantity.");

        RuleFor(x => x.CompletedQty)
            .Equal(0)
            .When(x => x.Stage == ProductionStage.Sewing)
            .WithMessage("Completed quantity for sewing assignments is derived from sewing operations and must be zero.");
    }
}

