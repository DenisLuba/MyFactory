using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.WorkshopExpenses;

namespace MyFactory.MauiClient.Services.WorkshopExpensesServices;

public interface IWorkshopExpensesService
{
    Task<IReadOnlyList<WorkshopExpenseListResponse>?> ListAsync(Guid? workshopId = null);
    Task<WorkshopExpenseGetResponse?> GetAsync(Guid id);
    Task<WorkshopExpenseCreateResponse?> CreateAsync(WorkshopExpenseCreateRequest request);
    Task<WorkshopExpenseUpdateResponse?> UpdateAsync(Guid id, WorkshopExpenseUpdateRequest request);
}
