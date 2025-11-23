using MyFactory.WebApi.Contracts.Finance;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Finance;

public class CreateAdvanceResponseExample : IExamplesProvider<CreateAdvanceResponse>
{
    public CreateAdvanceResponse GetExamples() =>
        new(AdvanceId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
}

