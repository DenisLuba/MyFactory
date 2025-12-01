using System;
using System.Collections.Generic;
using System.Linq;
using MyFactory.WebApi.Contracts.Employees;

namespace MyFactory.WebApi.Services.Employees;

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private readonly List<EmployeeCardResponse> _employees = new()
    {
        new(
            Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0001"),
            "Иванова О.Г.",
            "Швея",
            4,
            true,
            "EMP-01",
            new DateOnly(2021, 10, 1)
        ),
        new(
            Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0002"),
            "Сергейчук А.А.",
            "Швея",
            3,
            true,
            "EMP-02",
            new DateOnly(2022, 2, 14)
        ),
        new(
            Guid.Parse("aaaaaaaa-aa01-4a0a-b001-aaaaaaaa0003"),
            "Малахов В.И.",
            "Раскройщик",
            5,
            true,
            "EMP-03",
            new DateOnly(2020, 8, 25)
        )
    };

    public IEnumerable<EmployeeListResponse> GetAll(string? role)
    {
        var source = string.IsNullOrWhiteSpace(role)
            ? _employees
            : _employees.Where(e => e.Position.Equals(role, StringComparison.OrdinalIgnoreCase)).ToList();

        return source.Select(e => new EmployeeListResponse(e.Id, e.FullName, e.Position, e.Grade, e.IsActive));
    }

    public EmployeeCardResponse? GetById(Guid id)
        => _employees.FirstOrDefault(e => e.Id == id);

    public bool Update(Guid id, EmployeeUpdateRequest request)
    {
        var existing = _employees.FirstOrDefault(e => e.Id == id);
        if (existing is null)
        {
            return false;
        }

        var index = _employees.IndexOf(existing);
        _employees[index] = existing with
        {
            FullName = request.FullName,
            Position = request.Position,
            Grade = request.Grade,
            IsActive = request.IsActive
        };

        return true;
    }
}
