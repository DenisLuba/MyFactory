using System.Threading;
using System.Threading.Tasks;

namespace MyFactory.Infrastructure.Persistence;

public class InitialDataSeeder
{
    public Task SeedAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}
