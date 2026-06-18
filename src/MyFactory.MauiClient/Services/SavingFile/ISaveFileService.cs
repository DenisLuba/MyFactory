namespace MyFactory.MauiClient.Services.SavingFile;

public interface ISaveFileService
{
    Task SaveFileAsync(string title, string content, CancellationToken cancellationToken = default);
}
