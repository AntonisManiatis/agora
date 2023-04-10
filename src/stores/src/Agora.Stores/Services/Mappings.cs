using Mapster;

namespace Agora.Stores.Services;

static class Mappings
{
    static Mappings()
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.NewConfig<Core.Products.Product, Product>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.StoreId, src => src.StoreId.Value)
            .Map(dest => dest.Sku, src => src.Sku)
            .Map(dest => dest.Quantity, src => src.Quantity.Value);
    }

    // Maybe a dumb idea but to make the CLR load the class so that the static constructor gets called.
    internal static void Init() { }
}