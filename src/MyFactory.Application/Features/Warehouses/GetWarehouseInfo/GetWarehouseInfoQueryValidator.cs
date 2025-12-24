using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.GetWarehouseInfo;

public sealed class GetWarehouseInfoQueryValidator
    : AbstractValidator<GetWarehouseInfoQuery>
{
    public GetWarehouseInfoQueryValidator()
    {
        RuleFor(x => x.WarehouseId)
            .NotEmpty();
    }
}