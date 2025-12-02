using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.WarehouseMaterials;

namespace MyFactory.MauiClient.Services.WarehouseMaterialsServices;

public interface IWarehouseMaterialsService
{
    Task<List<MaterialReceiptListResponse>?> ListReceiptsAsync();
    Task<MaterialReceiptCardResponse?> GetReceiptAsync(Guid id);
    Task<MaterialReceiptUpsertResponse?> CreateReceiptAsync(MaterialReceiptUpsertRequest request);
    Task<MaterialReceiptUpsertResponse?> UpdateReceiptAsync(Guid id, MaterialReceiptUpsertRequest request);
    Task<MaterialReceiptPostResponse?> PostReceiptAsync(Guid id);
    Task<List<MaterialReceiptLineResponse>?> GetLinesAsync(Guid id);
    Task<MaterialReceiptLineUpsertResponse?> AddLineAsync(Guid id, MaterialReceiptLineUpsertRequest request);
    Task<MaterialReceiptLineUpsertResponse?> UpdateLineAsync(Guid id, Guid lineId, MaterialReceiptLineUpsertRequest request);
    Task DeleteLineAsync(Guid id, Guid lineId);
}
