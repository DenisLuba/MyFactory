using MyFactory.Domain.Entities.Workshops;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public sealed class ActiveWorkshopsWithExpensesSpecification : Specification<Workshop>
{
    public ActiveWorkshopsWithExpensesSpecification(bool activeOnly = true)
        : base(workshop => !activeOnly || workshop.IsActive)
    {
        AddInclude(workshop => workshop.ExpenseHistory);
        AsNoTrackingQuery();
    }
}
