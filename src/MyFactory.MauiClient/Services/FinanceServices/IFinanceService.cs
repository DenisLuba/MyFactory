using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyFactory.MauiClient.Models.Finance;
using MyFactory.MauiClient.UIModels.Finance;

namespace MyFactory.MauiClient.Services.FinanceServices
{
    public interface IFinanceService
    {
        Task<RecordOverheadResponse?> AddOverheadAsync(RecordOverheadRequest request);
        Task<RecordOverheadResponse?> UpdateOverheadAsync(Guid overheadId, RecordOverheadRequest request);
        Task<RecordOverheadResponse?> PostOverheadAsync(Guid overheadId);
        Task<RecordOverheadResponse?> DeleteOverheadAsync(Guid overheadId);
        Task<List<OverheadItem>?> GetOverheadsAsync(int month, int year, string? article = null, OverheadStatus? status = null);
        Task<List<string>?> GetOverheadArticlesAsync();
        Task<AdvanceStatusResponse?> CreateAdvanceAsync(CreateAdvanceRequest request);
        Task<AdvanceStatusResponse?> SubmitAdvanceReportAsync(string advanceId, SubmitAdvanceReportRequest request);
        Task<List<AdvanceItem>?> GetAdvancesAsync();
        Task<AdvanceStatusResponse?> CloseAdvanceAsync(string advanceNumber);
        Task<AdvanceStatusResponse?> DeleteAdvanceAsync(string advanceNumber);
    }
}