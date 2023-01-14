using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Ajay.Legend.App.Services
{
    public static class ServicesConfiguration
    {
        public static void AddServiceTier(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<ISheepService, SheepService>();
        }
    }
}