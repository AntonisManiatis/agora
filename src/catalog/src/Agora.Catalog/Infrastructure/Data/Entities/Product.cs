using Agora.Shared;

namespace Agora.Catalog.Infrastructure.Data.Entities;

class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    internal static class Schema
    {
        internal const string Table = "product";

        internal static readonly string Id = nameof(Product.Id).ToSnakeCase();
        internal static readonly string Title = nameof(Product.Title).ToSnakeCase();
        internal static readonly string Description = nameof(Product.Description).ToSnakeCase();
    }
}