namespace MyFactory.WebApi.Contracts.Products;

public record ProductBomCreateRequest(
    string Material,
    double Qty,
    string Unit,
    decimal Price
);
