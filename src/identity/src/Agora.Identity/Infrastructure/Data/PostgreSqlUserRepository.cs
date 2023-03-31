using Agora.Identity.Core;
using Agora.Shared.Infrastructure.Data;

using Dapper;

namespace Agora.Identity.Infrastructure.Data;

internal sealed class PostgreSqlUserRepository : IUserRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlUserRepository(IDbConnector connector)
    {
        this.connector = connector;
    }

    public async Task<bool> ExistsAsync(string email)
    {
        using var connection = await connector.ConnectAsync();

        var exists = await connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(*) FROM identity.user WHERE email=@Email",
            new { Email = email } // ! Can I avoid the extra allocation here?
        );

        return exists;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        using var connection = await connector.ConnectAsync();

        var user = await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM identity.user WHERE email=@Email",
            new { Email = email } // ! again allocation.
        );

        return user;
    }

    public async Task<Guid> AddAsync(User user)
    {
        using var connection = await connector.ConnectAsync();

        var userId = await connection.ExecuteScalarAsync<Guid>(
            $"INSERT INTO identity.user (first_name, last_name, email, password) VALUES (@FirstName, @LastName, @Email, @Password) RETURNING id",
            new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password
            }
        );

        return userId;
    }
}