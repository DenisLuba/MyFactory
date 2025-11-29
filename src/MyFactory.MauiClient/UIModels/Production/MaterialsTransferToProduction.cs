namespace MyFactory.MauiClient.UIModels.Production;

// Перемещение материалов в производство (документ)
public record MaterialsTransferToProduction(
    string DocumentId,
    string Date,
    string From,
    string To
);

