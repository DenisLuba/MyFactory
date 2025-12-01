using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFactory.WebApi.Contracts.Products;
using MyFactory.WebApi.SwaggerExamples.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.Controllers;

[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private static readonly Guid PajamaId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DressId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ShirtId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    private static readonly IReadOnlyList<ProductCardResponse> Products = new List<ProductCardResponse>
    {
        new(PajamaId, "SP-001", "Пижама женская", 2.5, "Лёгкая летняя пижама", 2),
        new(DressId, "DR-215", "Платье трикотажное", 3.2, "Платье миди в базовой расцветке", 3),
        new(ShirtId, "SH-087", "Рубашка мужская", 4.1, "Классическая хлопковая рубашка", 1)
    };

    private static readonly IReadOnlyDictionary<Guid, string> ProductStatuses = new Dictionary<Guid, string>
    {
        [PajamaId] = "Активен",
        [DressId] = "Черновик",
        [ShirtId] = "В архиве"
    };

    private static readonly Dictionary<Guid, List<ProductBomItemResponse>> BomItems = new()
    {
        [PajamaId] = new List<ProductBomItemResponse>
        {
            new(Guid.Parse("a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1"), "Ткань хлопок", 2.4, "м", 380m, 912m),
            new(Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), "Резинка эластичная", 1.2, "м", 52m, 62.4m)
        },
        [DressId] = new List<ProductBomItemResponse>
        {
            new(Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3"), "Ткань вискоза", 3.1, "м", 420m, 1302m)
        }
    };

    private static readonly Dictionary<Guid, List<ProductOperationItemResponse>> OperationItems = new()
    {
        [PajamaId] = new List<ProductOperationItemResponse>
        {
            new(Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"), "Раскрой комплектов", 18.5, 265m),
            new(Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5"), "Пошив изделия", 42.5, 590m)
        },
        [DressId] = new List<ProductOperationItemResponse>
        {
            new(Guid.Parse("f6f6f6f6-f6f6-f6f6-f6f6-f6f6f6f6f6f6"), "Пошив платья", 48.0, 720m)
        }
    };

    [HttpGet]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductsListResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ProductsListResponse>), StatusCodes.Status200OK)]
    public IActionResult List()
        => Ok(Products.Select(MapToList));

    [HttpGet("{id}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductCardResponseExample))]
    [ProducesResponseType(typeof(ProductCardResponse), StatusCodes.Status200OK)]
    public IActionResult Get(Guid id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id) ?? Products.First();
        return Ok(product);
    }

    [HttpPut("{id}")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductUpdateRequest), typeof(ProductUpdateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductUpdateResponseExample))]
    [ProducesResponseType(typeof(ProductUpdateResponse), StatusCodes.Status200OK)]
    public IActionResult Update(Guid id, [FromBody] ProductUpdateRequest request)
    {
        var response = new ProductUpdateResponse(id, "Updated");
        return Ok(response);
    }

    [HttpGet("{id}/bom")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductBomItemResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ProductBomItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetBom(Guid id)
    {
        var items = BomItems.TryGetValue(id, out var bom)
            ? bom
            : BomItems.Values.FirstOrDefault() ?? new List<ProductBomItemResponse>();
        return Ok(items);
    }

    [HttpPost("{id}/bom")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductBomCreateRequest), typeof(ProductBomCreateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ProductBomItemResponseExample))]
    [ProducesResponseType(typeof(ProductBomItemResponse), StatusCodes.Status201Created)]
    public IActionResult AddBom(Guid id, [FromBody] ProductBomCreateRequest request)
    {
        if (!TryEnsureProductExists(id))
        {
            return NotFound();
        }

        var total = (decimal)request.Qty * request.Price;
        var item = new ProductBomItemResponse(Guid.NewGuid(), request.Material, request.Qty, request.Unit, request.Price, total);
        var list = GetOrCreateBomList(id);
        list.Add(item);

        return CreatedAtAction(nameof(GetBom), new { id }, item);
    }

    [HttpDelete("{id}/bom/{lineId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductBomDeleteResponseExample))]
    [ProducesResponseType(typeof(ProductBomDeleteResponse), StatusCodes.Status200OK)]
    public IActionResult DeleteBom(Guid id, Guid lineId)
    {
        if (!BomItems.TryGetValue(id, out var list))
        {
            return NotFound(new ProductBomDeleteResponse(lineId, "NotFound"));
        }

        var index = list.FindIndex(item => item.Id == lineId);
        if (index < 0)
        {
            return NotFound(new ProductBomDeleteResponse(lineId, "NotFound"));
        }

        list.RemoveAt(index);
        return Ok(new ProductBomDeleteResponse(lineId, "Deleted"));
    }

    [HttpGet("{id}/operations")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductOperationItemResponseExample))]
    [ProducesResponseType(typeof(IEnumerable<ProductOperationItemResponse>), StatusCodes.Status200OK)]
    public IActionResult GetOperations(Guid id)
    {
        var items = OperationItems.TryGetValue(id, out var ops)
            ? ops
            : OperationItems.Values.FirstOrDefault() ?? new List<ProductOperationItemResponse>();
        return Ok(items);
    }

    [HttpPost("{id}/operations")]
    [Consumes("application/json")]
    [SwaggerRequestExample(typeof(ProductOperationCreateRequest), typeof(ProductOperationCreateRequestExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ProductOperationItemResponseExample))]
    [ProducesResponseType(typeof(ProductOperationItemResponse), StatusCodes.Status201Created)]
    public IActionResult AddOperation(Guid id, [FromBody] ProductOperationCreateRequest request)
    {
        if (!TryEnsureProductExists(id))
        {
            return NotFound();
        }

        var item = new ProductOperationItemResponse(Guid.NewGuid(), request.Operation, request.Minutes, request.Cost);
        var list = GetOrCreateOperationList(id);
        list.Add(item);

        return CreatedAtAction(nameof(GetOperations), new { id }, item);
    }

    [HttpDelete("{id}/operations/{lineId}")]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProductOperationDeleteResponseExample))]
    [ProducesResponseType(typeof(ProductOperationDeleteResponse), StatusCodes.Status200OK)]
    public IActionResult DeleteOperation(Guid id, Guid lineId)
    {
        if (!OperationItems.TryGetValue(id, out var list))
        {
            return NotFound(new ProductOperationDeleteResponse(lineId, "NotFound"));
        }

        var index = list.FindIndex(item => item.Id == lineId);
        if (index < 0)
        {
            return NotFound(new ProductOperationDeleteResponse(lineId, "NotFound"));
        }

        list.RemoveAt(index);
        return Ok(new ProductOperationDeleteResponse(lineId, "Deleted"));
    }

    private static ProductsListResponse MapToList(ProductCardResponse source)
    {
        var status = ProductStatuses.TryGetValue(source.Id, out var value) ? value : "Активен";
        return new ProductsListResponse(source.Id, source.Sku, source.Name, source.PlanPerHour, status, source.ImageCount);
    }

    private static bool TryEnsureProductExists(Guid id) => Products.Any(product => product.Id == id);

    private static List<ProductBomItemResponse> GetOrCreateBomList(Guid productId)
    {
        if (!BomItems.TryGetValue(productId, out var list))
        {
            list = new List<ProductBomItemResponse>();
            BomItems[productId] = list;
        }

        return list;
    }

    private static List<ProductOperationItemResponse> GetOrCreateOperationList(Guid productId)
    {
        if (!OperationItems.TryGetValue(productId, out var list))
        {
            list = new List<ProductOperationItemResponse>();
            OperationItems[productId] = list;
        }

        return list;
    }
}
