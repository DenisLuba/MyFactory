using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.SalesOrders;
using System.Xml.Linq;

namespace MyFactory.MauiClient.Models.SalesOrders;

public sealed record SalesOrderListItemResponse(
    Guid Id,
    int OrderNumber,
    string CustomerName,
    DateTime OrderDate,
    SalesOrderStatus Status) : ListItemResponse(Id, $"№: {OrderNumber}, заказчик: {CustomerName}");
