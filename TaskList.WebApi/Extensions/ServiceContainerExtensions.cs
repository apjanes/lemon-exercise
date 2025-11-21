using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskList.Domain.Repositories;
using TaskList.Infrastructure;
using TaskList.Infrastructure.Repositories;
using TaskList.WebApi.Authentication;
using TaskList.WebApi.ErrorHandling;

namespace TaskList.WebApi.Extensions;

public static class ServiceContainerExtensions
{
    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddTransient<ErrorHandlingMiddleware>();
        return services;

    }

    public static IServiceCollection AddTaskList(this IServiceCollection services)
    {
        services.AddDbContext<TaskListDbContext>(x =>
        {
            // DEBUG: Note about running from Visual Studio
            x.UseSqlite("DataSource=taskList.sqlite");
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;

    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration(configuration);
        services.AddSingleton<IJwtConfiguration>(jwtConfiguration);
        services.AddSingleton(new TokenService(jwtConfiguration));
        services.AddSingleton<IRefreshStore, InMemoryRefreshStore>();


        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = jwtConfiguration.SigningKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = jwtConfiguration.Audience,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        return services;
    }
}