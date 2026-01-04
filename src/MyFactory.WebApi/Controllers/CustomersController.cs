using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyFactory.Application.Features.Customers.CreateCustomer;
using MyFactory.Application.Features.Customers.DeactivateCustomer;
using MyFactory.Application.Features.Customers.GetCustomerCard;
using MyFactory.Application.Features.Customers.GetCustomerDetails;
using MyFactory.Application.Features.Customers.GetCustomers;
using MyFactory.Application.Features.Customers.UpdateCustomer;
using MyFactory.WebApi.Contracts.Customers;
using MyFactory.WebApi.SwaggerExamples.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/customers")]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // -------------------------
    //  LIST
    // -------------------------
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerListItemResponse>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(CustomerListResponseExample))]
    public async Task<IActionResult> GetList()
    {
        var result = await _mediator.Send(new GetCustomersQuery());
        var response = result
            .Select(x => new CustomerListItemResponse(x.Id, x.Name, x.Phone, x.Email, x.Address))
            .ToList();
        return Ok(response);
    }

    // -------------------------
    //  DETAILS
    // -------------------------
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDetailsResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(CustomerDetailsResponseExample))]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        var dto = await _mediator.Send(new GetCustomerDetailsQuery(id));
        var response = new CustomerDetailsResponse(dto.Id, dto.Name, dto.Phone, dto.Email, dto.Address);
        return Ok(response);
    }

    // -------------------------
    //  CARD
    // -------------------------
    [HttpGet("{id:guid}/card")]
    [ProducesResponseType(typeof(CustomerCardResponse), StatusCodes.Status200OK)]
    [SwaggerResponseExample(200, typeof(CustomerCardResponseExample))]
    public async Task<IActionResult> GetCard(Guid id)
    {
        var dto = await _mediator.Send(new GetCustomerCardQuery(id));
        var response = new CustomerCardResponse(
            dto.Id,
            dto.Name,
            dto.Phone,
            dto.Email,
            dto.Address,
            dto.Orders.Select(o => new CustomerOrderItemResponse(o.Id, o.OrderNumber, o.OrderDate, o.Status)).ToList());
        return Ok(response);
    }

    // -------------------------
    //  CREATE
    // -------------------------
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CreateCustomerResponse), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateCustomerRequest), typeof(CreateCustomerRequestExample))]
    [SwaggerResponseExample(201, typeof(CreateCustomerResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest req)
    {
        var id = await _mediator.Send(new CreateCustomerCommand(req.Name, req.Phone, req.Email, req.Address));
        return Created(string.Empty, new CreateCustomerResponse(id));
    }

    // -------------------------
    //  UPDATE
    // -------------------------
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [SwaggerRequestExample(typeof(UpdateCustomerRequest), typeof(UpdateCustomerRequestExample))]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest req)
    {
        await _mediator.Send(new UpdateCustomerCommand(id, req.Name, req.Phone, req.Email, req.Address));
        return NoContent();
    }

    // -------------------------
    //  DEACTIVATE
    // -------------------------
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _mediator.Send(new DeactivateCustomerCommand(id));
        return NoContent();
    }
}

