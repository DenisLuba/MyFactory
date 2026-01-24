
using CommunityToolkit.Maui.Storage;
using System.Text;

namespace MyFactory.MauiClient.Services.SavingFile;

public sealed class SaveFileService : ISaveFileService
{
    private readonly IFileSaver _fileSaver;

    public SaveFileService(IFileSaver fileSaver)
    {
        _fileSaver = fileSaver;
    }

    public SaveFileService() : this(FileSaver.Default) { }

    public async Task SaveFileAsync(string title, string content, CancellationToken cancellationToken = default)
    {
        var fileName = $"{Sanitize(title)}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        await using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var result = await _fileSaver.SaveAsync(fileName, stream, cancellationToken);

        if (!result.IsSuccessful)
        {
            throw new InvalidOperationException("Не удалось сохранить файл.");
        }
    }

    private static string Sanitize(string name) =>
        string.Join("_", name.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
}
