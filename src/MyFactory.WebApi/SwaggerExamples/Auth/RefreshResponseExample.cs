using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Auth;

public class RefreshResponseExample : IExamplesProvider<RefreshResponse>
{
    public RefreshResponse GetExamples() =>
        new(
            AccessToken: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkFkbWluIiwiaWF0IjoxNTE2MjM5MDIyLCJleHAiOjE1MTYyNDI2MjJ9.new_access_token_signature_here",
            ExpiresIn: 3600
        );
}

