namespace MyFactory.WebApi.Contracts.FinishedGoods;

public record ReceiptFinishedGoodsResponse(Guid ReceiptId, FinishedGoodsStatus Status);

