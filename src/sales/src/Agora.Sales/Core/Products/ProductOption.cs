namespace Agora.Sales.Core.Products;

sealed class ProductOption : IEquatable<ProductOption>
{
    private ProductOption(string title)
    {
        Title = title;
    }

    internal static ProductOption Of(string title)
    {
        return new ProductOption(title);
    }

    internal string Title { get; }

    public override bool Equals(object? obj) => Equals(obj as ProductOption);

    public override int GetHashCode() => HashCode.Combine(Title);

    public bool Equals(ProductOption? other) => other != null && Title == other.Title;
}