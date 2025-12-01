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
        Task<AdvanceStatusResponse?> CreateAdvanceAsync(CreateAdvanceRequest request);
        Task<AdvanceStatusResponse?> SubmitAdvanceReportAsync(string advanceId, SubmitAdvanceReportRequest request);
        Task<List<AdvanceItem>?> GetAdvancesAsync();
        Task<AdvanceStatusResponse?> CloseAdvanceAsync(string advanceNumber);
        Task<AdvanceStatusResponse?> DeleteAdvanceAsync(string advanceNumber);
    }
}