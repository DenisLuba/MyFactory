using MyFactory.MauiClient.Models.Shifts;

namespace MyFactory.MauiClient.Services.ShiftsServices
{
    public interface IShiftsService
    {
        Task<ShiftsCreatePlanResponse?> CreatePlanAsync(ShiftsCreatePlanRequest request);
        Task<List<ShiftsGetPlansResponse>?> GetPlansAsync(DateTime? date = null);
        Task<ShiftsRecordResultResponse?> RecordResultAsync(ShiftsRecordResultRequest request);
        Task<List<ShiftsGetResultsResponse>?> GetResultsAsync(Guid? employeeId = null, DateTime? date = null);
    }
}