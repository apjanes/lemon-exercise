using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskList.Domain.Repositories;
using TaskList.Infrastructure;
using TaskList.Infrastructure.Repositories;
using TaskList.WebApi.Authentication;

namespace TaskList.WebApi.Extensions;

public static class ServiceContainerExtensions
{
    public static IServiceCollection AddTaskList(this IServiceCollection services)
    {
        services.AddDbContext<TaskListDbContext>(x =>
        {
            x.UseSqlite("DataSource=taskList.sqlite");
        });

        services.AddScoped<IWorkItemRepository, WorkItemRepository>();

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
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        return services;
    }
}