namespace MyFactory.MauiClient.UIModels.FinishedGoods;

// Оприходование готовой продукции 
public record FinishedGoodsReceiptItem(
    string Product,
    int Quantity,
    string Date,
    string Warehouse,
    decimal TotalAmount
);