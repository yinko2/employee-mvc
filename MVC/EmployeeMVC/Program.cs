using EmployeeMVC.Data;
using EmployeeMVC.Extensions;
using EmployeeMVC.Services;
using EmployeeMVC.Settings;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// get configurations from appsettings
IConfiguration _configuration = builder.Configuration;
var mysqlDbSettings = _configuration.GetSection(nameof(MysqlDbSettings)).Get<MysqlDbSettings>();

// Add services to the container.
builder.Services.ConfigureCors(_configuration);

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "EmployeeMVC";
        options.Authority = "https://localhost:5443";
    });
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
        .AddCookie("cookie")
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = _configuration["InteractiveServiceSettings:AuthorityUrl"];
            options.ClientId = _configuration["InteractiveServiceSettings:ClientId"];
            options.ClientSecret = _configuration["InteractiveServiceSettings:ClientSecret"];

            options.ResponseType = "code";
            options.UsePkce = true;
            options.ResponseMode = "query";

            options.Scope.Add(_configuration["InteractiveServiceSettings:Scopes:0"]);
            options.SaveTokens = true;

        });

builder.Services.Configure<IdentityServerSettings>(_configuration.GetSection("IdentityServerSettings"));
builder.Services.AddSingleton<ITokenService, TokenService>();

//add repository
builder.Services.ConfigureRepositoryWrapper();

//add database context
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseMySql(mysqlDbSettings.ConnectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse(mysqlDbSettings.ServerVersion)));

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureModelBindingExceptionHandling();

builder.Services.AddHealthChecks()
    .AddMySql(
        mysqlDbSettings.ConnectionString,
        name: "mysqldb",
        timeout: TimeSpan.FromSeconds(3),
        tags: new[] { "ready" }
    );

builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .AddNewtonsoftJson(o => {
        o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
        o.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;       //it must be Include, otherwise default value (boolean=false, int=0, int?=null, object=null) will be missing in response json			
        o.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });

var app = builder.Build();

//allow cors policy
app.UseCors("CorsAllowAllPolicy");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = (check) => check.Tags.Contains("ready"),
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(
                new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                        duration = entry.Value.Duration.ToString()
                    })
                }
            );

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
    {
        Predicate = (_) => false
    });
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
