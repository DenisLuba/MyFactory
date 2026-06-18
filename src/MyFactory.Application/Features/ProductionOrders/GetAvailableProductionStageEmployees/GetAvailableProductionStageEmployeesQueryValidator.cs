using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetAvailableProductionStageEmployees;

public sealed class GetAvailableProductionStageEmployeesQueryValidator
    : AbstractValidator<GetAvailableProductionStageEmployeesQuery>
{
    public GetAvailableProductionStageEmployeesQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.Stage).IsInEnum();
    }
}
