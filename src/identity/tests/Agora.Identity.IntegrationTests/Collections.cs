using Agora.Shared.IntegrationTests;

namespace Agora.Identity.IntegrationTests;

[CollectionDefinition("Identity")]
public class Collections :
    ICollectionFixture<PostgreSqlFixture>,
    IClassFixture<ServiceFixture>
{ }