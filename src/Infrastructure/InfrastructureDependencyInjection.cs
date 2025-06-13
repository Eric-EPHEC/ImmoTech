using Domain.Entities;
using Application.Common;
using Infrastructure.Common;
using Infrastructure.Persistences;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
           services.AddImmotechDbContext(configuration);

           services.AddHttpContextAccessor();
           services.AddScoped<Application.Common.ICurrentUser, Infrastructure.Common.CurrentUser>();
           services.AddScoped<Application.Common.IImmotechDbContext>(provider => provider.GetRequiredService<Infrastructure.Persistences.ImmotechDbContext>());

            return services;
        }
    }
} 