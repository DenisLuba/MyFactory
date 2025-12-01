using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Employees;
using MyFactory.WebApi.Services.Employees;
using MyFactory.WebApi.SwaggerExamples.Employees;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/employees")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repository;

    public EmployeesController(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EmployeeListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<EmployeeListResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll([FromQuery] string? role = null)
    {
        var result = _repository.GetAll(role);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EmployeeCardResponseExample))]
    [ProducesResponseType(typeof(EmployeeCardResponse), StatusCodes.Status200OK)]
    public IActionResult GetById(Guid id)
    {
        var employee = _repository.GetById(id);
        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(EmployeeUpdateRequest), typeof(EmployeeUpdateRequestExample))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Update(Guid id, [FromBody] EmployeeUpdateRequest request)
    {
        var updated = _repository.Update(id, request);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}
