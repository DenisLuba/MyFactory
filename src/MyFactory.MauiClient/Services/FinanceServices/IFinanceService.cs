using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.UIModels.Finance;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFactory.MauiClient.Services.FinanceServices
{
    public interface IFinanceService
    {
        Task<RecordOverheadResponse?> AddOverheadAsync(RecordOverheadRequest request);
        Task<List<OverheadResponse>?> GetOverheadsAsync(int month, int year);
        Task<CreateAdvanceResponse?> CreateAdvanceAsync(CreateAdvanceRequest request);
        Task<SubmitAdvanceReportResponse?> SubmitAdvanceReportAsync(string advanceId, SubmitAdvanceReportRequest request);
        Task<List<AdvanceItem>?> GetAdvancesAsync();
        Task CloseAdvanceAsync(string advanceNumber);
        Task DeleteAdvanceAsync(string advanceNumber);
    }
}