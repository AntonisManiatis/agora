namespace Agora.Stores.Core.Stores;

interface IStoreRepository
{
    Task<Store?> GetStoreAsync(Guid storeId); // TODO: Use value object

    // Task<bool> ExistsAsync(string storeName);

    Task<IEnumerable<Store>> GetStoresAsync();

    Task AddAsync(Store store);
}