using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionStageEmployees;

public sealed class GetProductionStageEmployeesQueryValidator : AbstractValidator<GetProductionStageEmployeesQuery>
{
    public GetProductionStageEmployeesQueryValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.Stage).IsInEnum();
    }
}
