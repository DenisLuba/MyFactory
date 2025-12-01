namespace MyFactory.MauiClient.Models.Products;

public record ProductBomCreateRequest(
    string Material,
    double Qty,
    string Unit,
    decimal Price
);
