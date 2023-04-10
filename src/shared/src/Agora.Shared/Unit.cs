namespace Agora.Shared;

// See: https://chtenb.dev/?page=unit-cs

/// <summary>
/// Because void cannot be returned in C# and there are cases where ErrorOr doesn't return an "or".
/// </summary>
public struct Unit : IEquatable<Unit>
{
    public override bool Equals(object? obj) => obj is Unit;
    public override int GetHashCode() => 0;
    public bool Equals(Unit other) => true;
    public override string ToString() => "()";

    public static bool operator ==(Unit left, Unit right) => left.Equals(right);
    public static bool operator !=(Unit left, Unit right) => !(left == right);
}