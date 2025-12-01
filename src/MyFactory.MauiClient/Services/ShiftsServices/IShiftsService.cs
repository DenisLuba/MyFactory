using System;
using System.Collections.Generic;
using MyFactory.MauiClient.Models.Shifts;

namespace MyFactory.MauiClient.Services.ShiftsServices
{
    public interface IShiftsService
    {
        Task<IReadOnlyList<ShiftResultListResponse>?> GetResultsAsync(Guid? employeeId = null, DateTime? date = null);
        Task<ShiftResultCardResponse?> GetResultByIdAsync(Guid shiftPlanId);
        Task<ShiftsRecordResultResponse?> SaveResultAsync(ShiftsRecordResultRequest request);
    }
}