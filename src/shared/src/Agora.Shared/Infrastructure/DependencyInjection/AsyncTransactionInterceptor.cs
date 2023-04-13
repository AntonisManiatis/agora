using Agora.Shared.Infrastructure.Data;

using Castle.DynamicProxy;

using ErrorOr;

namespace Agora.Shared.Infrastructure.DependencyInjection;

sealed class AsyncTransactionInterceptor : AsyncInterceptorBase
{
    private readonly IDbConnector connector;

    public AsyncTransactionInterceptor(IDbConnector connector) =>
        this.connector = connector;

    protected override async Task InterceptAsync(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed
    )
    {
        // TODO: Only if method has [Transactional]
        using var connection = await connector.ConnectAsync();

        using (var transaction = connection.BeginTransaction())
        {
            await proceed(invocation, proceedInfo);

            transaction.Commit();

            // TODO: Also post events to broker here.
        }
    }

    protected override async Task<TResult> InterceptAsync<TResult>(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed
    )
    {
        // TODO: Only if method has [Transactional]
        using var connection = await connector.ConnectAsync();

        using (var transaction = connection.BeginTransaction())
        {
            var result = await proceed(invocation, proceedInfo);
            // TODO: test it, also not complete.
            if (typeof(TResult).IsAssignableFrom(typeof(IErrorOr)))
            {
                transaction.Rollback();
                return result;
            }

            transaction.Commit();
            // TODO: Also post events to broker here.
            return result;
        }
    }
}