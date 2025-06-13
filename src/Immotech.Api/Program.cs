using Application;
using Infrastructure;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using Application.Common;
using Immotech.Api.Common;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services.AddControllers();
webApplicationBuilder.Services.AddEndpointsApiExplorer();
webApplicationBuilder.Services.AddSwaggerGen();

// Add Infrastructure services
webApplicationBuilder.Services.AddInfrastructure(webApplicationBuilder.Configuration);

webApplicationBuilder.Services.AddIdentityApiEndpoints<Domain.Entities.User>(
            )
            .AddEntityFrameworkStores<ImmotechDbContext>();

// Add Application services
webApplicationBuilder.Services.AddApplication();

webApplicationBuilder.Services.AddHttpContextAccessor();
webApplicationBuilder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = webApplicationBuilder.Build();

if (app.Environment.IsDevelopment())
{
    /// Apply migrations that have not been applied to the database by checking the migrations history (a table in the database created by EF Core)
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ImmotechDbContext>();
    dbContext.Database.Migrate();
    

    app.UseSwagger();
    app.UseSwaggerUI();
  
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<Domain.Entities.User>(); // Map the Identity API endpoints to the application. This adds built-in endpoints for user management.
app.MapControllers();
app.Run();