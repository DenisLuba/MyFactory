namespace MyFactory.MauiClient.UIModels.Reference;

// Сотрудники
public record EmployeeItem(
    string Id,
    string FullName,
    string Position,
    int Grade,
    decimal HourlyRate,
    int PremiumPercentage
);