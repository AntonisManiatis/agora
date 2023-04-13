using Agora.Shared.IntegrationTests;

namespace Agora.Catalogs.IntegrationTests;

[CollectionDefinition("Catalog")]
public class Collections :
    ICollectionFixture<PostgreSqlFixture>,
    IClassFixture<ServiceFixture>
{ }