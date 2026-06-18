namespace MyFactory.WebApi.Contracts.Products;

internal static class ProductStatusMapper
{
    public static ProductStatus ToContract(this Domain.Entities.Products.ProductStatus status) => status switch
    {
        Domain.Entities.Products.ProductStatus.Active => ProductStatus.Active,
        Domain.Entities.Products.ProductStatus.Inactive => ProductStatus.Inactive,
        Domain.Entities.Products.ProductStatus.Development => ProductStatus.Development,
        Domain.Entities.Products.ProductStatus.Discontinued => ProductStatus.Discontinued,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown product status")
    };

    public static Domain.Entities.Products.ProductStatus ToDomain(this ProductStatus status) => status switch
    {
        ProductStatus.Active => Domain.Entities.Products.ProductStatus.Active,
        ProductStatus.Inactive => Domain.Entities.Products.ProductStatus.Inactive,
        ProductStatus.Development => Domain.Entities.Products.ProductStatus.Development,
        ProductStatus.Discontinued => Domain.Entities.Products.ProductStatus.Discontinued,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unknown product status")
    };
}

