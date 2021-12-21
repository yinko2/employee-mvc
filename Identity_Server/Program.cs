using Ids;
using Ids.Data;
using Ids.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Debug()
.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
.MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
.MinimumLevel.Override("System", LogEventLevel.Warning)
.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
.Enrich.FromLogContext()
// uncomment to write to Azure diagnostics stream
//.WriteTo.File(
//    @"D:\home\LogFiles\Application\identityserver.txt",
//    fileSizeLimitBytes: 1_000_000,
//    rollOnFileSizeLimit: true,
//    shared: true,
//    flushToDiskInterval: TimeSpan.FromSeconds(1))
.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
.CreateLogger();

var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}

var builder = WebApplication.CreateBuilder(args);

// get configurations from appsettings
IConfiguration _configuration = builder.Configuration;
var mysqlDbSettings = _configuration.GetSection(nameof(MysqlDbSettings)).Get<MysqlDbSettings>();
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

if (seed)
{
    Log.Information("Seeding database...");
    SeedData.EnsureSeedData(mysqlDbSettings.ConnectionString, mysqlDbSettings.ServerVersion!);
    Log.Information("Done seeding database.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(mysqlDbSettings.ConnectionString, ServerVersion.Parse(mysqlDbSettings.ServerVersion),
        sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = builder => builder.UseMySql(mysqlDbSettings.ConnectionString, ServerVersion.Parse(mysqlDbSettings.ServerVersion), opt => opt.MigrationsAssembly(migrationsAssembly));
    })
    // this adds the operational data from DB (codes, tokens, consents)
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = builder => builder.UseMySql(mysqlDbSettings.ConnectionString, ServerVersion.Parse(mysqlDbSettings.ServerVersion), opt => opt.MigrationsAssembly(migrationsAssembly));

        // this enables automatic token cleanup. this is optional.
        options.EnableTokenCleanup = true;
    })

    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();
