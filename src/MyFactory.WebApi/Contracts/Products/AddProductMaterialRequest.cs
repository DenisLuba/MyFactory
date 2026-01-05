namespace MyFactory.WebApi.Contracts.Products;

public record AddProductMaterialRequest(Guid MaterialId, decimal QtyPerUnit);
