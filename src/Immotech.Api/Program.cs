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
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ImmotechDbContext>();
    dbContext.Database.Migrate();
    

    app.UseSwagger();
    app.UseSwaggerUI();
  
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();