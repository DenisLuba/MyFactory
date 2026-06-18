using MyFactory.MauiClient.Models.Departments;

namespace MyFactory.MauiClient.Services.Departments;

public interface IDepartmentsService
{
    Task<IReadOnlyList<DepartmentListItemResponse>?> GetListAsync(bool includeInactive = false);
    Task<DepartmentDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateDepartmentResponse?> CreateAsync(CreateDepartmentRequest request);
    Task UpdateAsync(Guid id, UpdateDepartmentRequest request);
    Task ActivateAsync(Guid id);
    Task DeactivateAsync(Guid id);
}
