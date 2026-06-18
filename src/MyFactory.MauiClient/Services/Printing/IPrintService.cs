namespace MyFactory.MauiClient.Services.Printing;

public interface IPrintService
{
    Task PrintTextAsync(string title, string content, CancellationToken cancellationToken = default);
}
