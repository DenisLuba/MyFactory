using MyFactory.Application.DTOs.Departments;

namespace MyFactory.Application.DTOs.Employees;

public sealed class EmployeeDetailsDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = null!;

    public DepartmentDto Department { get; init; } = null!;
    public PositionDto Position { get; init; } = null!;

    public int? Grade { get; init; }

    public decimal? RatePerNormHour { get; init; }
    public decimal? PremiumPercent { get; init; }

    public DateOnly HiredAt { get; init; }
    public DateOnly? FiredAt { get; init; }
    public bool IsActive { get; init; }

    public IReadOnlyList<ContactDto> Contacts { get; init; } = [];
}