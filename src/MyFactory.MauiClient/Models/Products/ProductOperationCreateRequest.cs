namespace MyFactory.MauiClient.Models.Products;

public record ProductOperationCreateRequest(
    string Operation,
    double Minutes,
    decimal Cost
);
