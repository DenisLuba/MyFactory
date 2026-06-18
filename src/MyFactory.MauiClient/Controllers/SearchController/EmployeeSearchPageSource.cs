using MyFactory.MauiClient.Models.Common;
using MyFactory.MauiClient.Services.Common;
using MyFactory.MauiClient.Services.Employees;

namespace MyFactory.MauiClient.Controllers;

public sealed class EmployeeSearchPageSource : ISearchPageSouce
{
    private readonly IEmployeesService _employeesService;
    private readonly Guid? _departmentId;
    private readonly bool? _canCut;
    private readonly bool? _canSew;
    private readonly bool? _canPackage;
    private readonly ICollection<Guid>? _exceptEmployeeIds;

    public EmployeeSearchPageSource(
        IEmployeesService employeesService,
        Guid? departmentId = null,
        bool? canCut = null,
        bool? canSew = null,
        bool? canPackage = null,
        ICollection<Guid>? exceptEmployeeIds = null)
    {
        _employeesService = employeesService;
        _departmentId = departmentId;
        _canCut = canCut;
        _canSew = canSew;
        _canPackage = canPackage;
        _exceptEmployeeIds = exceptEmployeeIds;
    }

    public async Task<ListResponse<SearchItemDto>?> GetPageAsync(
        int skip,
        int take,
        string? searchName,
        string? searchType,
        CancellationToken ct = default)
    {
        var response = await _employeesService.GetListByDateAsync(
            searchName: searchName,
            sortBy: null,
            sortDesc: false,
            skip: skip,
            take: take,
            isActive: true,
            departmentId: _departmentId,
            positionId: null,
            grade: null,
            hiredFrom: null,
            hiredTo: null,
            canCut: _canCut,
            canSew: _canSew,
            canPackage: _canPackage,
            exceptEmployeeIds: _exceptEmployeeIds);

        if (response is null)
            return null;

        var items = response.Items
            .Where(i => i.Id != Guid.Empty)
            .Select(i => new SearchItemDto(i.Id.ToString(), i.FullName))
            .ToList();

        return new ListResponse<SearchItemDto>(
            Items: items,
            TotalCount: response.TotalCount,
            Take: response.Take,
            Skip: response.Skip,
            HasMore: response.HasMore);
    }
}
