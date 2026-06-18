namespace MyFactory.Application.Common.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(string fileName, Stream content, CancellationToken cancellationToken = default);

    Task<Stream?> GetAsync(string path, CancellationToken cancellationToken = default);

    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
}
