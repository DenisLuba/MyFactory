namespace MyFactory.MauiClient.Models.Workshops;

public record WorkshopUpdateRequest(
    string Name,
    WorkshopType Type,
    WorkshopStatus Status
);
