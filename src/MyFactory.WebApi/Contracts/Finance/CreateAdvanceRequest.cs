namespace MyFactory.WebApi.Contracts.Finance;

public record CreateAdvanceRequest(
    // TODO: Добавить свойства на основе бизнес-логики
    Guid EmployeeId,
    decimal Amount,
    string Purpose,
    DateTime RequestDate
);