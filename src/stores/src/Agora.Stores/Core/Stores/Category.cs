using Agora.Stores.Core.Products;

namespace Agora.Stores.Core.Stores;

sealed class Category
{
    private readonly List<ProductId> products = new();

    internal int Id { get; set; }
    internal string Name { get; set; } = string.Empty;
    internal IReadOnlyList<ProductId> Products => products;

    internal void AddProduct(ProductId productId)
    {
        products.Add(productId);
    }
}