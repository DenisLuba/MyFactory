using MyFactory.WebApi.Contracts.Employees;

namespace MyFactory.WebApi.Services.Employees;

public interface IEmployeeRepository
{
    IEnumerable<EmployeeListResponse> GetAll(string? role);
    EmployeeCardResponse? GetById(Guid id);
    bool Update(Guid id, EmployeeUpdateRequest request);
}
