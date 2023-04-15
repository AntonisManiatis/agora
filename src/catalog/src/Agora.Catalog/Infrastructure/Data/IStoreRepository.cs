using Agora.Catalog.Infrastructure.Data.Entities;

namespace Agora.Catalog.Infrastructure.Data;

interface IStoreRepository
{
    Task<bool> ExistsAsync(string storeName);

    Task SaveAsync(Store store);
}