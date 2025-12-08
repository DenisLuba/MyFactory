using System;
using FluentValidation;

namespace MyFactory.Application.Features.Inventory.Queries.GetInventoryHistory;

public sealed class GetInventoryHistoryQueryValidator : AbstractValidator<GetInventoryHistoryQuery>
{
    public GetInventoryHistoryQueryValidator()
    {
        RuleFor(query => query.MaterialId).NotEmpty();

        RuleFor(query => query.WarehouseId)
            .Must(id => id is null || id.Value != Guid.Empty)
            .WithMessage("Warehouse id must be a valid GUID when provided.");

        RuleFor(query => query)
            .Must(query => !query.FromDate.HasValue || !query.ToDate.HasValue || query.FromDate <= query.ToDate)
            .WithMessage("FromDate must be earlier than or equal to ToDate.");
    }
}
