using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class RecordOverheadResponseExample : IExamplesProvider<RecordOverheadResponse>
{
    public RecordOverheadResponse GetExamples() =>
        new(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Status: OverheadStatus.Draft
        );
}

