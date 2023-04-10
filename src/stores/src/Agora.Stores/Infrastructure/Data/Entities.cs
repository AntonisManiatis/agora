namespace Agora.Stores.Infrastructure.Data;

sealed class ProductEntity
{
    internal int Id { get; set; }
    internal Guid ProductId { get; set; }
    internal Guid StoreId { get; set; }
    internal string? Sku { get; set; }
    internal int Quantity { get; set; }
}