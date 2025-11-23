namespace MyFactory.WebApi.Contracts.Materials;

public record AddMaterialPriceResponse(MaterialPriceStatus Status, Guid Id);

public enum MaterialPriceStatus
{
    PriceAdded, 
    PriceRemoved, 
    PriceUpdated,
}