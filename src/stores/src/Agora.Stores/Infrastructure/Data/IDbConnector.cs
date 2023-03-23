using System.Data;

namespace Agora.Stores.Infrastructure;

public interface IDbConnector
{
    Task<IDbConnection> ConnectAsync(); // ? Maybe ct?
}