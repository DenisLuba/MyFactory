using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.CancelInventoryReceipt;

public sealed record CancelInventoryReceiptCommand(Guid ReceiptId) : IRequest<InventoryReceiptDto>;
