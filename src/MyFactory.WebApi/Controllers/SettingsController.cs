using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Settings;
using MyFactory.WebApi.SwaggerExamples.Settings;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(SettingsGetResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<SettingsGetResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
        => Ok(new[]
        {
            new SettingsGetResponse(
                Key: "StandardShiftHours",
                Value: "8",
                Description: "Стандартная длительность смены (часы)"
            ),
            new SettingsGetResponse(
                Key: "Currency",
                Value: "₽",
                Description: "Валюта системы"
            )
        });

    [HttpGet("{key}")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(SettingsGetResponseExample))]
    [ProducesResponseType(typeof(SettingsGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(string key)
        => Ok(
            new SettingsGetResponse(
                Key: key,
                Value: key switch
                {
                    "StandardShiftHours" => "8",
                    "Currency" => "₽",
                    _ => "unknown"
                },
                Description: "..."
            )
        );

    [HttpPut("{key}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [SwaggerRequestExample(typeof(SettingsUpdateRequest), typeof(SettingsUpdateRequestExample))]
    [SwaggerResponseExample(200, typeof(SettingsUpdateResponseExample))]
    [ProducesResponseType(typeof(SettingsUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(string key, [FromBody] SettingsUpdateRequest dto)
        => Ok(
            new SettingsUpdateResponse(
                Key: key,
                Status: SettingsUpdateStatus.Updated
            )
        );
}

