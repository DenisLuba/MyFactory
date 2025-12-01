using System.Collections.Generic;
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
    [SwaggerResponseExample(200, typeof(SettingsListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<SettingsListResponse>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
        => Ok(new[]
        {
            new SettingsListResponse(
                Key: "StandardShiftHours",
                Value: "8",
                Description: "Стандартная длительность смены (часы)"
            ),
            new SettingsListResponse(
                Key: "Currency",
                Value: "₽",
                Description: "Валюта системы"
            )
        });

    [HttpGet("{key}")]
    [Produces("application/json")]
    [SwaggerResponseExample(200, typeof(SettingGetResponseExample))]
    [ProducesResponseType(typeof(SettingGetResponse), StatusCodes.Status200OK)]
    public IActionResult Get(string key)
        => Ok(
            new SettingGetResponse(
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
    [SwaggerRequestExample(typeof(SettingUpdateRequest), typeof(SettingUpdateRequestExample))]
    [SwaggerResponseExample(200, typeof(SettingUpdateResponseExample))]
    [ProducesResponseType(typeof(SettingUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(string key, [FromBody] SettingUpdateRequest dto)
        => Ok(
            new SettingUpdateResponse(
                Key: key,
                Status: SettingUpdateStatus.Updated
            )
        );
}

