using Hyprship;
using Hyprship.Auth;
using Hyprship.Data.Models;
using Hyprship.Lib;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = "public",
});

builder.AddHyprship();

var app = builder.Build();

app.UseHyprship();

app.Run();