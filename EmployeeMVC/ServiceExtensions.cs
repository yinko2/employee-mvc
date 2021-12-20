using Microsoft.AspNetCore.Cors.Infrastructure;
using EmployeeMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;

namespace EmployeeMVC.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services, IConfiguration config)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin(); // For anyone access.
            //corsBuilder.WithOrigins("http://localhost:4200","http://localhost", "http://localhost:81"); // for a specific url. Don't add a forward slash on the end!  If want to test from swaggerui, need to add swagger url here.
            // corsBuilder.WithOrigins(config.GetSection("AllowedOrigins").Value.Split(","));
            // corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllowAllPolicy", corsBuilder.Build());
            });
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        //it is for error handler of model validation exception when direct bind request parameter to model in controller function
        public static void ConfigureModelBindingExceptionHandling(this IServiceCollection services) 
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    ValidationProblemDetails? error = actionContext.ModelState
                    .Where(e => e.Value!.Errors.Count > 0)
                    .Select(e => new ValidationProblemDetails(actionContext.ModelState)).FirstOrDefault();
            
                    string ErrorMessage = "";
                    foreach (KeyValuePair<string, string[]> errobj in error!.Errors) {
                        foreach(string s in errobj.Value) {
                            ErrorMessage = ErrorMessage + s + "\r\n";
                        }
                    }
                    return new BadRequestObjectResult(new { data = 0, error = ErrorMessage});
                };
            });
        }
    }
}
