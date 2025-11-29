namespace MyFactory.MauiClient.UIModels.Production;

// Сменное задание
public record ShiftPlanItem(
    string Employee,
    string Product,
    string Date,
    int PlannedQuantity
);