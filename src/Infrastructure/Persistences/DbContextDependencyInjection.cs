using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistences
{
    public static class DbContextDependencyInjection
    {
        public static void AddImmotechDbContext(this IServiceCollection services, IConfiguration configuration) 
        {
            var ct = configuration.GetConnectionString("ImmoTech");
            ArgumentNullException.ThrowIfNull(ct,nameof(ct));
            services.AddDbContext<ImmotechDbContext>(o => 
            {
                // ConnectionStrings__ImmoTech
                o.UseSqlServer(ct);
            });
        }
    }
}
