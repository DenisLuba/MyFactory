using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class GetWarehousesQueryValidator
    : AbstractValidator<GetWarehousesQuery>
{
    public GetWarehousesQueryValidator()
    {
        // пока нечего валидировать
    }
}