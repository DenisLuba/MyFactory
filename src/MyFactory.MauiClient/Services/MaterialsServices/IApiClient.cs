using MyFactory.MauiClient.Models;

namespace MyFactory.MauiClient.Services;

public interface IApiClient
{
    Task<IEnumerable<MaterialListItem>> GetMaterialsAsync();
}
