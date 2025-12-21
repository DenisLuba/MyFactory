using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.PostInventoryReceipt;

public sealed record PostInventoryReceiptCommand(Guid ReceiptId, Guid WarehouseId) : IRequest<InventoryReceiptDto>;
