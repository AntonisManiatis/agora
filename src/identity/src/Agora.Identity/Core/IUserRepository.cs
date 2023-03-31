namespace Agora.Identity.Core;

internal interface IUserRepository
{
    Task<bool> ExistsAsync(string email);

    Task<User?> GetUserByEmail(string email);

    Task<Guid> AddAsync(User user);
}