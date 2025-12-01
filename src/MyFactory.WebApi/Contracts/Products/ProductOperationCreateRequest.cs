namespace MyFactory.WebApi.Contracts.Products;

public record ProductOperationCreateRequest(
    string Operation,
    double Minutes,
    decimal Cost
);
