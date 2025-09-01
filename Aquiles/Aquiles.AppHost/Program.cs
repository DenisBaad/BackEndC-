var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Aquiles_API>("aquiles");

builder.Build().Run();
