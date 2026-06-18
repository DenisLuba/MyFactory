using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFactory.Application.DTOs.Shipments;

public sealed record ShipmentDetailsItemDto(
    Guid ShipmentItemId,
    Guid SalesOrderItemId,
    Guid ProductId,
    Guid WarehouseId,
    int Qty,
    decimal UnitPrice
);
