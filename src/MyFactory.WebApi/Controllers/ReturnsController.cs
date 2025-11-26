using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Returns;
using MyFactory.WebApi.SwaggerExamples.Returns;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/returns")]
public class ReturnsController : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(ReturnsCreateRequest), typeof(ReturnsCreateRequestExample))]
    [SwaggerResponseExample(201, typeof(ReturnsCreateResponseExample))]
    [ProducesResponseType(typeof(ReturnsCreateResponse), StatusCodes.Status201Created)]
    public IActionResult CreateReturn([FromBody] ReturnsCreateRequest request)
        => Created(
            "",
            new ReturnsCreateResponse(
                ReturnId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Status: ReturnStatus.Accepted
            )
        );
}
