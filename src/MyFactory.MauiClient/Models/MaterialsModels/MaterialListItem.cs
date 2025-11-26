namespace MyFactory.MauiClient.Models;

public record MaterialListItem(
    Guid Id,
    string Code,
    string Name,
    decimal LastPrice,
    string Unit
);

