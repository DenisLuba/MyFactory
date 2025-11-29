using MyFactory.MauiClient.Models.Users;

namespace MyFactory.MauiClient.Services.UsersServices
{
    public interface IUsersService
    {
        Task<List<UsersGetByRoleResponse>?> GetByRoleAsync(string? role = null);
        Task<UsersGetByIdResponse?> GetByIdAsync(Guid id);
        Task<UsersCreateResponse?> CreateAsync(UsersCreateRequest request);
        Task<UsersUpdateResponse?> UpdateAsync(Guid id, UsersUpdateRequest request);
        Task<UsersDeleteResponse?> DeleteAsync(Guid id);
    }
}