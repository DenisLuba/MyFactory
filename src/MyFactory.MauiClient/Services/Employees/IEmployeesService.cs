using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Models.Employees;
using MyFactory.MauiClient.Services.Common;

namespace MyFactory.MauiClient.Services.Employees;

public interface IEmployeesService : IGetListService<EmployeeListItemResponse>
{
    Task<ListResponse<EmployeeListItemResponse>?> GetListByDateAsync(
        string? searchName,
        string? sortBy,
        bool sortDesc = false,
        int skip = 0,
        int take = 30,
        bool? isActive = null,
        Guid? departmentId = null,
        Guid? positionId = null,
        int? grade = null,
        DateTime? hiredFrom = null,
        DateTime? hiredTo = null,
        bool? canCut = null,
        bool? canSew = null,
        bool? canPackage = null,
        ICollection<Guid>? exceptEmployeeIds = null);
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