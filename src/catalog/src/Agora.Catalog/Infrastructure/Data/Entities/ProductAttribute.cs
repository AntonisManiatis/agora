namespace Agora.Catalog.Infrastructure.Data.Entities;

class ProductAttribute
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool PickOne { get; set; }
    public List<ProductOption>? Options { get; set; }
}

class ProductOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}