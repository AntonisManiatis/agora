namespace Agora.Identity.Core;

// ! this should not be public.
public interface IUserRepository
{
    Task<bool> ExistsAsync(string email);

    Task<Guid> AddAsync(User user);
}