using MediatR;
using System;
using MyFactory.Domain.Entities.Production;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrders;

public sealed record GetProductionOrdersQuery 
(
    Guid? SearchSaleOrderId = null,
    string? SearchProductionOrderNumber = null,
    string? SearchSaleOrderNumber = null,
    string? SearchCustomerName = null,
    string? SearchProductName = null,
    string? SortBy = null,     
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    ProductionOrderStatus? Status = null
) : IRequest<ListDto<ProductionOrderListItemDto>>;