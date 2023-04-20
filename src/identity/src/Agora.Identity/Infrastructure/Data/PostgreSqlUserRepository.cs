using Agora.Identity.Core;
using Agora.Shared.Infrastructure.Data;

using Dapper;

namespace Agora.Identity.Infrastructure.Data;

internal sealed class PostgreSqlUserRepository : IUserRepository
{
    private readonly IDbConnector connector;

    public PostgreSqlUserRepository(IDbConnector connector) => this.connector = connector;

    public async Task<bool> ExistsAsync(string email)
    {
        var connection = await connector.ConnectAsync();

        return await connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(*) FROM identity.user WHERE email=@Email",
            new { Email = email } // ! Can I avoid the extra allocation here?
        );
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var connection = await connector.ConnectAsync();

        return await connection.QueryFirstOrDefaultAsync<User>(
            "SELECT * FROM identity.user WHERE email=@Email",
            new { Email = email } // ! again allocation.
        );
    }

    public async Task<Guid> AddAsync(User user)
    {
        var connection = await connector.ConnectAsync();

        return await connection.ExecuteScalarAsync<Guid>(
            "INSERT INTO identity.user (first_name, last_name, email, password) VALUES (@FirstName, @LastName, @Email, @Password) RETURNING id",
            new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Password
            }
        );
    }
}