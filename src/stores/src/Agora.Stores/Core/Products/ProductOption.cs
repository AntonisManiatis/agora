namespace Agora.Stores.Core.Products;

sealed class ProductOption
{
    internal ProductOption(int id)
    {
        Id = id;
    }

    internal int Id { get; }
    internal string Title { get; set; } = string.Empty;
}