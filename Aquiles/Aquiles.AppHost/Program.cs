var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Aquiles_API>("aquiles-api");

builder.AddProject<Projects.Enderecos_API>("enderecos-api");

builder.Build().Run();
