namespace Agora.Stores.Core.Stores;

interface IStoreRepository
{
    Task<Store?> GetStoreAsync(Guid storeId);

    Task<bool> ExistsAsync(string storeName);

    Task<IEnumerable<Store>> GetStoresAsync();

    Task<Guid> AddAsync(Store store);
}