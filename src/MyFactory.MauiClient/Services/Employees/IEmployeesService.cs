using MyFactory.MauiClient.Models.Employees;

namespace MyFactory.MauiClient.Services.Employees;

public interface IEmployeesService
{
    Task<IReadOnlyList<EmployeeListItemResponse>?> GetListAsync(string? search = null, bool includeInactive = false, EmployeeSortBy sortBy = EmployeeSortBy.FullName, bool sortDesc = false);
    Task<EmployeeDetailsResponse?> GetDetailsAsync(Guid id);
    Task<CreateEmployeeResponse?> CreateAsync(CreateEmployeeRequest request);
    Task UpdateAsync(Guid id, UpdateEmployeeRequest request);
    Task ActivateAsync(Guid id, ActivateEmployeeRequest request);
    Task DeactivateAsync(Guid id, DeactivateEmployeeRequest request);
    Task<IReadOnlyList<EmployeeProductionAssignmentResponse>?> GetAssignmentsAsync(Guid id);
    Task<IReadOnlyList<TimesheetListItemResponse>?> GetTimesheetsAsync(Guid? employeeId, Guid? departmentId, int year, int month);
    Task<IReadOnlyList<EmployeeTimesheetEntryResponse>?> GetEmployeeTimesheetAsync(Guid id, int year, int month);
    Task<AddTimesheetEntryResponse?> AddTimesheetEntryAsync(Guid id, AddTimesheetEntryRequest request);
    Task UpdateTimesheetEntryAsync(Guid entryId, UpdateTimesheetEntryRequest request);
    Task RemoveTimesheetEntryAsync(Guid entryId);
}