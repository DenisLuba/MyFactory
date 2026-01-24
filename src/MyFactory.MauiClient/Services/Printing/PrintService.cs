#if !WINDOWS
namespace MyFactory.MauiClient.Services.Printing;

public sealed class PrintService : IPrintService
{
    public Task PrintTextAsync(string title, string content, CancellationToken cancellationToken = default)
    {
        throw new PlatformNotSupportedException("Печать доступна только в Windows-сборке.");
    }
}
#endif