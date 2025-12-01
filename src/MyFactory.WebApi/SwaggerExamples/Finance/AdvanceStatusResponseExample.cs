using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class AdvanceStatusResponseExample : IExamplesProvider<AdvanceStatusResponse>
{
    public AdvanceStatusResponse GetExamples() =>
        new(
            id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Status: AdvanceStatus.ReportSubmitted
        );
}

