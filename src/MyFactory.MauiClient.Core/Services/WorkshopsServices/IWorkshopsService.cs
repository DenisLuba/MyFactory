using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Workshops;

namespace MyFactory.MauiClient.Services.WorkshopsServices;

public interface IWorkshopsService
{
    Task<IReadOnlyList<WorkshopsListResponse>?> ListAsync();
    Task<WorkshopGetResponse?> GetAsync(Guid id);
    Task<WorkshopCreateResponse?> CreateAsync(WorkshopCreateRequest request);
    Task<WorkshopUpdateResponse?> UpdateAsync(Guid id, WorkshopUpdateRequest request);
}
