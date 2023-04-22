using Microsoft.Extensions.DependencyInjection;

namespace Agora.Shared;

public sealed class Scoped<T> : IDisposable where T : notnull
{
    private readonly IServiceScope scope;

    public Scoped(IServiceScope scope) => this.scope = scope;

    public T Service => scope.ServiceProvider.GetRequiredService<T>();

    public void Dispose() => scope.Dispose();
}