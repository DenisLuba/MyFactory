using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.CreateInventoryReceipt;

public sealed record CreateInventoryReceiptCommand(string ReceiptNumber, Guid SupplierId, DateOnly ReceiptDate) : IRequest<InventoryReceiptDto>;
