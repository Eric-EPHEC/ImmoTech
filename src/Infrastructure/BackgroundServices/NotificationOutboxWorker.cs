using Application.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.BackgroundServices;

public class NotificationOutboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationOutboxWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

    public NotificationOutboxWorker(IServiceScopeFactory scopeFactory, ILogger<NotificationOutboxWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("NotificationOutboxWorker started");
        var timer = new PeriodicTimer(_interval);
        while (await timer.WaitForNextTickAsync(cancellationToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<IImmotechDbContext>();
                var email = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                // get the notifications that are not sent yet

                var notifications = await ctx.Notifications.AsNoTracking().IgnoreQueryFilters()
                                            .Where(n => !n.SentAt.HasValue)
                                            .OrderBy(n => n.Id).Select(n => new { n.Id, n.RecipientEmail, n.Message })  
                                            .Take(50)
                                            .ToListAsync(cancellationToken);

                // send in parallel
                await Parallel.ForEachAsync(notifications, cancellationToken, async (n, ct) =>
                {
                    var to = n.RecipientEmail;
                    if (string.IsNullOrWhiteSpace(to)) return;
                    await email.SendEmailAsync(to, "Notification", n.Message, ct);
                });

                if (notifications.Count > 0)
                {
                    var ids = notifications.Select(n => n.Id).ToList();
                    await ctx.Notifications.IgnoreQueryFilters()
                             .Where(n => ids.Contains(n.Id))
                             .ExecuteUpdateAsync(s => s.SetProperty(n => n.SentAt, _ => DateTimeOffset.UtcNow), cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NotificationOutboxWorker failure");
            }
        }
    }
} 