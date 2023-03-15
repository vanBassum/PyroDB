using Microsoft.Extensions.DependencyInjection;
using System.Collections.Specialized;
using System.Net;

namespace Mica
{

    //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio
    public static class ServiceCollectionExtensions
    {
        public static void AddMicaScheduler(this IServiceCollection services, Action<MicaOptionsBuilder> options = null)
        {
            var builder = new MicaOptionsBuilder(services);
            options?.Invoke(builder);
            services.AddHostedService<MicaScheduler>();
        }
    }
}
