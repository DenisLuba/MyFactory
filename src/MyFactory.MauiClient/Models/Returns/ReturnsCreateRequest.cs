namespace MyFactory.MauiClient.Models.Returns;

public record ReturnsCreateRequest(
    Guid CustomerId,
    Guid SpecificationId,
    int Quantity,
    string Reason,
    DateTime ReturnDate
);

