using System.Reflection;

using Agora.Catalogs.Services;
using Agora.Shared.IntegrationTests;

namespace Agora.Catalogs.IntegrationTests;

public sealed class CatalogPostgreSqlFixture : PostgreSqlFixture
{
    protected override Assembly[] Migrations => new[] { typeof(IProductService).Assembly };
}