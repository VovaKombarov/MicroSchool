using IdentityApi.Controllers;
using IdentityApi.Data;
using IdentityApi.Models;
using IdentityApi.Services;
using IdentityApi.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware();
app.Run();

public static class ServiceInitializer
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(
            options => options.UseSqlServer(
                   configuration.GetConnectionString(
                       "IdentityApiConnectionString")));

        services.AddOptions();
        services.Configure<Dictionary<string, string>>(
            configuration.GetSection("messages"));

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddSingleton<ILogger>(provider =>
          provider.GetRequiredService<ILogger<AccountsController>>());

        services.AddIdentity<User, IdentityRole>(options => 
            options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationContext>();

        IdentityConfiguration config = new IdentityConfiguration(configuration);

        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
            .AddInMemoryApiResources(config.GetApiResources())
            .AddInMemoryApiScopes(config.GetApiScopes())
            .AddInMemoryClients(config.GetClients());

        services.AddControllers();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "IdentityApi",
                Description = ".Net 7 web abi for managing Identity service"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}

public static class MiddlewareInitializer
{
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error");
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseRouting();
        app.MapControllers();
        app.UseIdentityServer();

        return app;
    }
}
