var builder = DistributedApplication.CreateBuilder(args);
var api = builder.AddProject<Projects.API>("Booking API");


builder
    .AddNpmApp("Portal", "../../Portal")
    .WithHttpEndpoint(port: 5000, targetPort: 4200);

builder.Build().Run();
