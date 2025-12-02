using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Services.WarehousesServices;

public interface IWarehousesService
{
    Task<List<WarehousesListResponse>?> ListAsync();
    Task<WarehousesGetResponse?> GetAsync(Guid id);
    Task<WarehousesCreateResponse?> CreateAsync(WarehousesCreateRequest request);
    Task<WarehousesUpdateResponse?> UpdateAsync(Guid id, WarehousesUpdateRequest request);
    Task DeleteAsync(Guid id);
}