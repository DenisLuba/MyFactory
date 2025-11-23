using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class SubmitAdvanceReportResponseExample : IExamplesProvider<SubmitAdvanceReportResponse>
{
    public SubmitAdvanceReportResponse GetExamples() =>
        new(
            AdvanceId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Status: FinanceStatus.ReportSubmitted
        );
}

