using System.Threading;
using System.Threading.Tasks;

namespace MyFactory.Application.Interfaces.Auth;

public interface IPasswordHasher
{
    Task<string> HashAsync(string password, CancellationToken cancellationToken = default);

    Task<bool> VerifyAsync(string password, string passwordHash, CancellationToken cancellationToken = default);
}
