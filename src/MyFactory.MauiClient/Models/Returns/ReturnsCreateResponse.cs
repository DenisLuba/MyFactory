namespace MyFactory.MauiClient.Models.Returns;

public record ReturnsCreateResponse(
    Guid ReturnId,
    ReturnStatus Status
);

public enum ReturnStatus
{
    Accepted,
    Rejected
}

