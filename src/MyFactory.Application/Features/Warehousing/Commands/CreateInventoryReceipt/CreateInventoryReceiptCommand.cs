using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.CreateInventoryReceipt;

public sealed record CreateInventoryReceiptCommand(string ReceiptNumber, Guid SupplierId, DateTime ReceiptDate) : IRequest<InventoryReceiptDto>;
