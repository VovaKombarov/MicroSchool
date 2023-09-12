using Common.Api.Exstensions;
using Common.EventBus;
using IdentityModel;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeacherApi;
using TeacherApi.Controllers;
using TeacherApi.Data;
using TeacherApi.IntegrationEvents;
using TeacherApi.IntegrationEvents.EventHandling;
using TeacherApi.IntegrationEvents.Events;
using TeacherApi.IntegrationEvents.Services;
using TeacherApi.Models;
using TeacherApi.Services;
using TeacherApi.Utilities;
using System.IO;
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
        ConfigurationManager configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString(
                "TeacherApiConnectionString")));

        services.AddServiceDefaults(configuration);

        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<ITeacherIntegrationEventService, TeacherIntegrationEventService>();
        services.AddScoped<IRepository<Class>, Repository<Class>>();
        services.AddScoped<IRepository<Student>, Repository<Student>>();
        services.AddScoped<IRepository<TeacherClassSubject>, Repository<TeacherClassSubject>>();
        services.AddScoped<IRepository<Parent>, Repository<Parent>>();
        services.AddScoped<IRepository<Lesson>, Repository<Lesson>>();
        services.AddScoped<IRepository<StudentInLesson>, Repository<StudentInLesson>>();
        services.AddScoped<IRepository<Homework>, Repository<Homework>>();
        services.AddScoped<IRepository<HomeworkProgressStatus>, Repository<HomeworkProgressStatus>>();
        services.AddScoped<IRepository<Teacher>, Repository<Teacher>>();
        services.AddScoped<IRepository<Subject>, Repository<Subject>>();
        services.AddScoped<IRepository<CompletedHomework>, Repository<CompletedHomework>>();
        services.AddScoped<IRepository<HomeworkStatus>, Repository<HomeworkStatus>>();
        services.AddScoped<IRepository<TeacherParentMeeting>, Repository<TeacherParentMeeting>>();

        services.AddTransient<CreateParentTeacherMeetingIntegrationEventHandler>();
        services.AddTransient<RemoveParentTeacherMeetingIntegrationEventHandler>();

        services.AddSingleton<ILogger>(provider =>
            provider.GetRequiredService<ILogger<TeachersController>>());

        Authenticator authenticator = new Authenticator(configuration);
        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            // Если необходим отзыв token, надо использовать Referense token.
            // Не забывать переключать тип token Client в IdentityConfiguration. 
            .AddIdentityServerAuthentication(options =>
            {
                authenticator.ReferenseTokenAuthentication(options);
            });
            // Если отзыв token не нужен, используем JWT token.
            //.AddJwtBearer("Bearer", options =>
            //{
            //    JwtBerarerAuthentication(options);
            //});


        // Проверяем наличие области действия в токене доступа
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();

                //policy.RequireClaim("scope", configuration["ApiScope:Teacher"]);
            });
        });

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "TeacherApi",
                Description = ".Net 7 web abi for managing teacher service"
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeachersService v1"));
        }

        var eventBus = app.Services.GetRequiredService<IEventBus>();

        eventBus.Subscribe<CreateParentTeacherMeetingEvent,
            CreateParentTeacherMeetingIntegrationEventHandler>();

        eventBus.Subscribe<RemoveParentTeacherMeetingEvent,
            RemoveParentTeacherMeetingIntegrationEventHandler>();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization("ApiScope");

        return app;
    }
}




