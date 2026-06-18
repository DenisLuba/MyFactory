using FluentValidation;

namespace MyFactory.Application.Features.Departments.GetDepartments;

public class GetDepartmentsQueryValidator : AbstractValidator<GetDepartmentsQuery>
{
    public GetDepartmentsQueryValidator()
    {
        // No specific validation rules for this query as it has only one boolean property.
        // This class is defined for consistency and future extensibility.
    }
}

