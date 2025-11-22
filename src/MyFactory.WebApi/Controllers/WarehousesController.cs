using Microsoft.AspNetCore.Mvc;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehousesController : ControllerBase
{
    [HttpGet]
    public IActionResult List()
        => Ok(new[] 
        {
            new { id="st-001", name="Основной склад", type="materials", location="ул. Заводская, 1" },
            new { id="st-003", name="Склад ГП", type="finished", location="ул. Заводская, 2" }
        });

    [HttpGet("{id}")]
    public IActionResult Get(string id) 
        => Ok(new 
        { 
            id,
            name = "Основной склад",
            type = "materials",
            location = "ул. Заводская, 1"
        });

    [HttpPost]
    public IActionResult Create([FromBody] object dto) => Created("", new { status = "created" });

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] object dto) => Ok(new { status = "updated", id });
}
