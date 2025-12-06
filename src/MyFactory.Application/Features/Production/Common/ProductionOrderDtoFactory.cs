using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Domain.Entities.Employees;
using MyFactory.Domain.Entities.Production;
using MyFactory.Domain.Entities.Specifications;
using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Application.Features.Production.Common;

internal static class ProductionOrderDtoFactory
{
    public static async Task<ProductionOrderDto> CreateAsync(
        IApplicationDbContext context,
        ProductionOrder order,
        CancellationToken cancellationToken)
    {
        var result = await CreateAsync(context, new[] { order }, cancellationToken);
        return result.First();
    }

    public static async Task<IReadOnlyCollection<ProductionOrderDto>> CreateAsync(
        IApplicationDbContext context,
        IReadOnlyCollection<ProductionOrder> orders,
        CancellationToken cancellationToken)
    {
        if (orders.Count == 0)
        {
            return Array.Empty<ProductionOrderDto>();
        }

        var lookups = await LoadLookupsAsync(context, orders, cancellationToken);

        return orders
            .Select(order => ProductionOrderDto.FromEntity(order, lookups.Specifications, lookups.Workshops, lookups.Employees))
            .ToList();
    }

    private static async Task<(
        IReadOnlyDictionary<Guid, Specification> Specifications,
        IReadOnlyDictionary<Guid, Workshop> Workshops,
        IReadOnlyDictionary<Guid, Employee> Employees)> LoadLookupsAsync(
        IApplicationDbContext context,
        IReadOnlyCollection<ProductionOrder> orders,
        CancellationToken cancellationToken)
    {
        var specificationIds = orders
            .Select(order => order.SpecificationId)
            .Distinct()
            .ToList();

        var workshopIds = orders
            .SelectMany(order => order.Allocations.Select(allocation => allocation.WorkshopId)
                .Concat(order.Stages.Select(stage => stage.WorkshopId)))
            .Distinct()
            .ToList();

        var employeeIds = orders
            .SelectMany(order => order.Stages.SelectMany(stage => stage.Assignments.Select(assignment => assignment.EmployeeId)))
            .Distinct()
            .ToList();

        var specifications = specificationIds.Count == 0
            ? new Dictionary<Guid, Specification>()
            : await context.Specifications
                .Where(specification => specificationIds.Contains(specification.Id))
                .ToDictionaryAsync(specification => specification.Id, cancellationToken);

        var workshops = workshopIds.Count == 0
            ? new Dictionary<Guid, Workshop>()
            : await context.Workshops
                .Where(workshop => workshopIds.Contains(workshop.Id))
                .ToDictionaryAsync(workshop => workshop.Id, cancellationToken);

        var employees = employeeIds.Count == 0
            ? new Dictionary<Guid, Employee>()
            : await context.Employees
                .Where(employee => employeeIds.Contains(employee.Id))
                .ToDictionaryAsync(employee => employee.Id, cancellationToken);

        return (specifications, workshops, employees);
    }
}
