using RoadGuard.Data;
using RoadGuard.Services;
using RoadGuard.Repositories;
using RoadGuard.Infrastructure;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder( args );

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>( options =>
    options.UseNpgsql( builder.Configuration.GetConnectionString( "DefaultConnection" ) )
          .UseSnakeCaseNamingConvention()
          .EnableSensitiveDataLogging( builder.Environment.IsDevelopment() )
          .EnableDetailedErrors( builder.Environment.IsDevelopment() )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
  c.SwaggerDoc( "v1", new OpenApiInfo
  {
    Title = "RoadGuard API",
    Version = "v1"
  } );
} );

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<DriverRatingRepository>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RatingService>();

builder.Services.AddScoped<ApplicationInitializer>();

builder.Services.AddControllers();

var app = builder.Build();

try
{
  using (var scope = app.Services.CreateScope())
  {
    var initLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    initLogger.LogInformation( "🚀 Starting database initialization..." );

    var initializer = scope.ServiceProvider.GetRequiredService<ApplicationInitializer>();
    await initializer.InitializeDatabaseAsync().ConfigureAwait( false );

    initLogger.LogInformation( "✅ Database initialization completed successfully" );
  }
}
catch (Exception ex)
{
  var errorLogger = app.Services.GetRequiredService<ILogger<Program>>();
  errorLogger.LogError( ex, "❌ Failed to initialize database" );
  throw;
}

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI( c =>
  {
    c.SwaggerEndpoint( "/swagger/v1/swagger.json", "RoadGuard API v1" );
    c.RoutePrefix = "swagger";
  } );
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
  app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile( "index.html" );

var appLogger = app.Services.GetRequiredService<ILogger<Program>>();
appLogger.LogInformation( "🎯 RoadGuard API is ready to start!" );

await app.RunAsync().ConfigureAwait( false );