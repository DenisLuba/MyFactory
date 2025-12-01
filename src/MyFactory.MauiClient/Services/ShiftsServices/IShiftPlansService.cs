using System;
using System.Collections.Generic;
using MyFactory.MauiClient.Models.Shifts;

namespace MyFactory.MauiClient.Services.ShiftsServices
{
    public interface IShiftPlansService
    {
        Task<ShiftsCreatePlanResponse?> CreatePlanAsync(ShiftsCreatePlanRequest request);
        Task<IReadOnlyList<ShiftPlanListResponse>?> GetPlansAsync(DateTime? date = null);
        Task<ShiftPlanCardResponse?> GetPlanByIdAsync(Guid shiftPlanId);
        Task<ShiftsRecordResultResponse?> RecordResultAsync(ShiftsRecordResultRequest request);
    }
}
