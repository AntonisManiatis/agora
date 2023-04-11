using System.Data;

namespace Agora.Shared.Infrastructure.Data;

public interface IDbConnector
{
    Task<IDbConnection> ConnectAsync(CancellationToken cancellationToken = default);
}