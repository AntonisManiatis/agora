using Agora.Shared.IntegrationTests;

namespace Agora.Catalog.IntegrationTests;

[CollectionDefinition("Catalog")]
public class Collections :
    ICollectionFixture<PostgreSqlFixture>,
    IClassFixture<ServiceFixture>
{ }