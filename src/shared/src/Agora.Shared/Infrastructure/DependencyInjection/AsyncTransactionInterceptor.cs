using System.Reflection;

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
        // ! test if it works.
        var transactional = invocation.MethodInvocationTarget.GetCustomAttribute<TransactionalAttribute>();
        if (transactional is null)
        {
            await proceed(invocation, proceedInfo);
            return;
        }

        using var connection = await connector.ConnectAsync();

        // Related to async versions of transaction methods.
        // https://github.com/npgsql/npgsql/issues/836
        using (var transaction = connection.BeginTransaction()) // TODO: Use Isolation level of attribute.
        {
            await proceed(invocation, proceedInfo);

            // TODO: Also post events to broker here, use Outbox pattern or similar.
            transaction.Commit();
        }
    }

    protected override async Task<TResult> InterceptAsync<TResult>(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed
    )
    {
        var transactional = invocation.MethodInvocationTarget.GetCustomAttribute<TransactionalAttribute>();
        if (transactional is null)
        {
            return await proceed(invocation, proceedInfo);
        }

        using var connection = await connector.ConnectAsync();

        using (var transaction = connection.BeginTransaction()) // TODO: Use Isolation level of attribute.
        {
            var result = await proceed(invocation, proceedInfo);
            // TODO: test it, also not complete.
            if (typeof(TResult).IsAssignableFrom(typeof(IErrorOr)))
            {
                if (invocation.ReturnValue is IErrorOr errorOr && errorOr.IsError)
                {
                    transaction.Rollback();
                }

                return result;
            }

            // TODO: Also post events to broker here, use Outbox pattern or similar.
            transaction.Commit();
            return result;
        }
    }
}