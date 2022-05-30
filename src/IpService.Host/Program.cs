using System.Data.SqlClient;
using Hangfire;
using Hangfire.Dashboard;
using IpService;
using IpService.Contracts.Interfaces;
using IpService.Data;
using IpService.Data.DbContexts;
using IpService.Host;
using IPStackIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Refit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Logging.AddConsole();

// Configure IPStack configuration.
builder.Services.Configure<IpStackOptions>(builder.Configuration.GetSection(IpStackOptions.SectionName));

// Refit client.
builder.Services.AddRefitClient<IIpStackClient>().ConfigureHttpClient(client => client.BaseAddress =
    new Uri(builder.Configuration.GetSection(IpStackOptions.SectionName).Get<IpStackOptions>().BaseUrl));

// Register services with dependency injection.
builder.Services.AddScoped<IIpInfoProvider, IpInfoProvider>();

// Register dbContext.
builder.Services.AddDbContext<IpServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Register repository.
builder.Services.AddScoped<IIpServiceRepository, IpServiceRepository>();

// Register memory cache provider.
builder.Services.AddSingleton<ICacheStore>(services => new MemoryCacheStore(services.GetService<IMemoryCache>()));

// Register hangfire.
builder.Services.AddHangfire(
    config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire")));
builder.Services.AddHangfireServer();

// Register swagger.
builder.Services.AddSwaggerGen();

// Serilog.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Do the database migration.
MigrateDatabase(builder.Configuration);

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new HangfireAuthorizationFilter()
    }
});
app.UseSwagger();
app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "docs";
    }
);
app.Run();

void MigrateDatabase(IConfiguration configuration)
{
    try
    {
        var cnx = new SqlConnection(configuration.GetConnectionString("Migration"));
        var createDatabaseEvolve = new Evolve.Evolve(cnx, msg => Log.Information(msg))
        {
            IsEraseDisabled = true,
            Locations = new[] { "db/create" }
        };

        createDatabaseEvolve.Migrate();

        var connection = new SqlConnection(configuration.GetConnectionString("Default"));
        var migrationEvolve = new Evolve.Evolve(connection, msg => Log.Information(msg))
        {
            IsEraseDisabled = true,
            Locations = new[] { "db/migrations" }
        };

        migrationEvolve.Migrate();
    }
    catch (Exception ex)
    {
        Log.Error("Database migration failed.", ex);
        throw;
    }
}