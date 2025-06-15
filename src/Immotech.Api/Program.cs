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

// bind Jwt settings
webApplicationBuilder.Services.Configure<Immotech.Api.Common.JwtSettings>(webApplicationBuilder.Configuration.GetSection("Jwt"));
webApplicationBuilder.Services.AddScoped<Immotech.Api.Common.IJwtTokenGenerator, Immotech.Api.Common.JwtTokenGenerator>();

webApplicationBuilder.Services.AddIdentityApiEndpoints<Domain.Entities.User>(
            )
            .AddEntityFrameworkStores<ImmotechDbContext>();

// Add Application services
webApplicationBuilder.Services.AddApplication();

webApplicationBuilder.Services.AddHttpContextAccessor();
webApplicationBuilder.Services.AddScoped<ICurrentUser, CurrentUser>();

// configure social login providers (Google, Microsoft, Facebook)
webApplicationBuilder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        var jwtSettings = webApplicationBuilder.Configuration.GetSection("Jwt").Get<Immotech.Api.Common.JwtSettings>();
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    })
    .AddGoogle(o =>
    {
        o.ClientId = webApplicationBuilder.Configuration["Auth:Google:ClientId"]!;
        o.ClientSecret = webApplicationBuilder.Configuration["Auth:Google:ClientSecret"]!;
        o.Scope.Add("email");
    })
    .AddMicrosoftAccount(o =>
    {
        o.ClientId = webApplicationBuilder.Configuration["Auth:Microsoft:ClientId"]!;
        o.ClientSecret = webApplicationBuilder.Configuration["Auth:Microsoft:ClientSecret"]!;
    })
    .AddFacebook(o =>
    {
        o.AppId = webApplicationBuilder.Configuration["Auth:Facebook:AppId"]!;
        o.AppSecret = webApplicationBuilder.Configuration["Auth:Facebook:AppSecret"]!;
    });
// Note: JwtBearer added later via Identity endpoints

webApplicationBuilder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.SetIsOriginAllowed(origin => true)
               .AllowAnyMethod()
               .AllowAnyHeader();      
    });
});

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

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<Domain.Entities.User>(); // Map the Identity API endpoints to the application. This adds built-in endpoints for user management.
app.MapControllers();
app.Run();