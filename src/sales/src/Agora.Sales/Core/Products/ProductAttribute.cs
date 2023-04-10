namespace Agora.Sales.Core.Products;

sealed class ProductAttribute
{
    private ProductAttribute(
        string title,
        IReadOnlyList<ProductOption> options
    )
    {
        Title = title;
        Options = options;
    }

    internal static ProductAttribute Of(
        string title,
        IReadOnlyList<ProductOption> options
    )
    {
        return new ProductAttribute(title, options);
    }

    internal string Title { get; }
    internal IReadOnlyList<ProductOption> Options { get; }

    // TODO: Implement equality
}