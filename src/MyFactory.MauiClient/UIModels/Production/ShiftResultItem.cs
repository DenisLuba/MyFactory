namespace MyFactory.MauiClient.UIModels.Production;

public record ShiftResultItem(
    string Employee,
    string Product,
    int ActualQuantity,
    double HoursWorked,
    string Bonus
);