namespace Agora.Catalogs.Infrastructure.Data.Entities;

public record Product(
    Guid Id,
    string Title,
    string Description
);