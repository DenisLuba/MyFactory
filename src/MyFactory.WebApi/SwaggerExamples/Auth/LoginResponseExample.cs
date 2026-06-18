using MyFactory.WebApi.Contracts.Auth;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Auth;

public class LoginResponseExample : IExamplesProvider<LoginResponse>
{
    public LoginResponse GetExamples() =>
        new(
            AccessToken: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkFkbWluIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
            RefreshToken: "rft_abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
            ExpiresIn: 3600
        );
}

