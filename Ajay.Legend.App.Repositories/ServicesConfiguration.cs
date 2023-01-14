using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Ajay.Legend.App.Repositories.Entities;

namespace Ajay.Legend.App.Repositories
{
    public static class ServicesConfiguration
    {
        public static void AddRepositoryTier(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("AzureSql");
            services.AddDbContext<CdpDbContext>(options => options.UseSqlServer(sqlConnectionString));
            services.AddScoped<DbContext>(provider => provider.GetRequiredService<CdpDbContext>());
            services.AddTransient<IGenericRepository<Sheep>, GenericRepository<Sheep>>();
        }
    }
}