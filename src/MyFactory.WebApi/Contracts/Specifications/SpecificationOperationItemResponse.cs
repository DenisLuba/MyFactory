namespace MyFactory.WebApi.Contracts.Specifications;

public record SpecificationOperationItemResponse(
    Guid Id,
    string Operation,
    double Minutes,
    decimal Cost
);
