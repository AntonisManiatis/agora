using Agora.Shared;

namespace Agora.Catalog.Infrastructure.Data.Entities;

class ProductAttribute
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool PickOne { get; set; }
    public List<ProductOption> Options { get; set; } = new();

    internal static class Schema
    {
        internal const string Table = "category_attribute"; // ? name?

        internal static string Id = nameof(ProductAttribute.Id).ToSnakeCase();
        internal static string CategoryId = nameof(ProductAttribute.CategoryId).ToSnakeCase();
        internal static string Name = nameof(ProductAttribute.Name).ToSnakeCase();
        internal static string PickOne = nameof(ProductAttribute.PickOne).ToSnakeCase();
    }
}

class ProductOption
{
    public int Id { get; set; }
    public int AttributeId { get; set; }
    public string Name { get; set; } = string.Empty;

    internal static class Schema
    {
        internal const string Table = "category_attribute_option"; // ? name?

        internal static string Id = nameof(ProductOption.Id).ToSnakeCase();
        internal static string AttributeId = nameof(ProductOption.AttributeId).ToSnakeCase();
        internal static string Name = nameof(ProductOption.Name).ToSnakeCase();
    }
}