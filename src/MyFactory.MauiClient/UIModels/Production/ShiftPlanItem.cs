namespace MyFactory.MauiClient.UIModels.Production;

public record ShiftPlanItem(
    string Employee,
    string Product,
    string Date,
    int PlannedQuantity
);