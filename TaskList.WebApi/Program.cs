using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskList.Domain.Entities;
using TaskList.Infrastructure;
using TaskList.WebApi.ErrorHandling;
using TaskList.WebApi.Extensions;
using TaskList.WebApi.Filters;

namespace TaskList.WebApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTaskList();
        builder.Services.AddMiddleware();
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        builder.Services.AddControllers(x => x.Filters.Add<FluentValidationActionFilter>());
        builder.Services.AddControllers();
        builder.Services.Configure<ApiBehaviorOptions>(x => x.SuppressModelStateInvalidFilter = true);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddJwtAuthentication(builder.Configuration);
        builder.Services.AddAuthorization();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost", policy =>
            {
                policy
                    .WithOrigins("https://localhost:4000")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowLocalhost");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await InitialiseData(app.Services);
        await app.RunAsync();
    }

    /// <summary>
    /// DEBUG: finish comment. Create user registration if possible.
    /// Initialize dummy data required for running the exercise. This approach would not be used for any real-life
    /// system but exists to manage 

    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static async Task InitialiseData(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var hasher = scopedServices.GetRequiredService<IPasswordHasher>();
            var dbContext = scopedServices.GetRequiredService<TaskListDbContext>();

            await dbContext.Database.MigrateAsync();

            var fred = new User
            {
                Id = new Guid("0000019a-a11c-f278-f6dc-915e81c6b2d0"),
                FirstName = "Fred",
                LastName = "Flintstone",
                Username = "fred",
                PasswordHash = hasher.Hash("flintstone")
            };

            var barney = new User
            {
                Id = new Guid("0000019a-a11c-f279-00ea-7ec897888d30"),
                FirstName = "Barney",
                LastName = "Rubble",
                Username = "barney",
                PasswordHash = hasher.Hash("rubble")
            };

            await AddIfMissingAsync(fred, dbContext);
            await AddIfMissingAsync(barney, dbContext);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task AddIfMissingAsync(User user, TaskListDbContext dbContext)
    {
        var existing = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == user.Username);

        if (existing == null)
        {
            dbContext.Users.Add(user);
        }
    }
}