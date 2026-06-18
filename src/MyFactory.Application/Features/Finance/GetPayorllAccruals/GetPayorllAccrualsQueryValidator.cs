using FluentValidation;

namespace MyFactory.Application.Features.Finance.GetPayrollAccruals;

public sealed class GetPayrollAccrualsQueryValidator : AbstractValidator<GetPayrollAccrualsQuery>
{
	private const int MaxTake = 100;
	private static readonly string[] AllowedSortFields =
	[
		"employee",
		"employeename",
		"totalamount",
		"paidamount",
		"remaining",
		"remainingamount"
	];

	public GetPayrollAccrualsQueryValidator()
	{
		RuleFor(x => x.From)
			.NotEqual(default(DateOnly));

		RuleFor(x => x.To)
			.NotEqual(default(DateOnly));

		RuleFor(x => x)
			.Must(x => x.To >= x.From)
			.WithMessage("To must be greater than or equal to From.");

		RuleFor(x => x.SortBy)
			.Must(sort =>
				sort is null ||
				AllowedSortFields.Contains(sort.Trim().ToLowerInvariant()))
			.WithMessage("SortBy must be one of: employee, employeename, totalamount, paidamount, remaining or remainingamount.");

		RuleFor(x => x.Skip)
			.GreaterThanOrEqualTo(0);

		RuleFor(x => x.Take)
			.InclusiveBetween(1, MaxTake);

		RuleFor(x => x.EmployeeId)
			.NotEmpty()
			.When(x => x.EmployeeId.HasValue);

		RuleFor(x => x.DepartmentId)
			.NotEmpty()
			.When(x => x.DepartmentId.HasValue);
	}
}
