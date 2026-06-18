using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Infrastructure.Common;

namespace MyFactory.Infrastructure.Services;

public class LocalFileStorage : IFileStorage
{
    private readonly string _root;

    public LocalFileStorage(IOptions<Settings> options)
    {
        var settings = options.Value;
        _root = ResolveRoot(settings.FileStorageRoot);
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveAsync(string fileName, Stream content, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name is required", nameof(fileName));
        }

        var safeName = Path.GetFileName(fileName.Trim());
        if (string.IsNullOrWhiteSpace(safeName))
        {
            throw new ArgumentException("File name is invalid", nameof(fileName));
        }

        var uniqueName = $"{Guid.NewGuid():N}_{safeName}";
        var fullPath = BuildPathUnderRoot(uniqueName);

        await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fileStream, cancellationToken);

        return uniqueName;
    }

    public async Task<Stream?> GetAsync(string path, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        var fullPath = BuildPathUnderRoot(path);
        if (!File.Exists(fullPath))
        {
            return null;
        }

        var memoryStream = new MemoryStream();
        await using var sourceStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        await sourceStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return Task.CompletedTask;
        }

        var fullPath = BuildPathUnderRoot(path);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    private static string ResolveRoot(string? configuredRoot)
    {
        if (!string.IsNullOrWhiteSpace(configuredRoot))
        {
            return Path.IsPathRooted(configuredRoot)
                ? configuredRoot
                : Path.Combine(AppContext.BaseDirectory, configuredRoot);
        }

        return Path.Combine(AppContext.BaseDirectory, "storage");
    }

    private string BuildPathUnderRoot(string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_root, relativePath));
        var fullRoot = Path.GetFullPath(_root);

        if (!fullPath.StartsWith(fullRoot, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid file path.");
        }

        return fullPath;
    }
}
