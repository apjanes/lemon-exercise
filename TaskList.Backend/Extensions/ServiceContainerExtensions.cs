using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskList.Backend.Authentication;
using TaskList.Backend.Data;
using TaskList.Backend.Data.Repositories;

namespace TaskList.Backend.Extensions;

public static class ServiceContainerExtensions
{
    public static IServiceCollection AddTaskList(this IServiceCollection services)
    {
        services.AddDbContext<TaskListDbContext>(x =>
        {
            x.UseInMemoryDatabase("taskList");
        });

        services.AddScoped<IWorkItemRepository, WorkItemRepository>();

        return services;

    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = new JwtConfiguration(configuration);
        services.AddSingleton<IJwtConfiguration>(jwtConfiguration);
        services.AddSingleton<TokenService>(new TokenService(jwtConfiguration));
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
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}