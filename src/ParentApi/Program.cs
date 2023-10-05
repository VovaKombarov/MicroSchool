using Common.Api.Exstensions;
using Common.EventBus;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParentApi.Controllers;
using ParentApi.Data;
using ParentApi.IntegrationEvents;
using ParentApi.IntegrationEvents.EventHandling;
using ParentApi.IntegrationEvents.Events;
using ParentApi.IntegrationEvents.Services;
using ParentApi.Models;
using ParentApi.Services;
using ParentApi.Utilities;
using System;
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
        services.AddDbContext<AppDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString(
                "ParentApiConnectionString"))
            .UseLowerCaseNamingConvention());

        services.AddServiceDefaults(configuration);

        services.AddScoped<IParentService, ParentService>();
        services.AddScoped<IParentIntegrationEventService, ParentIntegrationEventService>();

        services.AddScoped<IRepository<Lesson>, Repository<Lesson>>();
        services.AddScoped<IRepository<Student>, Repository<Student>>();
        services.AddScoped<IRepository<TeacherParentMeeting>, Repository<TeacherParentMeeting>>();
        services.AddScoped<IRepository<Teacher>, Repository<Teacher>>();
        services.AddScoped<IRepository<Parent>, Repository<Parent>>();
        services.AddScoped<IRepository<StudentInLesson>, Repository<StudentInLesson>>();
        services.AddScoped<IRepository<CompletedHomework>, Repository<CompletedHomework>>();
        services.AddScoped<IRepository<Subject>, Repository<Subject>>();
        services.AddScoped<IRepository<Class>, Repository<Class>>();
        services.AddScoped<IRepository<TeacherClassSubject>, Repository<TeacherClassSubject>>();
        services.AddScoped<IRepository<HomeworkStatus>, Repository<HomeworkStatus>>();
        services.AddScoped<IRepository<HomeworkProgressStatus>, Repository<HomeworkProgressStatus>>();
        services.AddScoped<IRepository<Homework>, Repository<Homework>>();

        services.AddTransient<CreateLessonIntegrationEventHandler>();
        services.AddTransient<CreateTeacherParentMeetingIntegrationEventHandler>();
        services.AddTransient<CreateHomeworkIntegrationEventHandler>();
        services.AddTransient<ChangeStatusHomeworkEventHandler>();
        services.AddTransient<GradeHomeworkEventHandler>();
        services.AddTransient<CreateCommentEventHandler>();
        services.AddTransient<GradeStudentInLessonEventHandler>();
        services.AddTransient<RemoveTeacherParentMeetingEventHandler>();

        services.AddSingleton<ILogger>(provider =>
           provider.GetRequiredService<ILogger<ParentsController>>());

        AuthenticationHandler authenticator = new AuthenticationHandler(configuration);
        services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
           .AddJwtBearer("Bearer", options =>
           {
               authenticator.JwtBerarerAuthentication(options);
           });

        // Проверяем наличие области действия в токене доступа
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", configuration["AllowedScopes:Parent"]);

            });
        });

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ParentApi",
                Description = ".Net 7 web abi for managing parent service"
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
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var eventBus = app.Services.GetRequiredService<IEventBus>();

        eventBus.Subscribe<
            CreateTeacherParentMeetingEvent,
            CreateTeacherParentMeetingIntegrationEventHandler>();

        eventBus.Subscribe<CreateLessonEvent,
            CreateLessonIntegrationEventHandler>();

        eventBus.Subscribe<CreateHomeworkEvent,
          CreateHomeworkIntegrationEventHandler>();

        eventBus.Subscribe<ChangeStatusHomeworkEvent,
            ChangeStatusHomeworkEventHandler>();

        eventBus.Subscribe<
            GradeHomeworkEvent,
            GradeHomeworkEventHandler>();

        eventBus.Subscribe<
           CreateCommentEvent,
           CreateCommentEventHandler>();

        eventBus.Subscribe<
             GradeStudentInLessonEvent,
             GradeStudentInLessonEventHandler>();

        eventBus.Subscribe<
            RemoveTeacherParentMeetingEvent,
            RemoveTeacherParentMeetingEventHandler>();


        if (app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/error");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ParentsService v1"));
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers().RequireAuthorization("ApiScope");

        return app;
    }
}


