using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.WorkshopExpenses;
using MyFactory.WebApi.SwaggerExamples.WorkshopExpenses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/workshops/expenses")]
[Produces("application/json")]
public class WorkshopExpensesController : ControllerBase
{
    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkshopExpensesListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<WorkshopExpenseListResponse>), StatusCodes.Status200OK)]
    public IActionResult List([FromQuery] Guid? workshopId = null)
    {
        var expenses = new List<WorkshopExpenseListResponse>
        {
            new(
                Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                "Крой",
                1500m,
                new DateTime(2025, 1, 1),
                null
            ),
            new(
                Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                "Пошив",
                2200m,
                new DateTime(2025, 2, 1),
                new DateTime(2025, 12, 31)
            )
        };

        if (workshopId.HasValue)
        {
            expenses = expenses.FindAll(x => x.WorkshopId == workshopId);
        }

        return Ok(expenses);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkshopExpenseGetResponseExample))]
    [ProducesResponseType(typeof(WorkshopExpenseGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
        => Ok(
            new WorkshopExpenseGetResponse(
                id,
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                1500m,
                new DateTime(2025, 1, 1),
                null
            )
        );

    [HttpPost]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(WorkshopExpenseCreateRequest), typeof(WorkshopExpenseCreateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(WorkshopExpenseCreateResponseExample))]
    [ProducesResponseType(typeof(WorkshopExpenseCreateResponse), StatusCodes.Status201Created)]
    public IActionResult Create([FromBody] WorkshopExpenseCreateRequest request)
        => Created(
            string.Empty,
            new WorkshopExpenseCreateResponse(Guid.NewGuid())
        );

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(WorkshopExpenseUpdateRequest), typeof(WorkshopExpenseUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(WorkshopExpenseUpdateResponseExample))]
    [ProducesResponseType(typeof(WorkshopExpenseUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] WorkshopExpenseUpdateRequest request)
        => Ok(new WorkshopExpenseUpdateResponse(id));
}
