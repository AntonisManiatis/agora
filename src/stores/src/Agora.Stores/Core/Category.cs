using Agora.Stores.Core.Products;

namespace Agora.Stores.Core;

sealed class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IList<ProductId> Products { get; set; } = new List<ProductId>();
}