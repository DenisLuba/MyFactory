using System;
using System.Collections.Generic;
using MyFactory.Domain.Common;
using MyFactory.Domain.Entities.FinishedGoods;
using MyFactory.Domain.Entities.Identity;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Infrastructure.Persistence.Auditing;

internal static class AuditMetadata
{
    private static readonly ISet<Type> ManualCreationTimestampTypes = new HashSet<Type>
    {
        typeof(Specification),
        typeof(ProductionOrder),
        typeof(PurchaseRequest),
        typeof(Role),
        typeof(User)
    };

    private static readonly ISet<Type> ManualUpdateTimestampTypes = new HashSet<Type>
    {
        typeof(FinishedGoodsInventory)
    };

    public static bool ShouldSetCreatedAt(BaseEntity entity)
    {
        return !ManualCreationTimestampTypes.Contains(entity.GetType());
    }

    public static bool ShouldSetUpdatedAt(BaseEntity entity)
    {
        return !ManualUpdateTimestampTypes.Contains(entity.GetType());
    }
}
