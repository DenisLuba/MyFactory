using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Customers;
using MyFactory.WebApi.SwaggerExamples.Customers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private static readonly IReadOnlyList<CustomerLookupResponse> Customers = new List<CustomerLookupResponse>
    {
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "ООО \"Текстиль\"", "Сети на северо-западе"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "ИП Клиент1", "Оптовые поставки"),
        new(Guid.Parse("33333333-3333-3333-3333-333333333333"), "ООО \"Ателье Люкс\"", "Розничные магазины"),
        new(Guid.Parse("44444444-4444-4444-4444-444444444444"), "ИП Пижама", "Онлайн продажи"),
        new(Guid.Parse("55555555-5555-5555-5555-555555555555"), "ООО \"Дом Ткани\"", "Новые клиенты"),
        new(Guid.Parse("66666666-6666-6666-6666-666666666666"), "АО \"Стиль-Мода\"", "Федеральная сеть"),
        new(Guid.Parse("77777777-7777-7777-7777-777777777777"), "ООО \"Мягкий Дом\"", "HoReCa"),
        new(Guid.Parse("88888888-8888-8888-8888-888888888888"), "ИП Хлопков", "Региональные поставки")
    };

    // GET /api/customers/search
    [HttpGet("search")]
    [Produces("application/json")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomerLookupResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<CustomerLookupResponse>), StatusCodes.Status200OK)]
    public IActionResult Search([FromQuery] string? query)
    {
        var term = query?.Trim();
        var result = Customers
            .Where(c => string.IsNullOrWhiteSpace(term)
                || c.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                || c.CustomerId.ToString().Contains(term, StringComparison.OrdinalIgnoreCase))
            .Take(10)
            .ToList();

        return Ok(result);
    }
}
