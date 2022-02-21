using FrwkQuickWait.Domain.Constants;
using FrwkQuickWait.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FrwkQuickWait.Infrastructure.IOC
{
    public static class Configurations
    {         
        public static void AddHealthCheckConfiguration(this IServiceCollection services, IConfiguration configuration)
            => services.AddHealthChecks()
                       .AddSqlServer(connectionString: configuration.GetConnectionString("ConnectionString"), name: "Instancia do sql server");

        public static IServiceCollection AddAuthenticateConfuguration(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddAuthorizeConfiguration(this IServiceCollection services)
             => services.AddAuthorization(options =>
                {
                    options.AddPolicy("Admin", policy => policy.RequireRole("manager"));
                    options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
                });
        
    }
}
