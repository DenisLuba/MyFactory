using System;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class CustomerReturnWithItemsSpecification : Specification<CustomerReturn>
{
    public CustomerReturnWithItemsSpecification(Guid returnId)
        : base(returnEntity => returnEntity.Id == returnId)
    {
        AddInclude(returnEntity => returnEntity.Customer!);
        AddInclude(returnEntity => returnEntity.Items);
        AddInclude($"{nameof(CustomerReturn.Items)}.{nameof(CustomerReturnItem.Specification)}");
        AsNoTrackingQuery();
    }
}
