using MyFactory.MauiClient.Models.Finance;

namespace MyFactory.MauiClient.Services.FinanceServices
{
    public interface IFinanceService
    {
        Task<RecordOverheadResponse?> AddOverheadAsync(RecordOverheadRequest request);
        Task<List<OverheadResponse>?> GetOverheadsAsync(int month, int year);
        Task<CreateAdvanceResponse?> CreateAdvanceAsync(CreateAdvanceRequest request);
        Task<SubmitAdvanceReportResponse?> SubmitAdvanceReportAsync(string advanceId, SubmitAdvanceReportRequest request);
    }
}