using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.DTOs.Production;

public sealed record ProductionStageDto(
    Guid Id,
    Guid ProductionOrderId,
    Guid WorkshopId,
    string WorkshopName,
    string StageType,
    decimal QuantityIn,
    decimal QuantityOut,
    string Status,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    DateTime? RecordedAt,
    IReadOnlyCollection<WorkerAssignmentDto> Assignments)
{
    public static ProductionStageDto FromEntity(
        ProductionStage stage,
        IReadOnlyDictionary<Guid, Workshop> workshops,
        IReadOnlyDictionary<Guid, Employee> employees)
    {
        var workshopName = workshops.TryGetValue(stage.WorkshopId, out var workshop)
            ? workshop.Name
            : string.Empty;

        var assignmentDtos = stage.Assignments
            .Select(assignment => WorkerAssignmentDto.FromEntity(assignment, employees))
            .ToList();

        return new ProductionStageDto(
            stage.Id,
            stage.ProductionOrderId,
            stage.WorkshopId,
            workshopName,
            stage.StageType,
            stage.QuantityIn,
            stage.QuantityOut,
            stage.Status,
            stage.StartedAt,
            stage.CompletedAt,
            stage.RecordedAt,
            assignmentDtos);
    }
}
