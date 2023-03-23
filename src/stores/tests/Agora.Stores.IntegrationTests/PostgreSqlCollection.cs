namespace Agora.Stores.IntegrationTests;

[CollectionDefinition(nameof(PostgreSqlFixture))]
public class DatabaseCollection : ICollectionFixture<PostgreSqlFixture> { }