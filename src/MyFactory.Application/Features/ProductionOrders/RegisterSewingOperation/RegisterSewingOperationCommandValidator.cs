using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.RegisterSewingOperation;

public sealed class RegisterSewingOperationCommandValidator
    : AbstractValidator<RegisterSewingOperationCommand>
{
    public RegisterSewingOperationCommandValidator()
    {
        RuleFor(x => x.ProductionOrderId).NotEmpty();
        RuleFor(x => x.AssignmentId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.QtySewn).GreaterThan(0);
        RuleFor(x => x.HoursWorked).GreaterThan(0);
        RuleFor(x => x.OperationDate).NotEmpty();
    }
}
