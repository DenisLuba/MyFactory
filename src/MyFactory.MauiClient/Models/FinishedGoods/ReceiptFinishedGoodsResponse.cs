namespace MyFactory.MauiClient.Models.FinishedGoods;

public record ReceiptFinishedGoodsResponse(
    Guid ReceiptId,
    FinishedGoodsStatus Status);
