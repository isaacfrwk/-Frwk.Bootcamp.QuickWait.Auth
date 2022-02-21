using FrwkQuickWait.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrwkQuickWait.Infrastructure.IOC
{
    public static class DataBaseConfigurations
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection service, IConfiguration configuration) => 
            service.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        public static void UseDatabaseConfiguration(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
            context.Database.EnsureCreated();
        }

    }
}
