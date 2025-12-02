using System;

namespace MyFactory.MauiClient.UIModels.Warehouse;

public record MaterialLookupItem(Guid MaterialId, string Code, string Name, string Unit, decimal LastPrice)
{
    public string DisplayName => string.IsNullOrWhiteSpace(Code) ? Name : $"{Code} — {Name}";
}
