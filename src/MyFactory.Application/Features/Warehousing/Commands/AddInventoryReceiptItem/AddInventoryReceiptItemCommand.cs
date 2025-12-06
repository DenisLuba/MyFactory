using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.AddInventoryReceiptItem;

public sealed record AddInventoryReceiptItemCommand(Guid ReceiptId, Guid MaterialId, decimal Quantity, decimal UnitPrice) : IRequest<InventoryReceiptDto>;
