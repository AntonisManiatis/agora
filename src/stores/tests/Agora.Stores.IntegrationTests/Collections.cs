using Agora.Shared.IntegrationTests;

namespace Agora.Stores.IntegrationTests;

[CollectionDefinition("Stores")]
public class Collections :
    ICollectionFixture<PostgreSqlFixture>,
    IClassFixture<ServiceFixture>
{ }