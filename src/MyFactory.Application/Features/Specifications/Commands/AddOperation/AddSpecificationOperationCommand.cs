using System;
using MediatR;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.AddOperation;

public sealed record AddSpecificationOperationCommand(
    Guid SpecificationId,
    Guid OperationId,
    Guid WorkshopId,
    decimal TimeMinutes,
    decimal OperationCost) : IRequest<SpecificationOperationResultDto>;
