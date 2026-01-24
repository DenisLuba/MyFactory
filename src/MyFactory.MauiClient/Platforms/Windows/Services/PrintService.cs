#if WINDOWS
using System.Diagnostics;
using System.Text;

namespace MyFactory.MauiClient.Services.Printing;

public sealed class PrintService : IPrintService
{
    public Task PrintTextAsync(string title, string content, CancellationToken cancellationToken = default)
    {
        var fileName = Path.Combine(Path.GetTempPath(), $"{Sanitize(title)}.txt");
        File.WriteAllText(fileName, content, Encoding.UTF8);

        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Verb = "print",
            UseShellExecute = true,
            CreateNoWindow = true
        };

        Process.Start(psi);
        return Task.CompletedTask;
    }

    private static string Sanitize(string name) =>
        string.Join("_", name.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
}
#endif