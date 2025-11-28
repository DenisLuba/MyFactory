namespace MyFactory.MauiClient.Models.Finance;

public record CreateAdvanceRequest(
    Guid EmployeeId,
    decimal Amount,
    string Purpose,
    DateTime RequestDate);
