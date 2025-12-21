using System;
using MediatR;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.CreateWarehouse;

public sealed record CreateWarehouseCommand(string Name, string Type, string Location) : IRequest<WarehouseDto>;
