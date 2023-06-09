namespace Agora.API.Stores;

public record StorePreferences(
    string? Language,   // ? Optional too? Depends on settings?
    string? Country,    // Optional
    string? Currency    // Optional
);

public record RegisterStoreRequest(
    StorePreferences Preferences,
    string Name
);

// TaxAddress TaxAddress,
// // ? Or Tax Identification Number 
// string Tin,
// // AKA GEMI in Greece.
// string? Brn,

public record TaxAddress
{
    public static readonly TaxAddress Undefined = new();

    public string Street { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string ZipCode { get; init; } = string.Empty;
}

public record Store(
    Guid Id,
    string Name
);