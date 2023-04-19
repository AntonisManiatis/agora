namespace Agora.Catalog.Infrastructure.Data.Entities;

class Store
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Lang { get; set; } = string.Empty;

    internal static class Schema
    {
        internal const string Table = "store";

        internal static readonly string Id = nameof(Store.Id).ToSnakeCase();
        internal static readonly string Name = nameof(Store.Name).ToSnakeCase();
        internal static readonly string Lang = nameof(Store.Lang).ToSnakeCase();
    }
}