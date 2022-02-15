using FrwkQuickWait.Data.Repositories;
using FrwkQuickWait.Domain.Interfaces.Repositories;
using FrwkQuickWait.Domain.Interfaces.Services;
using FrwkQuickWait.Service.Consumers;
using FrwkQuickWait.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FrwkQuickWait.Data
{
    public static class Bootstrapper
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
             => services
                .AddScoped<IUserRepository, UserRepository>();

        public static IServiceCollection AddServices(this IServiceCollection services)
             => services
                .AddScoped<IUserService, UserService>()
                .AddScoped<ITokenService, TokenService>();

        public static IServiceCollection AddHosted(this IServiceCollection services)
             => services
                .AddHostedService<UserConsumer>()
                .AddHostedService<AuthConsumer>();
    }
}
