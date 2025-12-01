using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Operations;

namespace MyFactory.MauiClient.Services.OperationsServices;

public interface IOperationsService
{
    Task<IReadOnlyList<OperationListResponse>?> GetOperationsAsync();
    Task<OperationCardResponse?> GetOperationAsync(Guid id);
    Task<OperationUpdateResponse?> UpdateOperationAsync(Guid id, OperationUpdateRequest request);
}
