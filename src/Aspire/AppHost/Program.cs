using Projects;
var builder = DistributedApplication.CreateBuilder(args);
var api = builder.AddProject<Booking_API>("Booking");


builder
    .AddNpmApp("Portal", "../../Portal")
    .WithHttpEndpoint(port: 5000, targetPort: 4200);

builder.Build().Run();
