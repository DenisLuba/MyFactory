using System;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.DTOs.Specifications;

public sealed record SpecificationOperationItemDto(
    Guid Id,
    Guid OperationId,
    string OperationName,
    Guid WorkshopId,
    string WorkshopName,
    decimal TimeMinutes,
    decimal OperationCost)
{
    public static SpecificationOperationItemDto FromEntity(
        SpecificationOperation operation,
        string operationName,
        string workshopName)
        => new(
            operation.Id,
            operation.OperationId,
            operationName,
            operation.WorkshopId,
            workshopName,
            operation.TimeMinutes,
            operation.OperationCost);
}
