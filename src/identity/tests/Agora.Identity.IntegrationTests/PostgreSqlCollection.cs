namespace Agora.Identity.IntegrationTests;

[CollectionDefinition(nameof(PostgreSqlFixture))]
public class DatabaseCollection : ICollectionFixture<PostgreSqlFixture> { }