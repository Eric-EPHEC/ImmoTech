using Application;
using Infrastructure;
using Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

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

var app = webApplicationBuilder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();