using AutoFuzzer.BussinessLogic.Services;
using AutoFuzzer.WorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddTransient<ContainerService>();
    })
    .Build();

await host.RunAsync();
