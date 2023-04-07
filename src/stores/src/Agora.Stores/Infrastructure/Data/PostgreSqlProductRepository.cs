using Agora.Stores.Core.Products;

namespace Agora.Stores.Infrastructure.Data;

sealed class PostgreSqlProductRepository : IProductRepository
{
    public Task AddAsync(Product product)
    {
        throw new NotImplementedException();
    }
}