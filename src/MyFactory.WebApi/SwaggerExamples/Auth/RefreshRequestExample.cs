using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Auth;

public class RefreshRequestExample : IExamplesProvider<RefreshRequest>
{
    public RefreshRequest GetExamples() =>
        new(
            RefreshToken: "rft_abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"
        );
}

