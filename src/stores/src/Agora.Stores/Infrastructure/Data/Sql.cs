namespace Agora.Stores.Infrastructure.Data;

internal static class Sql
{
    internal const string Table = "Store_request";
    internal const string InsertStoreApplication = $@"INSERT INTO {Table} (make, model, year, color)
VALUES ('Toyota', 'Camry', 2022, 'Red')
RETURNING id;";
}