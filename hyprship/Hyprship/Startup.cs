using Hypership.Services.Identity;

using Hyprship.Data.Models;
using Hyprship.Data.Sqlite;
using Hyprship.Lib;
using Hyprship.Routes;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

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

        // builder.Services.AddOpenApi();
        // builder.Services.AddEntityFrameworkNamingConventions();
        builder.Services.AddSingleton<IPasswordHasher<User>, Hypership.Services.Identity.PasswordHasher<User>>();
        // builder.Services.AddSingleton<IEmailSender<User>, EmailSender>();
        
        switch (dbProvider)
        {
            default:
                builder.Services.AddDbContext<Db, SqliteDb>(options =>
                {
                    options.UseSnakeCaseNamingConvention();
                    options.UseSqlite("Data Source=hyprship.db");
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                });
                
                break;
        }


        /*
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
            })
            .AddHyprxEntityFrameworkStores<Db>()
            .AddRoleManager<RoleManager>()
            .AddUserManager<UserManager>()
            .AddUserStore<UserStore>()
            .AddRoleStore<RoleStore>();
        */
    }

    public static void UseHyprship(this WebApplication app)
    {
        // app.UseHttpsRedirection();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // app.MapOpenApi();
        }

        // app.MapStaticAssets();
        app.MapGet("/hello", () =>
            {
                return "hello";
            })
            .WithName("GetHello");

        // app.MapAuthEndpoints();
        // app.UseHealthChecks("/healthz");

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