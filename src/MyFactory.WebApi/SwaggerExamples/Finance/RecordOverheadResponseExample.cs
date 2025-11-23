using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class RecordOverheadResponseExample : IExamplesProvider<RecordOverheadResponse>
{
    public RecordOverheadResponse GetExamples() =>
        new(Status: FinanceStatus.OverheadRecorded);
}

