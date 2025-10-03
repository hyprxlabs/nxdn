using Hyprship.Auth;
using Hyprship.Data.Models;
using Hyprship.Lib;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

namespace Hyprship;

public static class Startup
{
    public static void AddHyprship(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        var dbProvider = Environment.GetEnvironmentVariable("HS_DB_PROVIDER");
        if (string.IsNullOrWhiteSpace(dbProvider))
            dbProvider = builder.Configuration["DB:Provider"];

        if (string.IsNullOrWhiteSpace(dbProvider))
            dbProvider = "sqlite";

        builder.Services.AddOpenApi();
        builder.Services.AddAuthentication();
        builder.Services.AddDbContext<Db>(o =>
        {
            o.UseSnakeCaseNamingConvention();
            o.UseSqlite("Data Source=./data/hyprship.db");
        });

        builder.Services.AddIdentity<User, Role>((o) =>
        {
            o.SignIn.RequireConfirmedEmail = true;
            o.SignIn.RequireConfirmedPhoneNumber = false;

            o.Lockout.AllowedForNewUsers = true;
            o.Password.RequiredLength = 12;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireDigit = true;
            o.User.RequireUniqueEmail = true;
            o.Stores.MaxLengthForKeys = 128;
        });
    }

    public static void UseHyprship(this WebApplication app)
    {
        app.UseHttpsRedirection();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapStaticAssets();
        app.MapGet("/hello", () =>
            {
                return "hello";
            })
            .WithName("GetHello");

        app.MapAuthEndpoints();

        app.UseHealthChecks("/healthz");

        app.UseExceptionHandler(ehApp =>
        {
            ehApp.Run(async ctx =>
            {
                var eh = ctx.Features.Get<IExceptionHandlerFeature>();
                ctx.Response.ContentType = "application/json";
                var err = new Error("Unknown error");

                if (eh is not null)
                {
                    err = Error.Convert(eh.Error);
                    await Results.BadRequest(err).ExecuteAsync(ctx);
                    return;
                }

                var efh = ctx.Features.Get<IExceptionHandlerPathFeature>();
                if (efh is not null)
                {
                    err = Error.Convert(efh.Error);
                    await Results.NotFound(err).ExecuteAsync(ctx);
                    return;
                }

                await Results.InternalServerError(err).ExecuteAsync(ctx);
            });
        });
    }
}