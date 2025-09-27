var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Hyprship_Api>("api")
    .WithHttpHealthCheck("/health");

builder.Build().Run();