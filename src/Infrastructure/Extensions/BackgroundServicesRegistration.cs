using Application.Abstractions;
using Infrastructure.BackgroundServices;
using Infrastructure.Services.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class BackgroundServicesRegistration
{
    public static IServiceCollection AddBackgroundServices(this IServiceCollection services, IConfiguration config)
    {
        
        services.AddSingleton<IEmailSender, SmtpEmailSender>();
        services.AddHostedService<NotificationOutboxWorker>();
        return services;
    }
} 