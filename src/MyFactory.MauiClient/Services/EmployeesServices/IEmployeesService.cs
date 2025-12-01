using MyFactory.MauiClient.Models.Employees;

namespace MyFactory.MauiClient.Services.EmployeesServices;

public interface IEmployeesService
{
    Task<IReadOnlyList<EmployeeListResponse>?> GetEmployeesAsync(string? role = null);
    Task<EmployeeCardResponse?> GetEmployeeAsync(Guid employeeId);
    Task UpdateEmployeeAsync(Guid employeeId, EmployeeUpdateRequest payload);
}
