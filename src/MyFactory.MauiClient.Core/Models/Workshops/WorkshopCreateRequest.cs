namespace MyFactory.MauiClient.Models.Workshops;

public record WorkshopCreateRequest(
    string Name,
    WorkshopType Type,
    WorkshopStatus Status
);
