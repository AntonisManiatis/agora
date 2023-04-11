namespace Agora.Stores.Infrastructure.Data;

sealed class StoreEntity
{
    internal Guid StoreId { get; set; }
    internal Guid OwnerId { get; set; }
    internal string? Status { get; set; }
    internal string? Tin { get; set; }
}

sealed class ProductEntity
{
    internal int Id { get; set; }
    internal Guid ProductId { get; set; }
    internal Guid StoreId { get; set; }
    internal string? Sku { get; set; }
    internal int Quantity { get; set; }
}