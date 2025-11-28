namespace MyFactory.MauiClient.Models.Files;

/// <summary>
/// MAUI-клиент не может использовать IFormFile, поэтому храним содержимое файла вручную.
/// </summary>
public record UploadFileRequest(
    byte[] FileBytes,
    string FileName,
    string ContentType);
