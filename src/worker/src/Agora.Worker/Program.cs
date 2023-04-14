using System.Reflection;

using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(options =>
        {
            options.AddDelayedMessageScheduler();

            options.SetKebabCaseEndpointNameFormatter();

            // By default, sagas are in-memory, but should be changed to a durable
            // saga repository.
            options.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();

            options.AddConsumers(entryAssembly);
            options.AddSagaStateMachines(entryAssembly);
            options.AddSagas(entryAssembly);
            options.AddActivities(entryAssembly);

            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost");

                cfg.UseDelayedMessageScheduler();

                cfg.ConfigureEndpoints(context);
            });

        });
    })
    .Build();

host.Run();