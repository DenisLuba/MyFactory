using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.UpdateWarehouse;

public sealed record UpdateWarehouseCommand(Guid Id, string? Name, string? Type, string? Location) : IRequest<WarehouseDto>;
