namespace MyFactory.MauiClient.UIModels.Production;

// Распределение по участкам
public record WorkshopDistribution(
    string Stage,
    string WorkshopName,
    int QuantityInWork,
    int HandedOverToEmployees,
    int Left
);

