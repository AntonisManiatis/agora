namespace Agora.Stores.Core.Stores;

sealed class TaxAddress
{
    internal static readonly TaxAddress Undefined = new();

    internal string Street { get; init; } = string.Empty;
    internal string City { get; init; } = string.Empty;
    internal string State { get; init; } = string.Empty;
    internal string ZipCode { get; init; } = string.Empty;
}