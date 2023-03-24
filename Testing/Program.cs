using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Testing;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IClaimsPrincipalAccessor, ClaimsPrincipalAccessor>();
    })
    .ConfigureFunctionsWorkerDefaults(worker =>
    {
        worker.UseMiddleware<ClaimsPrincipalMiddleware>();
    })
    .Build();

host.Run();
