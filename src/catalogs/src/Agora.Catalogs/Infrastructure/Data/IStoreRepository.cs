using Agora.Catalogs.Infrastructure.Data.Entities;

namespace Agora.Catalogs.Infrastructure.Data;

interface IStoreRepository
{
    Task<bool> ExistsAsync(string storeName);

    Task SaveAsync(Store store);
}