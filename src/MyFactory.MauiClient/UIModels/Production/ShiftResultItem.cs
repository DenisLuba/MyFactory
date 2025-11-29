namespace MyFactory.MauiClient.UIModels.Production;

// Фактические результаты смены
public record ShiftResultItem(
    string Employee,
    string Product,
    int ActualQuantity,
    double HoursWorked,
    string Bonus
);